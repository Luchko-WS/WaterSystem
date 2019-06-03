using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServicesSyncTests
{
    [TestClass]
    public class MonitoringDavrGovUaServiceTest
    {
        [TestMethod]
        public void GetReport()
        {
            var siteUrl = "http://monitoring.davr.gov.ua/EcoWaterMon/GDKMap/Index";
            HttpWebRequest inviteHttpRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
            inviteHttpRequest.Method = WebRequestMethods.Http.Get;
            HttpWebResponse inviteHttpResponse = (HttpWebResponse)inviteHttpRequest.GetResponse();

            var setCookieHeaderValue = inviteHttpResponse.Headers.Get(5).Split(';');
            var sessionId = setCookieHeaderValue[0].Split('=');

            var url = "http://monitoring.davr.gov.ua/Reporting/Reporting/Export/393?dxrep_fake=&queryParams=[{\"Key\":\"p0\",\"Value\":\"02.05.2010\"},{\"Key\":\"p1\",\"Value\":\"24.05.2019\"},{\"Key\":\"p2\",\"Value\":\"400\"},{\"Key\":\"p3\",\"Value\":\"0\"},{\"Key\":\"p4\",\"Value\":\"0\"}]&mode=False";
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = WebRequestMethods.Http.Get;
            if(httpRequest.CookieContainer == null) httpRequest.CookieContainer = new CookieContainer();
            httpRequest.CookieContainer.Add(new Cookie(sessionId[0], sessionId[1], "/", "monitoring.davr.gov.ua"));
            Cookie authCookie = new Cookie(".ASPXFORMSDSBASEAUTHADMIN", "09EC56306A574E48031BE4B286DD509C31398907F4C63FE75C209EC60E80BEE2A93400A82182CE2EF7818636BFFD50A922976063B5A0C6B14EAC36B41AAAF2C0DA6C915CD0A7910CBE4D2F0E57BF654ABA4317377B39F7E75FD1494130C497C489271FD3FBED868D16DE21DE3B17B38D", "/", "monitoring.davr.gov.ua");
            authCookie.Secure = true;
            httpRequest.CookieContainer.Add(authCookie);

            HttpWebResponse httpResponse = (HttpWebResponse)inviteHttpRequest.GetResponse();

            //need always manually set .ASPXFORMSDSBASEAUTHADMIN cookie!
        }
    }
}
