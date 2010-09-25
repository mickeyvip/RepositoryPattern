using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEFRepository;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;

namespace EFRepository {
    public class EFUnitOfWorkFactory : IUnitOfWorkFactory {
        public IUnitOfWork Begin() {
            return new EFUnitOfWork();
        }
    }

    class EFUnitOfWork : IUnitOfWork {
        private Dictionary<Type, IDisposable> _containerByType = new Dictionary<Type, IDisposable>();

        public IContainer<TContainer> UsingContainer<TContainer>() where TContainer : ObjectContext, new() {
            IDisposable container;
            if (!_containerByType.TryGetValue(typeof(TContainer), out container)) {
                container = new EFContainer<TContainer>();
                this._containerByType.Add(typeof(TContainer), container);
            }
            return (IContainer<TContainer>)container;

        }

        public void Dispose() {
            foreach (var container in this._containerByType.Values) {
                container.Dispose();
            }
        }
    }

    public class EFContainer<TContainer> : IContainer<TContainer> where TContainer : ObjectContext, new() {
        private TContainer _container;

        public EFContainer() {
            _container = new TContainer();
        }

        public IRepository<TEntity> GetRepository<TEntity>(Func<TContainer, ObjectQuery<TEntity>> repositorySelector) where TEntity : EntityObject {
            return new EFRepository<TContainer, TEntity>(_container, repositorySelector(_container));
        }

        public void Dispose() {
            _container.Dispose();
        }
    }

    public class EFRepository<TContainer, TEntity> : IRepository<TEntity>
        where TContainer : ObjectContext, new()
        where TEntity : EntityObject {

        private TContainer _container;
        private ObjectQuery<TEntity> _objectQuery;

        public EFRepository(TContainer container, ObjectQuery<TEntity> objectQuery) {
            this._container = container;
            this._objectQuery = objectQuery;
        }


        public IQueryable<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> specification) {
            return _objectQuery.Where(specification);
        }

        public void Add(TEntity entity) {
            _container.AddObject(_objectQuery.Name, entity);
        }
    }
}
