using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Core.Services;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.NUnitTest
{
    public class TestServices
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task MediaSimpleCreateTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            IMediaService service = new MediaService(dbContext, ???);
            //act
            var media = new Media();
            await service.Upload(media);
            //assert
            media = await service.GetById(1);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media?.Id);
        }

        [Test]
        public async Task MediaSimpleDeleteTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            IMediaService service = new MediaService(dbContext);
            //act
            var media = new Media();
            await service.Upload(media);
            await service.DelteById(1);
            //assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.GetById(1));
        }

        [Test]
        public async Task MediaGetAllWithTypeTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            IMediaService service = new MediaService(dbContext);
            //act
            var media = new Media
            {
                MediaType = Enums.MediaType.Photo,
            };
            await service.Upload(media);
            media = new Media
            {
                MediaType = Enums.MediaType.Photo,
            };
            await service.Upload(media);
            media = new Media
            {
                MediaType = Enums.MediaType.Video,
            };
            await service.Upload(media);
            //assert
            var medias = await service.GetAllWithType(Enums.MediaType.Photo);
            Assert.IsNotNull(medias);
            Assert.AreEqual(2, medias.Count);
        }
    }
}