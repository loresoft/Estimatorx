using System;
using System.Linq;
using System.Linq.Expressions;

namespace Estimatorx.Data.Mongo.Providers
{
    /// <summary>
    /// An <c>interface</c> for common query operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntityQuery<TEntity, TKey>
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
    }
}
