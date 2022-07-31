using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Romka04.Complex.Core;
using StackExchange.Redis;

namespace Romka04.Complex.Worker
{
    internal class Worker
        : BackgroundService
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyRedis;

        private readonly RedisOptions _redisOptions;

        private ConnectionMultiplexer Redis => _lazyRedis.Value;

        public Worker(IOptions<RedisOptions> options)
        {
            if (null == options)
                throw new ArgumentNullException(nameof(options));
            _redisOptions = options.Value;
            _lazyRedis = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_redisOptions.GetConfiguration());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ISubscriber subScriber = Redis.GetSubscriber();

            Console.WriteLine($"Subscribe to the channel '{_redisOptions.PublishChannel}'");

            await subScriber.SubscribeAsync(_redisOptions.GetPublishChannel(), (channel, message) =>
            {
                // calc Fibonacci number
                var idx = int.Parse(message);
                var fab = FabCalculator.Fab(idx);

                var db = Redis.GetDatabase();
                db.HashSet("values", new HashEntry[] { new(message, fab) });
            });

            await Task.Delay(int.MaxValue, stoppingToken);
        }

        public override void Dispose()
        {
            if (_lazyRedis.IsValueCreated)
            {
                Redis.Dispose();
            }
            base.Dispose();
        }
    }
}
