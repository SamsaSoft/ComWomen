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
                MediaType = Enums.MediaType.Photo,
                Translations = new List<MediaTranslation>
                {
                    new MediaTranslation
                    {
                        Description = "Description",
                        Title = "Title",
                        Url = "https://images.com/image?id=1",
                        Language = Enums.Language.ky,
                    },
                    new MediaTranslation
                    {
                        Description = "Описание",
                        Title = "Название",
                        Url = "https://images.com/image?id=1&lg=ru",
                        Language = Enums.Language.ru,

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
            CollectionAssert.IsNotEmpty(media?.Translations);
            CollectionAssert.IsSubsetOf(
                new[] { Enums.Language.ky, Enums.Language.ru },
                media?.Translations.Select(x=>x.Language));
        }

        private Media CreateMedia() 
        {
            return new Media
            {
                MediaType = Enums.MediaType.Photo,
                Translations = new List<MediaTranslation>
                {
                    new MediaTranslation
                    {
                        Description = "Description",
                        Title = "Title",
                        Url = "https://images.com/image?id=1",
                        Language = Enums.Language.ky,
                    },
                    new MediaTranslation
                    {
                        Description = "Описание",
                        Title = "Название",
                        Url = "https://images.com/image?id=1&lg=ru",
                        Language = Enums.Language.ru,

                    },
                },
            }; 
        }

        [Test]
        public void MediaDeleteWithMediaTranslationTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = CreateMedia();
            dbContext.Add(media);
            dbContext.SaveChanges();
            var medias = dbContext.Set<Media>();
            medias.Remove(media);
            dbContext.SaveChanges();
            //assert
            CollectionAssert.IsEmpty(medias);
            var mediaTranslations = dbContext.Set<MediaTranslation>();
            CollectionAssert.IsEmpty(mediaTranslations);
        }

        [Test]
        public void MediaSimpleEditTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = new Media();
            dbContext.Add(media);
            dbContext.SaveChanges();
            var medias = dbContext.Set<Media>();
            media = medias.Find(1);
            Assert.IsNotNull(media);
            var now = System.DateTime.Now;
            media.EditedAt = now;
            dbContext.SaveChanges();
            //assert
            Assert.AreEqual(now, media?.EditedAt);
        }

        [Test]
        public void MediaEditWithMediaTranslationTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            var medias = dbContext.Set<Media>();
            //act
            var media = CreateMedia();
            dbContext.Add(media);
            dbContext.SaveChanges();
            media = medias.Find(1);
            Assert.IsNotNull(media);
            Assert.IsNotNull(media?.Translations);
            CollectionAssert.IsNotEmpty(media?.Translations);
            var mediaTranslation = media.Translations.FirstOrDefault(e=>e.Language == Enums.Language.ru);
            Assert.IsNotNull(mediaTranslation);
            mediaTranslation.Description = "Описание 2";
            dbContext.SaveChanges();
            //assert
            media = medias.Find(1);
            Assert.IsNotNull(media);
            Assert.IsNotNull(media?.Translations);
            CollectionAssert.IsNotEmpty(media?.Translations);
            mediaTranslation = media.Translations.FirstOrDefault(e => e.Language == Enums.Language.ru);
            Assert.IsNotNull(mediaTranslation);
            Assert.AreEqual("Описание 2", mediaTranslation?.Description);
        }
    }
}