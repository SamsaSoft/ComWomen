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
                        LanguageId = Enums.LanguageEnum.ky,
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
                new[] { Enums.LanguageEnum.ky, Enums.LanguageEnum.ru },
                media?.MediaTranslations.Select(x=>x.LanguageId));
        }

        private Media CreateMedia() 
        {
            return new Media
            {
                MediaTypeId = Enums.MediaTypeEnum.Photo,
                MediaTranslations = new List<MediaTranslation>
                {
                    new MediaTranslation
                    {
                        Description = "Description",
                        Title = "Title",
                        Url = "https://images.com/image?id=1",
                        LanguageId = Enums.LanguageEnum.ky,
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
            Assert.IsNotNull(media?.MediaTranslations);
            CollectionAssert.IsNotEmpty(media?.MediaTranslations);
            var mediaTranslation = media.MediaTranslations.FirstOrDefault(e=>e.LanguageId == Enums.LanguageEnum.ru);
            Assert.IsNotNull(mediaTranslation);
            mediaTranslation.Description = "Описание 2";
            dbContext.SaveChanges();
            //assert
            media = medias.Find(1);
            Assert.IsNotNull(media);
            Assert.IsNotNull(media?.MediaTranslations);
            CollectionAssert.IsNotEmpty(media?.MediaTranslations);
            mediaTranslation = media.MediaTranslations.FirstOrDefault(e => e.LanguageId == Enums.LanguageEnum.ru);
            Assert.IsNotNull(mediaTranslation);
            Assert.AreEqual("Описание 2", mediaTranslation?.Description);
        }
    }
}