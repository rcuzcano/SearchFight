using SearchFight.Enumerables;
using SearchFight.Helpers;

namespace SearchFight.SearchEngine
{
    public class SearchFightFactory
    {
        public virtual ISearchEngine CreateSearchFight(IWebRequest webRequest, SearchEngineEnum SearchFight)
        {
            ISearchEngine engine = null;

            switch (SearchFight)
            {
                case SearchEngineEnum.Google:
                    engine = new GoogleSearch(webRequest);
                    break;
                case SearchEngineEnum.Bing:
                    engine = new BingSearch(webRequest);
                    break;
                case SearchEngineEnum.Yahoo:
                    engine = new YahooSearch(webRequest);
                    break;
            }

            return engine;
        }
    }
}
