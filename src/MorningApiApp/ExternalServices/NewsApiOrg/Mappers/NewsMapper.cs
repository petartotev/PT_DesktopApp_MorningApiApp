using System.Linq;
using MorningApiApp.ExternalServices.NewsApiOrg.Models;
using MorningApiApp.ExternalServices.NewsApiOrg.OutputModels;

namespace MorningApiApp.ExternalServices.NewsApiOrg.Mappers
{
    public static class NewsMapper
    {
        public static MyRootNews ToMyRootNewsModel(this RootNews model)
        {
            return new MyRootNews
            {
                Status = model.Status,
                TotalResults = model.TotalResults,
                MyArticles = model.Articles.Select(x => x.ToMyNewsArticleModel()).ToList()
            };
        }

        public static MyNewsArticle ToMyNewsArticleModel(this Article model)
        {
            return new MyNewsArticle
            {
                MySource = model.Source.ToMyNewsSource(),
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                Author = model.Author,
                PublishedAt = model.PublishedAt,
                Url = model.Url,
                UrlToImage = model.UrlToImage
            };
        }

        public static MyNewsSource ToMyNewsSource(this Source model)
        {
            return new MyNewsSource
            {
                Id = model.Id,
                Name = model.Id
            };
        }
    }
}
