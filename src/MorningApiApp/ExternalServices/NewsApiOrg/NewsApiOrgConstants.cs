namespace MorningApiApp.ExternalServices.NewsApiOrg
{
    public class NewsApiOrgConstants
    {
        public static string UrlBase = "https://newsapi.org/v2";

        // Class = Everything / Top-Headlines
        public static string QueryEverything = "/everything";
        public static string QueryTopHeadlines = "/top-headlines";

        public static string FromDate = "&from={0}";
        public static string ToDate = "&to={0}";
        public static string SortBy = "&sortBy={0}";
        public static string Keyword = "?q={0}";
        public static string Sources = "?sources={0}";
        public static string SourcesAnd = "&sources={0}";
        public static string Category = "?category={0}";
        public static string CategoryAnd = "&category={0}";
        public static string Country = "?country={0}";
        public static string CountryAnd = "&country={0}";

        // https://newsapi.org/v2/everything?q=apple&from=2022-02-25&to=2022-02-25&sortBy=popularity&apiKey=API_KEY
        // https://newsapi.org/v2/everything?q=tesla&from=2022-01-26&sortBy=publishedAt&apiKey=API_KEY
        // https://newsapi.org/v2/top-headlines?country=us&category=business&apiKey=API_KEY
        // https://newsapi.org/v2/top-headlines?sources=techcrunch&apiKey=API_KEY
        // https://newsapi.org/v2/everything?domains=wsj.com&apiKey=API_KEY

        // everything?q=apple&from=2022-02-25&to=2022-02-25&sortBy=popularity
        // everything?q=tesla&from=2022-01-26&sortBy=publishedAt
        // everything?domains=wsj.com
        // top-headlines?country=us&category=business
        // top-headlines?sources=techcrunch

        // "message": "You cannot mix the sources parameter with the country or category parameters."
    }
}
