using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using IEFRepository;

namespace MemoryEFRepository {
    public class MemoryUnitOfWorkFactory : IUnitOfWorkFactory {
        private MemoryUnitOfWork _unitOfWork = new MemoryUnitOfWork();

        public IUnitOfWork Begin() {
            return this._unitOfWork;
        }
    }

    public class MemoryUnitOfWork : IUnitOfWork {
        private Dictionary<Type, object> _containerByType = new Dictionary<Type, object>();

        public IContainer<TContainer> UsingContainer<TContainer>() where TContainer : ObjectContext, new() {
            object container;

            if (!_containerByType.TryGetValue(typeof(TContainer), out container)) {
                container = new MemoryContainer<TContainer>();
                this._containerByType.Add(typeof(TContainer), container);
            }
            return (IContainer<TContainer>)container;
        }

        public void Dispose() {
        }
    }

    public class MemoryContainer<TContainer> : IContainer<TContainer> where TContainer : ObjectContext, new() {
        private Dictionary<Type, object> _containerByType = new Dictionary<Type, object>();

        public IRepository<TEntity> GetRepository<TEntity>(Func<TContainer, ObjectQuery<TEntity>> repositorySelector) where TEntity : EntityObject {
            object container;
            if (!_containerByType.TryGetValue(typeof(TEntity), out container)) {
                container = new MemoryRepository<TEntity>();
                this._containerByType.Add(typeof(TEntity), container);
            }

            return (IRepository<TEntity>)container;
        }

        public void Dispose() {
        }
    }

    public class MemoryRepository<TEntity> : IRepository<TEntity> {
        private List<TEntity> _entities = new List<TEntity>();

        public IQueryable<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> specification) {
            return _entities.Where(specification.Compile()).AsQueryable();
        }

        public void Add(TEntity entity) {
            _entities.Add(entity);
        }
    }
}
