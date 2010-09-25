using DataServices;
using IL2SRepository;
using MemoryL2SRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteworthyL2SModel;

namespace Repository.Tests {
    /// <summary>
    /// Summary description for L2SMemoryTest
    /// </summary>
    [TestClass]
    public class L2SMemoryTest {
        IUnitOfWorkFactory _memoryFactory;

        public L2SMemoryTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        private void InitArticleMemoryRepository() {
            IRepository<Article> memoryArticlesRepository = _memoryFactory
               .Begin()
               .UsingContainer<NoteworthyL2SEntities>()
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

            efRepository.ArticlesTopics.Add(new ArticlesTopic { Topic = ddd });

            Article correspondenceLaunch = new Article() {
                Title = "Correspondence Launch"
            };
            correspondenceLaunch.ArticlesTopics.Add(new ArticlesTopic { Topic = corresopndence });
            Article correspondenceDDD = new Article() {
                Title = "Correspondence and DDD"
            };
            correspondenceDDD.ArticlesTopics.Add(new ArticlesTopic { Topic = ddd });
            correspondenceDDD.ArticlesTopics.Add(new ArticlesTopic { Topic = corresopndence });

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
            L2SNoteworthyService noteworthyService = new L2SNoteworthyService(_memoryFactory);
            var articles = noteworthyService.GetArticlesByTopic("ddd").ToArray();
            Assert.AreEqual("Entity Framework and the Repository Pattern", articles[0].Title);
            Assert.AreEqual("Correspondence and DDD", articles[1].Title);
        }
    }
}
