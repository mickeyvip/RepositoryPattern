using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using IL2SRepository;

namespace L2SRepository {
    public class L2SUnitOfWorkFactory : IUnitOfWorkFactory {
        public IUnitOfWork Begin() {
            return new L2SUnitOfWork();
        }
    }

    class L2SUnitOfWork : IUnitOfWork {
        private Dictionary<Type, IDisposable> _containerByType = new Dictionary<Type, IDisposable>();

        public IContainer<TContainer> UsingContainer<TContainer>() where TContainer : DataContext, new() {
            IDisposable container;
            if (!_containerByType.TryGetValue(typeof(TContainer), out container)) {
                container = new L2SContainer<TContainer>();
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

    public class L2SContainer<TContainer> : IContainer<TContainer> where TContainer : DataContext, new() {
        private TContainer _container;

        public L2SContainer() {
            _container = new TContainer();
        }

        public void Dispose() {
            _container.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>(Func<TContainer, Table<TEntity>> repositorySelector) where TEntity : class {
            return new L2SRepository<TContainer, TEntity>(_container, repositorySelector(_container));
        }
    }

    public class L2SRepository<TContainer, TEntity> : IRepository<TEntity>
        where TContainer : DataContext, new()
        where TEntity : class {

        private TContainer _container;
        private Table<TEntity> _objectQuery;

        public L2SRepository(TContainer container, Table<TEntity> objectQuery) {
            this._container = container;
            this._objectQuery = objectQuery;
        }

        public IQueryable<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> specification) {
            return _objectQuery.Where(specification);
        }

        public void Add(TEntity entity) {
            _objectQuery.InsertOnSubmit(entity);
        }
    }
}
