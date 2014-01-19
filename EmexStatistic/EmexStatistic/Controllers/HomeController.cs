using System;
using System.Web;
using System.Web.Mvc;
using EmexStatistic.Models;

namespace EmexStatistic.Controllers
{
    public class HomeController : Controller
    {
        public FileResult Index()
        {
            string s = HttpUtility.UrlDecode(Request.QueryString.ToString());
            if (!string.IsNullOrEmpty(s))
                Save(s);

            return File(Request.PhysicalApplicationPath + "img\\s.png", "image/png");
        }

        private void Save(string s)
        {
            string[] param = s.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);


            var liveStatistic = new ClientSiteStatistic();

            foreach (string p in param)
            {
                if (p.ToLower().StartsWith("r"))
                {
                    liveStatistic.Refferer = p.Substring(1);
                    continue;
                }

                if (p.ToLower().StartsWith("l"))
                {
                    long userLogin;
                    if (!long.TryParse(p.Substring(1), out userLogin))
                        userLogin = 0;
                    liveStatistic.Login = userLogin;
                    continue;
                }

                if (p.ToLower().StartsWith("u"))
                    liveStatistic.Url = p.Substring(1);
            }

            liveStatistic.Date = DateTime.Now;

            Response.AddHeader("P3P", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            
            //постоянный куки,чтоб узнать что это один и тот же пользователь
            bool cookieExist = false;
            if (Request.Cookies["uid_stat"] != null)
            {
                try
                {
                    liveStatistic.Uid = new Guid(Request.Cookies["uid_stat"].Value);
                    cookieExist = true;
                }
                catch (Exception)
                {
                }
            }

            if (!cookieExist)
            {
                liveStatistic.Uid = Guid.NewGuid();
                var cookie = new HttpCookie("uid_stat", liveStatistic.Uid.ToString())
                {
                    Expires = DateTime.Now.AddYears(1),
                    Domain = ".emex-stat.ru",
                    Path = ""
                };
                Response.Cookies.Add(cookie);
            }

            //сессионный куки,чтоб знать как часто посещает
            bool sessionCookieExist = false;
            if (Request.Cookies["ses_uid"] != null)
            {
                try
                {
                    liveStatistic.Sid = new Guid(Request.Cookies["ses_uid"].Value);
                    sessionCookieExist = true;
                }
                catch (Exception)
                {
                }
            }

            if (!sessionCookieExist)
            {
                liveStatistic.Sid = Guid.NewGuid();
                var cookie = new HttpCookie("ses_uid", liveStatistic.Sid.ToString())
                {
                    Domain = ".emex-stat.ru"
                };

                Response.Cookies.Add(cookie);
            }

            liveStatistic.Ip = Request.UserHostAddress;
            new MemoryStore().Add(liveStatistic);
        }
    }
}