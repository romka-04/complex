using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
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
            await _fixture.PublishMessage(index);
            // act
            await Task.Delay(TimeSpan.FromSeconds(10)); // give time to proceed
            // assert
            _fixture.AssertMessageUpdated();
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
        _fixture = new WorkerTestsFixture();
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
    public RedisOptions CreateRadisOptions()
    {
        throw new NotImplementedException();
    }

    public Worker CreateSut()
    {
        throw new NotImplementedException();
    }

    public Task PublishMessage(int message)
        => PublishMessage(message.ToString());

    public async Task PublishMessage(string message)
    {
        throw new NotImplementedException();
    }

    public async Task AssertMessageUpdated()
    {
        throw new NotImplementedException();
    }
}