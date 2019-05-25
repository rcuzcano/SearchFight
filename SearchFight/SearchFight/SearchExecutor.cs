using SearchEngine.Models;
using SearchFight.Enumerables;
using SearchFight.SearchEngine;
using SearchFight.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchFight
{
    public class SearchExecutor
    {
        private readonly IWebRequest webRequest;
        private readonly string[] keywords;
        public List<Report> reports = new List<Report>();

        public SearchExecutor(IWebRequest _webRequest, string[] _keywords = null) {
            webRequest = _webRequest;
            keywords = _keywords;
        }

        public List<string> ExecuteSearch()
        {
            List<string> results = new List<string>();

            if (!HasKeywords())
            {
                Console.WriteLine("There are no keywords to execute the search");
                return results;
            }

            foreach (var keyword in keywords)
            {
                SearchFightFactory factory = new SearchFightFactory();
                string searchResult = $"{keyword}:";

                foreach (SearchEngineEnum searchEngineEnum in Enum.GetValues(typeof(SearchEngineEnum)))
                {
                    ISearchEngine searchEngine = factory.CreateSearchFight(webRequest, searchEngineEnum);
                    double quantity = searchEngine.GetSearchResults(keyword);

                    searchResult = $"{searchResult} {searchEngineEnum}: {quantity}";

                    reports.Add(new Report() { SearchEngine = searchEngineEnum.ToString(), Keyword = keyword, Quantity = quantity });
                }
                results.Add(searchResult);
            }
            return results;
        }

        public bool HasKeywords() {
            return (this.keywords != null && this.keywords.Length > 0);
        }
        
        public string CalculateWinnerPerSearchEngine(SearchEngineEnum searchEngineEnum)
        {
            var winner = reports.Where(s => s.SearchEngine == searchEngineEnum.ToString()).OrderByDescending(s => s.Quantity).FirstOrDefault();
            return $"{searchEngineEnum} winner: {winner.Keyword}";
        }

        public string CalculateTotalWinner()
        {
            var totalWinner = reports.OrderByDescending(s => s.Quantity).FirstOrDefault();
            return $"Total winner: {totalWinner.Keyword}";
        }

        public List<string> GetWinnerPerSearchEngine() {
            List<string> results = new List<string>();
            foreach (SearchEngineEnum searchEngineEnum in Enum.GetValues(typeof(SearchEngineEnum)))
            {
                results.Add(CalculateWinnerPerSearchEngine(searchEngineEnum));
            }
            return results;
        }

        public void PrintSearchResults() {
            var searchResults = ExecuteSearch();

            if (searchResults.Count > 0)
            {
                foreach (var searchResult in searchResults)
                {
                    Console.WriteLine(searchResult);
                }
                var reportPerSearchEngine = GetWinnerPerSearchEngine();
                foreach (var report in reportPerSearchEngine)
                {
                    Console.WriteLine(report);
                }
                Console.WriteLine(CalculateTotalWinner());
            }
        }
    }
}
