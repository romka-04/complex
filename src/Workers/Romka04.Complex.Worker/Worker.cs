using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Romka04.Complex.Worker
{
    internal class Worker
        : BackgroundService
    {
        private readonly RedisOptions _redisOptions;

        public Worker(IOptions<RedisOptions> options)
        {
            if (null == options)
                throw new ArgumentNullException(nameof(options));
            _redisOptions = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using ConnectionMultiplexer redis =
                await ConnectionMultiplexer.ConnectAsync(_redisOptions.GetConfiguration());
            ISubscriber subScriber = redis.GetSubscriber();

            Console.WriteLine($"Subscribe to the channel '{_redisOptions.PublishChannel}'");

            await subScriber.SubscribeAsync(_redisOptions.GetPublishChannel(), (channel, message) =>
            {
                //Output received message
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}]: {$"Message {message} received successfully"}");
            });

            await Task.Delay(int.MaxValue, stoppingToken);
        }
    }
}
