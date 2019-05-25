using SearchFight.Helpers;
using System;

namespace SearchFight
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IWebRequest webRequest = new WebRequestImplementation();
                SearchExecutor searchExecutor = new SearchExecutor(webRequest, args);
                searchExecutor.PrintSearchResults();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An unexpected error occured. Please try your request again later or check the following message for more details:\n" + ex.Message);
            }
        }
    }
}
