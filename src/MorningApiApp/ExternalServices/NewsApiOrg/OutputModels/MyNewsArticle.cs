using System;

namespace MorningApiApp.ExternalServices.NewsApiOrg.OutputModels
{
    public class MyNewsArticle
    {
        public MyNewsSource MySource { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public string UrlToImage { get; set; }

        public DateTime PublishedAt { get; set; }

        public override string ToString()
        {
            return
                $"{PublishedAt:yyyy-MM-dd}\n" +
                $"{Title}\n" +
                $"{Description}\n" +
                $"by {Author}\n" +
                $"source: {MySource.Name}\n" +
                $"{Content}" +
                $"{Url}";
        }
    }
}
