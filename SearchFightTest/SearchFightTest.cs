using SearchFight;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchEngine.Models;
using SearchFight.Enumerables;
using FluentAssertions;
using Moq;
using SearchFight.Helpers;
using System.Collections.Generic;

namespace SearchFightTest
{
    public class Results {
        public string Result { get; set; }
    }

    [TestClass]
    public class SearchFightTest
    {
        [TestMethod]
        public void GetSearchResults_ShouldNotReturnResultsFromAnySearchEngine()
        {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            string[] keywords = new string[0];
            SearchExecutor searchExecutor = new SearchExecutor(webRequest, keywords);
            var emptyList = new List<string>();

            //Act
            var result = searchExecutor.ExecuteSearch();

            //Assert
            result.Should().Equal(emptyList);
        }

        [TestMethod]
        public void GetSearchResults_ShouldReturnResultsFromGoogleForJava() {
            //Arrange
            string[] keywords = new string[1] { "java" };

            var mockedWebRequest = new Mock<IWebRequest>();
            mockedWebRequest
            .Setup(c => c.RequestResponse("https://www.google.com/search?q=java"))
            .Returns("<html><body><div id='resultStats'>Cerca de 670,000,000 resultados</div></body></html>");

            SearchExecutor searchExecutor = new SearchExecutor(mockedWebRequest.Object, keywords);

            //Act
            var result = searchExecutor.ExecuteSearch();

            //Assert
            result.Should().HaveElementAt(0, "java: Google: 670000000 Bing: 0 Yahoo: 0");
        }

        [TestMethod]
        public void GetSearchResults_ShouldReturnResultsFromBingForJava() {
            //Arrange
            string[] keywords = new string[1] { "java" };

            var mockedWebRequest = new Mock<IWebRequest>();
            mockedWebRequest
            .Setup(c => c.RequestResponse("https://www.bing.com/search?q=java"))
            .Returns("<html><body><div id='b_tween'><span class='sb_count' data-bm='4'>23.400.000 results</span></div></body></html>");

            SearchExecutor searchExecutor = new SearchExecutor(mockedWebRequest.Object, keywords);

            //Act
            var result = searchExecutor.ExecuteSearch();

            //Assert
            result.Should().HaveElementAt(0, "java: Google: 0 Bing: 23400000 Yahoo: 0");
        }

        [TestMethod]
        public void GetSearchResults_ShouldReturnResultsFromYahooForJava()
        {
            //Arrange
            string[] keywords = new string[1] { "java" };

            var mockedWebRequest = new Mock<IWebRequest>();
            mockedWebRequest
            .Setup(c => c.RequestResponse("https://search.yahoo.com/search?p=java"))
            .Returns("<html><body><div class='compPagination'><span>670,000,000 resultados</span></div></body></html>");

            SearchExecutor searchExecutor = new SearchExecutor(mockedWebRequest.Object, keywords);

            //Act
            var result = searchExecutor.ExecuteSearch();

            //Assert
            result.Should().HaveElementAt(0, "java: Google: 0 Bing: 0 Yahoo: 670000000");
        }

        [TestMethod]
        public void HasKeywords_ShouldReturnFalse() {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            SearchExecutor searchExecutor = new SearchExecutor(webRequest);

            //Act
            var result = searchExecutor.HasKeywords();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void HasKeywords_ShouldReturnTrue()
        {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            string[] keywords = new string[2] { ".net", "java" };
            SearchExecutor searchExecutor = new SearchExecutor(webRequest, keywords);

            //Act
            var result = searchExecutor.HasKeywords();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CalculateWinnerPerSearchEngine_ShouldReturnDotNetAsWinnerForGoogle()
        {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            SearchExecutor searchExecutor = new SearchExecutor(webRequest);
            Report googleReport1 = new Report() { SearchEngine = SearchEngineEnum.Google.ToString(), Keyword = ".net", Quantity = 999 };
            Report googleReport2 = new Report() { SearchEngine = SearchEngineEnum.Google.ToString(), Keyword = "java", Quantity = 758 };

            //Act
            searchExecutor.reports.Add(googleReport1);
            searchExecutor.reports.Add(googleReport2);
            var result = searchExecutor.CalculateWinnerPerSearchEngine(SearchEngineEnum.Google);

            //Assert
            result.Should().Be("Google winner: .net");
        }

        [TestMethod]
        public void CalculateWinnerPerSearchEngine_ShouldReturnDotNetAsWinnerForBing()
        {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            SearchExecutor searchExecutor = new SearchExecutor(webRequest);
            Report bingReport1 = new Report() { SearchEngine = SearchEngineEnum.Bing.ToString(), Keyword = ".net", Quantity = 120 };
            Report bingReport2 = new Report() { SearchEngine = SearchEngineEnum.Bing.ToString(), Keyword = "java", Quantity = 95 };

            //Act
            searchExecutor.reports.Add(bingReport1);
            searchExecutor.reports.Add(bingReport2);
            var result = searchExecutor.CalculateWinnerPerSearchEngine(SearchEngineEnum.Bing);

            //Assert
            result.Should().Be("Bing winner: .net");
        }

        [TestMethod]
        public void CalculateWinnerPerSearchEngine_ShouldReturnDotNetAsWinnerForYahoo()
        {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            SearchExecutor searchExecutor = new SearchExecutor(webRequest);
            Report yahooReport1 = new Report() { SearchEngine = SearchEngineEnum.Yahoo.ToString(), Keyword = ".net", Quantity = 65 };
            Report yahooReport2 = new Report() { SearchEngine = SearchEngineEnum.Yahoo.ToString(), Keyword = "java", Quantity = 50 };

            //Act
            searchExecutor.reports.Add(yahooReport1);
            searchExecutor.reports.Add(yahooReport2);
            var result = searchExecutor.CalculateWinnerPerSearchEngine(SearchEngineEnum.Yahoo);

            //Assert
            result.Should().Be("Yahoo winner: .net");
        }


        [TestMethod]
        public void CalculateTotalWinner_ShouldReturnDotNetAsWinner()
        {
            //Arrange
            IWebRequest webRequest = new WebRequestImplementation();
            SearchExecutor searchExecutor = new SearchExecutor(webRequest);
            Report googleReport1 = new Report() { SearchEngine = SearchEngineEnum.Google.ToString(), Keyword = ".net", Quantity = 999 };
            Report googleReport2 = new Report() { SearchEngine = SearchEngineEnum.Google.ToString(), Keyword = ".java", Quantity = 758 };

            Report bingReport1 = new Report() { SearchEngine = SearchEngineEnum.Bing.ToString(), Keyword = ".net", Quantity = 120 };
            Report bingReport2 = new Report() { SearchEngine = SearchEngineEnum.Bing.ToString(), Keyword = ".java", Quantity = 95 };

            //Act
            searchExecutor.reports.Add(googleReport1);
            searchExecutor.reports.Add(googleReport2);
            searchExecutor.reports.Add(bingReport1);
            searchExecutor.reports.Add(bingReport2);
            var result = searchExecutor.CalculateTotalWinner();

            //Assert
            result.Should().Be("Total winner: .net");
        }
    }
}
