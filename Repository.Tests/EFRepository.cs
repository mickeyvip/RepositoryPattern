using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IEFRepository;
using DataServices;
using EFRepository;

namespace Repository.Tests {
    /// <summary>
    /// Summary description for EFRepository
    /// </summary>
    [TestClass]
    public class EFRepository {
        IUnitOfWorkFactory _efFactory;
        EFNoteworthyService _efNoteworthyService; 

        public EFRepository() {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
        [TestInitialize()]
        public void MyTestInitialize() {
            this._efFactory = new EFUnitOfWorkFactory();
            this._efNoteworthyService = new EFNoteworthyService(this._efFactory);
        }
        
        #endregion

        [TestMethod]
        public void EFQueryReturnsArticles() {
            var articles = _efNoteworthyService.GetArticlesByTopic("TDD").ToArray();
            Assert.AreEqual("TDD test drive number 3", articles[0].Title);
            Assert.AreEqual("Entity Framework and the Repository Pattern", articles[1].Title);
        }
    }
}
