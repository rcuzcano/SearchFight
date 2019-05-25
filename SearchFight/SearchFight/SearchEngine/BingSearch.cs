using HtmlAgilityPack;
using SearchFight.Helpers;
using System;

namespace SearchFight.SearchEngine
{
    public class BingSearch : ISearchEngine
    {
        IWebRequest webRequest;
        public BingSearch(IWebRequest _webRequest)
        {
            webRequest = _webRequest;
        }
        private string GetBaseURL() {
            return "https://www.bing.com/search?q=";
        }

        public double GetSearchResults(string keyword) {
            try
            {
                string url = $"{this.GetBaseURL()}{keyword}";
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(webRequest.RequestResponse(url));

                var div = htmlDocument.GetElementbyId("b_tween");
                if (div != null) {
                    var strings = div.ChildNodes[0].InnerText.Split(' ');

                    foreach (var s in strings)
                    {
                        var sWithCommas = s.Replace('.', ',');
                        double result = 0;
                        if (double.TryParse(sWithCommas, out result))
                        {
                            return double.Parse(sWithCommas);
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
    }
}
