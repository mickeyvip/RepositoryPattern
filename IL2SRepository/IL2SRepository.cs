using System;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace IL2SRepository {
    public interface IUnitOfWorkFactory {
        IUnitOfWork Begin();
    }

    public interface IUnitOfWork : IDisposable {
        IContainer<TContainer> UsingContainer<TContainer>() where TContainer : DataContext, new();
    }

    public interface IContainer<TContainer> : IDisposable {
        IRepository<TEntity> GetRepository<TEntity>(Func<TContainer, Table<TEntity>> repositorySelector) where TEntity : class;
    }

    public interface IRepository<TEntity> {
        IQueryable<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> specification);
        void Add(TEntity entity);
    }
}
