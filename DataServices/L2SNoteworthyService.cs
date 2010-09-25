using System.Collections.Generic;
using System.Linq;
using IL2SRepository;
using NoteworthyL2SModel;

namespace DataServices {
    public class L2SNoteworthyService {
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public L2SNoteworthyService(IUnitOfWorkFactory unitOfWorkFactory) {
            this._unitOfWorkFactory = unitOfWorkFactory;
        }

        public List<Article> GetArticlesByTopic(string topicName) {
            using (var unitOfWork = _unitOfWorkFactory.Begin()) {
                return unitOfWork.UsingContainer<NoteworthyL2SEntities>()
                    .GetRepository(container => container.Articles)
                    .GetSatisfying(article => article.ArticlesTopics.Any(articleTopic => articleTopic.Topic.TopicName == topicName))
                    .ToList();
            }
        }

        public Topic GetTopic(int topicId) {
            using (var unitOfWork = _unitOfWorkFactory.Begin()) {
                return unitOfWork.UsingContainer<NoteworthyL2SEntities>()
                    .GetRepository(container => container.Topics)
                    .GetSatisfying(topic => topic.Id == topicId)
                    .SingleOrDefault();
            }
        }

        public int CountTopicsAndArticles() {
            int i1;
            int i2;
            using (var unitOfWork = _unitOfWorkFactory.Begin()) {
                i1 = unitOfWork.UsingContainer<NoteworthyL2SEntities>()
                    .GetRepository(container => container.Topics)
                    .GetSatisfying(topic => true)
                    .Count();

                i2 = unitOfWork.UsingContainer<NoteworthyL2SEntities>()
                    .GetRepository(container => container.Articles)
                    .GetSatisfying(article => true)
                    .Count();
            }

            return i1 + i2;
        }
    }
}
