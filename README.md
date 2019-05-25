# SearchFight


SeachFight is a console appication that determines the popularity of any keyword or keywords that the user wants to search using Google, Bing and Yahoo search engines. For it's execution, see the lines described below:

    C:\> SearchFight.exe .net java
    .net: Google: 4450000000 Bing: 12354420 Yahoo: 3453400
    java: Google: 966000000 Bing: 94381485 Yahoo: 9254000
    Google winner: .net
    Bing winner: java
	Yahoo winner: java
    Total winner: .net

Currently the application supports 3 search engines (Google, Bing and Yahoo), in case the user wants to add an additional search engine, it will just need to add an additional SearcEngine class and modify it's corresponding Factory. It also requires to implementing the methods described into the ISearchEngine interface and adding a new SearchEngineEnum value. The application will automatically recognize that new seach engine.

The solution also includes a SearchFightTest project that includes Unit Testing for it's corresponding application code coverage. It's currently using Moq and FluentAssertions for having a better code readability and maintainability.