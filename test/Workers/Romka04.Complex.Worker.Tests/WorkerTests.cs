using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;

namespace Romka04.Complex.Worker;

[TestFixture]
public class WorkerTests
{
    [Test]
    public async Task ExecuteAsync_NewValueAddedToRedis_ShouldCalculateFabNumber()
    {
        // arrange
        const int index = 19;
        const int expected = 4181; // Fibonacci number for index 19

        var sut = _fixture.CreateSut();
        try
        {
            await sut.StartAsync(CancellationToken.None);
            await Task.Delay(1);
            await _fixture.PublishMessage(index);
            // act
            await Task.Delay(TimeSpan.FromSeconds(5)); // give time to proceed
            // assert
            await _fixture.AssertMessageUpdated(index, expected);
        }
        finally
        {
            await sut.StopAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
        }
    }

    #region Test Helpers

    private RedisTestcontainer _testcontainers;
    private WorkerTestsFixture _fixture;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        _testcontainers = new TestcontainersBuilder<RedisTestcontainer>()
            .WithDatabase(new RedisTestcontainerConfiguration())
            .Build();
        await _testcontainers.StartAsync().ConfigureAwait(false);
    }

    [SetUp]
    public void SetUp()
    {
        _fixture = new WorkerTestsFixture(_testcontainers);
    }

    [TearDown]
    public async Task TearDown()
    {
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _testcontainers.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}

internal class WorkerTestsFixture
{
    private readonly string _publishChannel = Guid.NewGuid().ToString("N");
    private readonly RedisTestcontainer _redisTestcontainer;

    public WorkerTestsFixture(RedisTestcontainer redisTestcontainer)
    {
        _redisTestcontainer = redisTestcontainer
            ?? throw new ArgumentNullException(nameof(redisTestcontainer));
    }

    public Worker CreateSut()
    {
        var redisOptions = new RedisOptions
        {
            Configuration  = _redisTestcontainer.ConnectionString,
            PublishChannel = _publishChannel 
        };

        var opt = Mock.Of<IOptions<RedisOptions>>(x => x.Value == redisOptions);

        return new Worker(opt);
    }

    public Task PublishMessage(int message)
        => PublishMessage(message.ToString());

    public async Task PublishMessage(string message)
    {
        using ConnectionMultiplexer redis =
            await CreateConnectAsync();
        ISubscriber subScriber = redis.GetSubscriber();

        await subScriber.PublishAsync(_publishChannel, message, CommandFlags.FireAndForget);

        await TestContext.Out.WriteLineAsync($"Message '{message}' successfully sent to '{_publishChannel}'");
    }

    private async Task<ConnectionMultiplexer> CreateConnectAsync()
    {
        return await ConnectionMultiplexer.ConnectAsync(_redisTestcontainer.ConnectionString);
    }

    public Task AssertMessageUpdated(int message, int expected)
        => AssertMessageUpdated(message.ToString(), expected.ToString());

    public async Task AssertMessageUpdated(string message, string expected)
    {
        using ConnectionMultiplexer redis =
            await CreateConnectAsync();

        IDatabase db = redis.GetDatabase();
        var hashSet = db.HashGet("values", message);

        hashSet.HasValue.Should().BeTrue();

        var actual = hashSet.ToString();
        actual.Should().Be(expected);
    }
}