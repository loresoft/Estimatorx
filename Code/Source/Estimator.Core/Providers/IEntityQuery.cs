using System;
using System.Linq;
using System.Linq.Expressions;

namespace Estimator.Core.Providers
{
    /// <summary>
    /// An interface for common query operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntityQuery<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Find the entity with the specified key.
        /// </summary>
        /// <param name="key">The key of the entity to find.</param>
        /// <returns>An instance of TEnity that has the specified identifier if found, otherwise null.</returns>
        TEntity Find(TKey key);

        /// <summary>
        /// Find the first entity using the specified criteria expression.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <returns>
        /// An instance of TEnity that matches the criteria if found, otherwise null.
        /// </returns>
        TEntity FindOne(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Find all entities using the specified criteria expression.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Get all <typeparamref name="TEntity"/> entities as an IQueryable
        /// </summary>
        /// <returns>IQueryable of <typeparamref name="TEntity"/>.</returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Gets the number of entities in the collection.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Determinds if the specified criteria exists.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns><c>true</c> if criteria expression is found; otherwise <c>false</c>.</returns>
        bool Exists(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Gets the key for the specified entity.
        /// </summary>
        /// <param name="entity">The entity to get the key from.</param>
        /// <returns>The key for the specified entity.</returns>
        TKey EntityKey(TEntity entity);
    }
}