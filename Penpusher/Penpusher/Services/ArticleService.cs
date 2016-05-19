using System.Collections.Generic;
using System.Linq;
using Penpusher.Services.Base;

namespace Penpusher.Services
{
    public class ArticleService : ServiceBase<Article>, IArticleService
    {
        private readonly IRepository<Article> _repository;

        public ArticleService(IRepository<Article> repository)
        {
            _repository = repository;
        }

        public Article AddArticle(Article article)
        {
            return _repository.Add(article);
        }

        public  Article GetById(int id) {
            return _repository.GetAll().FirstOrDefault(x => x.Id == id);
        } 

        public bool CheckDoesExists(string title)
        {
            return _repository.GetAll().Count(x => x.Title == title) > 0;
        }

        public override IEnumerable<Article> Find(string title)
        {
            return _repository.GetAll().Where(x => x.Title == title);
        }

        public IEnumerable<Article> GetArticlesFromProvider(int newsProviderId)
        {
            return _repository.GetAll().Where(x=> x.IdNewsProvider == newsProviderId).ToList();
        }

        public IEnumerable<Article> GetAllArticleses()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Article> GetArticlesFromSelectedProviders(IEnumerable<NewsProvider> newsProviders)
        {
            var articles = new List<Article>();

            if (newsProviders.ToList().Count > 0)
            {
                foreach (NewsProvider provider in newsProviders)
                {
                    var nextProviderArticles = GetArticlesFromProvider(provider.Id).ToList();
                    articles.AddRange(nextProviderArticles);
                }
            }
            return articles;
        }
    }
}
