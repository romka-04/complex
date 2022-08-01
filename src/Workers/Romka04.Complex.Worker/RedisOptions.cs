using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Romka04.Complex.Worker
{
    internal class RedisOptions
    {
        public const string Name = "Redis";


        public string Configuration { get; set; } = String.Empty;
        public string PublishChannel { get; set; } = String.Empty;

        public string GetConfiguration()
        {
            if (string.IsNullOrWhiteSpace(Configuration))
                throw new Exception(
                    "Please populate connection string in 'appsettings.json' file in order to be able initialize redis");

            return Configuration;
        }

        public string GetPublishChannel()
        {
            if (string.IsNullOrWhiteSpace(PublishChannel))
                throw new Exception(
                    "Please populate publish channel info in 'appsettings.json' file in order to be able initialize redis");

            return PublishChannel;
        }
    }
}
