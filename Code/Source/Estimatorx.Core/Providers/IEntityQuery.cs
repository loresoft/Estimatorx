using System;
using System.Linq;
using System.Linq.Expressions;

namespace Estimatorx.Core.Providers
{
    /// <summary>
    /// An <c>interface</c> for common query operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntityQuery<TEntity, TKey> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Find the entity with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the entity to find.</param>
        /// <returns>An instance of TEnity that has the specified identifier if found, otherwise null.</returns>
        TEntity Find(TKey key);

        /// <summary>
        /// Find the first entity using the specified <paramref name="criteria"/> expression.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <returns>
        /// An instance of TEnity that matches the criteria if found, otherwise null.
        /// </returns>
        TEntity FindOne(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Find all entities using the specified <paramref name="criteria"/> expression.
        /// </summary>
        /// <param name="criteria">The criteria expression.</param>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Get all entities as an <see cref="IQueryable{TEntity}"/>.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/> of entities.</returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Gets the number of entities in the data store.
        /// </summary>
        /// <returns>The number of entities in the data store.</returns>
        long Count();

        /// <summary>
        /// Gets the number of entities in the data store with the specified <paramref name="criteria"/>.
        /// </summary>
        /// <returns>The number of entities in the data store specified criteria.</returns>
        long Count(Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// Determines if the specified <paramref name="criteria"/> exists.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns><c>true</c> if criteria expression is found; otherwise <c>false</c>.</returns>
        bool Exists(Expression<Func<TEntity, bool>> criteria);


        /// <summary>
        /// Gets the key for the specified <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The entity to get the key from.</param>
        /// <returns>The key for the specified entity.</returns>
        TKey EntityKey(TEntity entity);
    }
}