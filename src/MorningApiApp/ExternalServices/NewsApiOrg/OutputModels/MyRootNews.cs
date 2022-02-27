using System.Collections.Generic;

namespace MorningApiApp.ExternalServices.NewsApiOrg.OutputModels
{
    public class MyRootNews
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<MyNewsArticle> MyArticles { get; set; }
    }
}
