using System.Collections.Generic;
using System.Linq;
using IEFRepository;
using NoteworthyEFModel;

namespace DataServices {
    public class EFNoteworthyService {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        
        public EFNoteworthyService(IUnitOfWorkFactory unitOfWorkFactory) {
            this._unitOfWorkFactory = unitOfWorkFactory;
        }

        public List<Article> GetArticlesByTopic(string topicName) {
            using (var unitOfWork = _unitOfWorkFactory.Begin()) {
                return unitOfWork.UsingContainer<NoteworthyEFEntities>()
                    .GetRepository(container => container.Articles)
                    .GetSatisfying(article => article.Topics.Any(topic => topic.TopicName == topicName))
                    .ToList();
            }
        }
    }
}
