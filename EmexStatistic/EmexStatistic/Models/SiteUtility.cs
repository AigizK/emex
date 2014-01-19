using System.Web;

namespace EmexStatistic.Models
{
    public class SiteUtility
    {
        public static string SiteFolder
        {
            get { return HttpRuntime.BinDirectory.ToLower().Replace("\\bin\\", "\\"); }
        } 
    }
}