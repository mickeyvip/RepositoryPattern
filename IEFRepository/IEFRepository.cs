using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;

namespace IEFRepository {
    public interface IUnitOfWorkFactory {
        IUnitOfWork Begin();
    }

    public interface IUnitOfWork : IDisposable {
        IContainer<TContainer> UsingContainer<TContainer>() where TContainer : ObjectContext, new();
    }

    public interface IContainer<TContainer> : IDisposable {
        IRepository<TEntity> GetRepository<TEntity>(Func<TContainer, ObjectQuery<TEntity>> repositorySelector) where TEntity : EntityObject;
    }

    public interface IRepository<TEntity> {
        IQueryable<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> specification);
        void Add(TEntity entity);
    }
}
