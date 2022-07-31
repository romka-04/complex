using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Romka04.Complex.Worker
{
    internal class RedisConfig
    {
        public string Configuration { get; set; }
        public string PublishChannel { get; set; }
    }
}
