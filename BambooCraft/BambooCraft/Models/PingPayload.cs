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
        public VersionPayload vpl { get; set; }  = new VersionPayload();
        [JsonProperty(PropertyName = "players")]
        public PlayerPayload ppl { get; set; }  = new PlayerPayload();
        [JsonProperty(PropertyName = "description")]
        public DescriptionPayload dpl { get; set; } = new DescriptionPayload();
        public string favicon { get; set; }  = "";
    }
    public class VersionPayload
    {
        public string name { get; set; } = "";
        public int protocol { get; set; }  = 0;
    }

    public class PlayerPayload
    {
        public int max { get; set; }  = 0;
        public int online { get; set; }  = 0;
        public List<Player> sample { get; set; }  = new List<Player>();
    }

    public class Player
    {
        public string name { get; set; }  = "";
        public string id { get; set; }  = "";
    }

    public class DescriptionPayload
    {
        public string text { get; set; }  = "";
    }

}
