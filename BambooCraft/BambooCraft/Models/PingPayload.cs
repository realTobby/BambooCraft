using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Models
{
    
    public class PingPayload
    {
        [JsonProperty(PropertyName = "version")]
        public VersionPayload vpl = new VersionPayload();
        [JsonProperty(PropertyName = "players")]
        public PlayerPayload ppl = new PlayerPayload();
        [JsonProperty(PropertyName = "description")]
        public DescriptionPayload dpl = new DescriptionPayload();
        public string favicon = "";
    }
    public class VersionPayload
    {
        public string name = "";
        public int protocol = 0;
    }

    public class PlayerPayload
    {
        public int max = 0;
        public int online = 0;
        public List<Player> sample = new List<Player>();
    }

    public class Player
    {
        public string name = "";
        public string id = "";
    }

    public class DescriptionPayload
    {
        public string text = "";
    }

}
