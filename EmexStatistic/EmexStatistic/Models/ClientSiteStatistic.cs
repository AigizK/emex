using System;

namespace EmexStatistic.Models
{
    public class ClientSiteStatistic
    {
        public long Login { get; set; }
        public string Url { get; set; }
        public string Refferer { get; set; }
        public string Ip { get; set; }
        public Guid Uid { get; set; }
        public Guid Sid { get; set; } 
        public DateTime Date { get; set; }
    }
}