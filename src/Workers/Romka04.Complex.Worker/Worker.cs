using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Romka04.Complex.Worker
{
    internal class Worker
        : BackgroundService
    {
        private readonly IOptions<RedisConfig> _redisOptions;

        public Worker(IOptions<RedisConfig> redisOptions)
        {
            _redisOptions = redisOptions ?? throw new ArgumentNullException(nameof(redisOptions));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using ConnectionMultiplexer redis =
                ConnectionMultiplexer.Connect(_redisOptions.Value.Configuration);
            ISubscriber subScriber = redis.GetSubscriber();

            Console.WriteLine("Subscribe to the channel 'thecode - buzz - channel'");

            return subScriber.SubscribeAsync(_redisOptions.Value.PublishChannel, (channel, message) =>
            {
                //Output received message
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}]: {$"Message {message} received successfully"}");
            });
        }
    }
}
