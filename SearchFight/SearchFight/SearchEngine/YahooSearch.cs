using HtmlAgilityPack;
using SearchFight.Helpers;
using System;

namespace SearchFight.SearchEngine
{
    public class YahooSearch : ISearchEngine
    {
        IWebRequest webRequest;
        public YahooSearch(IWebRequest _webRequest) {
            webRequest = _webRequest;
        }

        private string GetBaseURL() {
            return "https://search.yahoo.com/search?p=";
        }

        public double GetSearchResults(string keyword) {
            try
            {
                string url = $"{this.GetBaseURL()}{keyword}";
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(webRequest.RequestResponse(url));

                var div = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'compPagination')]");
                if (div != null) {
                    var strings = div[div.Count - 1].LastChild.InnerText.Split(' ');

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
