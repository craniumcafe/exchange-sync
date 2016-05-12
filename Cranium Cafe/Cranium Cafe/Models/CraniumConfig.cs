using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraniumCafeConfig.Models
{
    class CraniumConfig
    {
        public string RestKey { get; set; }
        public string RestSecret { get; set; }
        public string AgentUsername { get; set; }
        public string AgentPassword { get; set; }
        public string ExcServerName { get; set; }
        public string PoolingInterval { get; set; }
        public List<UserDetails> UserDetailsList { get; set; }
    }
}
