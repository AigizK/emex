using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace EmexStatistic.Models
{
    public class MemoryStore
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public void Add(ClientSiteStatistic statistic)
        {
            var statistics = Cache["SiteStatictics"] as ConcurrentDictionary<Guid, ClientSiteStatistic> ??
                             new ConcurrentDictionary<Guid, ClientSiteStatistic>();

            statistics.AddOrUpdate(Guid.NewGuid(), x => statistic, (s, old) => statistic);
            Set("SiteStatictics", statistics, 60);
        }

        private void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        public IEnumerable<ClientSiteStatistic> TryRemoveItems()
        {
            var statistics = Cache["SiteStatictics"] as ConcurrentDictionary<Guid, ClientSiteStatistic> ??
                             new ConcurrentDictionary<Guid, ClientSiteStatistic>();

            if (statistics.IsEmpty)
                return null;

            var keys = statistics.Select(x => x.Key).ToArray();
            var result = new List<ClientSiteStatistic>();

            foreach (var key in keys)
            {
                ClientSiteStatistic statistic;
                if(statistics.TryRemove(key,out statistic))
                    result.Add(statistic);
            }

            return result;
        }
    }
}