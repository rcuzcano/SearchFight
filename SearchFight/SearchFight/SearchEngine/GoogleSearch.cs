using HtmlAgilityPack;
using SearchFight.Helpers;
using System;

namespace SearchFight.SearchEngine
{
    public class GoogleSearch : ISearchEngine
    {
        IWebRequest webRequest;
        public GoogleSearch(IWebRequest _webRequest) {
            webRequest = _webRequest;
        }

        private string GetBaseURL() {
            return "https://www.google.com/search?q=";
        }

        public double GetSearchResults(string keyword) {
            try
            {
                string url = $"{this.GetBaseURL()}{keyword}";
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(webRequest.RequestResponse(url));

                var div = htmlDocument.GetElementbyId("resultStats");
                if (div != null) {
                    var strings = div.InnerText.Split(' ');

                    foreach (var s in strings)
                    {
                        double result = 0;
                        if (double.TryParse(s, out result))
                        {
                            return double.Parse(s);
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

    }
}
