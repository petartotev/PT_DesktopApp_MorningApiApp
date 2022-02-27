using System.Collections.Generic;

namespace MorningApiApp.ExternalServices.NewsApiOrg.Models
{
    public class RootNews
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<Article> Articles { get; set; }
    }
}
