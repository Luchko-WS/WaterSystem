using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SyncOpenDateServices.TextyOrgUaWater
{
    public class TextyOrgUaWaterService
    {
        IStreamParser<TextyOrgUaWaterRow> _parser;

        public TextyOrgUaWaterService()
        {
            //default parser
            _parser = new TextyOrgUaWaterServiceParser();
        }

        public TextyOrgUaWaterService(IStreamParser<TextyOrgUaWaterRow> parser)
        {
            _parser = parser;
        }

        public async Task<ICollection<TextyOrgUaWaterRow>> GetData()
        {
            var url = "http://texty.org.ua/water/data/lastDayMeanValueAllKey2.csv";
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = WebRequestMethods.Http.Get;
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            Stream httpResponseStream = httpResponse.GetResponseStream();
            var data = await _parser.ParseAsync(httpResponseStream);
            return data;
        }
    }
}
