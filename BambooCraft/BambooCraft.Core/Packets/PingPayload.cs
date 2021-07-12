using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft.Core.Packets
{
    #region Server ping 
    /// <summary>
    /// C# represenation of the following JSON file
    /// https://gist.github.com/thinkofdeath/6927216
    /// </summary>
    /// 
    public class PingPayload
    {
        [JsonProperty(PropertyName = "version")]
        public VersionPayload versionPayload { get; set; } = new VersionPayload();
        [JsonProperty(PropertyName = "players")]
        public PlayerPayload playerPayload { get; set; } = new PlayerPayload();
        [JsonProperty(PropertyName = "description")]
        public DescriptionPayload descriptionPayload { get; set; } = new DescriptionPayload();
        public string favicon { get; set; } = "";
    }
    public class VersionPayload
    {
        public string name { get; set; } = "";
        public int protocol { get; set; } = 0;
    }

    public class PlayerPayload
    {
        public int max { get; set; } = 0;
        public int online { get; set; } = 0;
        public List<Player> sample { get; set; } = new List<Player>();
    }

    public class Player
    {
        public string name { get; set; } = "";
        public string id { get; set; } = "";
    }

    public class DescriptionPayload
    {
        public string text { get; set; } = "";
    }
    #endregion
}
