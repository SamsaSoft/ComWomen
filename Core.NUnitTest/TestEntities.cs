using NUnit.Framework;

namespace Core.NUnitTest
{
    public class TestEntities
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MediaCreateTesting()
        {
            //asign
            var dbContext = ApplicationDbContextTestFactory.Create();
            //act
            var media = new Media();
            dbContext.Add(media);
            dbContext.SaveChanges();
            //assert
            dbContext
        }
    }
}