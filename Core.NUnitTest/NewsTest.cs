using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Core.NUnitTest
{
    public class NewsTest
    {
        [Test]
        public void IncludeFiltredTest()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            var news = dbContext.Set<News>();
            var langs = dbContext.Set<NewsTranslation>();
            var newsItem = new News();
            newsItem.Translations = new List<NewsTranslation> {
                new NewsTranslation { Title ="1", Content ="content1", Language = Enums.Language.ru },
                new NewsTranslation { Title ="2", Content ="content2", Language = Enums.Language.ky },
            };
            news.Add(newsItem);
            dbContext.SaveChanges();
            var newsService = new NewsService(dbContext, null);
            var newsByLanguage = newsService.GetAllByLanguage(Enums.Language.ru);
            Assert.AreEqual(2, langs.Count());
        }

        [Test]
        public void IncludeNoFiltredTest()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            var news = dbContext.Set<News>();
            var langs = dbContext.Set<NewsTranslation>();
            var newsItem = new News();
            newsItem.Translations = new List<NewsTranslation> {
                new NewsTranslation { Title ="1", Content ="content1", Language = Enums.Language.ru },
                new NewsTranslation { Title ="2", Content ="content2", Language = Enums.Language.ky },
            };
            news.Add(newsItem);
            dbContext.SaveChanges();
            var newsByLanguage = news.Include(x => x.Translations).ToList();
            Assert.AreEqual(2, langs.Count());
        }

        [Test]
        public void IncludeFiltredLanguageTest()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            var news = dbContext.Set<News>();
            var langs = dbContext.Set<NewsTranslation>();
            var newsItem = new News();
            newsItem.Translations = new List<NewsTranslation> {
                new NewsTranslation { Title ="1", Content ="content1", Language = Enums.Language.ru },
                new NewsTranslation { Title ="2", Content ="content2", Language = Enums.Language.ky },
            };
            news.Add(newsItem);
            dbContext.SaveChanges();
            var newsByLanguage = langs
                .Where(x=>x.NewsId == 1 && x.Language == Enums.Language.ru)
                .Select(x=>x.News).ToList();
            Assert.AreEqual(2, langs.Count());
        }
    }
}
