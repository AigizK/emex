using System;
using System.Text;
using System.Timers;
using System.IO;
using ServiceStack;

namespace EmexStatistic.Models
{
    public class FileLogger
    {
        public void RepeatSave()
        {
            var tmLink = new Timer { Interval = 1 * 10 * 1000 };
            tmLink.Elapsed += (sender, e) =>
            {
                var timer = sender as Timer;
                timer.Stop();
                try
                {
                    SaveLogs();
                }
                catch (Exception ex)
                {
                }
                timer.Start();
            };
            tmLink.Start();
        }

        private void SaveLogs()
        {
            var logPath = Path.Combine(SiteUtility.SiteFolder, "Log", "stat.log");
            var items = new MemoryStore().TryRemoveItems();
            if (items == null)
                return;
            using (var sw = new StreamWriter(logPath, true, Encoding.UTF8))
            {
                foreach (var item in items)
                    sw.WriteLine(item.ToJson());
            }
        }
    }
}