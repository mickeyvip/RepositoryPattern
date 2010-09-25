using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataServices;
using IEFRepository;
using NoteworthyEFModel;
using MemoryEFRepository;

namespace Repository.Tests {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class EFMemoryTest {

        IUnitOfWorkFactory _memoryFactory;

        public EFMemoryTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        private void InitArticleMemoryRepository() {
            IRepository<Article> memoryArticlesRepository = _memoryFactory
               .Begin()
               .UsingContainer<NoteworthyEFEntities>()
               .GetRepository(container => container.Articles);

            Topic ddd = new Topic() {
                TopicName = "ddd"
            };

            Topic corresopndence = new Topic() {
                TopicName = "correspondence"
            };
            Article efRepository = new Article() {
                Title = "Entity Framework and the Repository Pattern"
            };

            efRepository.Topics.Add(ddd);

            Article correspondenceLaunch = new Article() {
                Title = "Correspondence Launch"
            };
            correspondenceLaunch.Topics.Add(corresopndence);
            Article correspondenceDDD = new Article() {
                Title = "Correspondence and DDD"
            };
            correspondenceDDD.Topics.Add(ddd);
            correspondenceDDD.Topics.Add(corresopndence);

            memoryArticlesRepository.Add(efRepository);
            memoryArticlesRepository.Add(correspondenceLaunch);
            memoryArticlesRepository.Add(correspondenceDDD);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() {
            this._memoryFactory = new MemoryUnitOfWorkFactory();
            this.InitArticleMemoryRepository();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        [TestMethod]
        public void MemoryQueryReturnsArticles() {
            EFNoteworthyService noteworthyService = new EFNoteworthyService(_memoryFactory);
            var articles = noteworthyService.GetArticlesByTopic("ddd").ToArray();
            Assert.AreEqual("Entity Framework and the Repository Pattern", articles[0].Title);
            Assert.AreEqual("Correspondence and DDD", articles[1].Title);
        }
    }
}
