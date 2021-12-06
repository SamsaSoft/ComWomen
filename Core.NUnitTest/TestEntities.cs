using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace Core.NUnitTest
{
    public class TestEntities
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MediaSimpleCreateTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = new Media();
            dbContext.Add(media);
            dbContext.SaveChanges();
            var medias = dbContext.Set<Media>();
            //assert
            CollectionAssert.IsNotEmpty(medias);
            media = medias.Find(1);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media?.Id);
        }

        [Test]
        public void MediaSimpleDeleteTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = new Media();
            dbContext.Add(media);
            dbContext.SaveChanges();
            var medias = dbContext.Set<Media>();
            medias.Remove(media);
            dbContext.SaveChanges();
            //assert
            CollectionAssert.IsEmpty(medias);
        }

        [Test]
        public void MediaCreateWithMediaTranslationTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = new Media
            {
                MediaTypeId = Enums.MediaTypeEnum.Photo,
                MediaTranslations = new List<MediaTranslation>
                {
                    new MediaTranslation
                    {
                        Description = "Description",
                        Title = "Title",
                        Url = "https://images.com/image?id=1",
                        LanguageId = Enums.LanguageEnum.en,
                    },
                    new MediaTranslation
                    {
                        Description = "Описание",
                        Title = "Название",
                        Url = "https://images.com/image?id=1&lg=ru",
                        LanguageId = Enums.LanguageEnum.ru,

                    },
                },
            };
            dbContext.Add(media);
            dbContext.SaveChanges();
            var medias = dbContext.Set<Media>();
            //assert
            CollectionAssert.IsNotEmpty(medias);
            media = medias.Find(1);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media?.Id);
            CollectionAssert.IsNotEmpty(media?.MediaTranslations);
            CollectionAssert.IsSubsetOf(
                new[] { Enums.LanguageEnum.en, Enums.LanguageEnum.ru },
                media?.MediaTranslations.Select(x=>x.LanguageId));
        }

        [Test]
        public void MediaDeleteWithMediaTranslationTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = new Media
            {
                MediaTypeId = Enums.MediaTypeEnum.Photo,
                MediaTranslations = new List<MediaTranslation>
                {
                    new MediaTranslation
                    {
                        Description = "Description",
                        Title = "Title",
                        Url = "https://images.com/image?id=1",
                        LanguageId = Enums.LanguageEnum.en,
                    },
                    new MediaTranslation
                    {
                        Description = "Описание",
                        Title = "Название",
                        Url = "https://images.com/image?id=1&lg=ru",
                        LanguageId = Enums.LanguageEnum.ru,

                    },
                },
            };
            dbContext.Add(media);
            dbContext.SaveChanges();
            var medias = dbContext.Set<Media>();
            medias.Remove(media); //TODO configure cascading deletion
            dbContext.SaveChanges();
            //assert
            CollectionAssert.IsEmpty(medias);
            var mediaTranslations = dbContext.Set<MediaTranslation>();
            CollectionAssert.IsEmpty(mediaTranslations);
        }

        //TODO add edit tests
    }
}