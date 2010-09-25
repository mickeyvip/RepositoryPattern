using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using L2SRepository;
using IL2SRepository;
using DataServices;

namespace Repository.Tests {
    /// <summary>
    /// Summary description for L2SRepositoryTest
    /// </summary>
    [TestClass]
    public class L2SRepositoryTest {

        IUnitOfWorkFactory _l2sFactory;
        L2SNoteworthyService _l2snoteworthyService; 

        public L2SRepositoryTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
       
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() {
            this._l2sFactory = new L2SUnitOfWorkFactory();
            this._l2snoteworthyService = new L2SNoteworthyService(this._l2sFactory);
        }

        #endregion

        [TestMethod]
        public void L2SQueryReturnsArticles() {
            var articles = _l2snoteworthyService.GetArticlesByTopic("TDD").ToArray();
            Assert.AreEqual("TDD test drive number 3", articles[0].Title);
            Assert.AreEqual("Entity Framework and the Repository Pattern", articles[1].Title);
        }

        [TestMethod]
        public void L2SQueryReturnsTopicById() {
            var topic1 = _l2snoteworthyService.GetTopic(1);
            var topic2 = _l2snoteworthyService.GetTopic(2);
            Assert.IsNotNull(topic1);
            Assert.IsNotNull(topic2);
            Assert.AreEqual("Entity Framework", topic1.TopicName);
            Assert.AreEqual("Repository Pattern", topic2.TopicName);
        }

        [TestMethod]
        public void L2SQueryReusesContainer() {
            int count = _l2snoteworthyService.CountTopicsAndArticles();
            Assert.AreEqual(10, count);
        }
    }
}
