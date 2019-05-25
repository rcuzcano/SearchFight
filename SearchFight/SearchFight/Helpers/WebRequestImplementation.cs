using System.IO;
using System.Net;

namespace SearchFight.Helpers
{
    public class WebRequestImplementation: IWebRequest
    {
        public string RequestResponse(string url) {
            var result = "";

            WebRequest request = WebRequest.Create(url);
            var response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                result = reader.ReadToEnd();
            }
            return result;
        }

    }
}
