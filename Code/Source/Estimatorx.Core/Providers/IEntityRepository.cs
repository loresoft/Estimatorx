using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Estimatorx.Core.Providers
{
    /// <summary>
    /// An <c>interface</c> for common data operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntityRepository<TEntity, TKey> : IEntityQuery<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Inserts the specified <paramref name="entity"/> to the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>The entity that was inserted.</returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts the specified <paramref name="entities"/> to the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Inserts the specified entities in a batch opertation to the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        void InsertBatch(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified <paramref name="entity"/> in the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The entity that was updated.</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates the specified <paramref name="entities"/> in the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be updated.</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Saves the specified <paramref name="entity"/> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The entity that was updated.</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Saves the specified <paramref name="entities"/> in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entities">The entities to be updated.</param>
        void Save(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes an entity with the specified <paramref name="key"/> from the underlying data repository.
        /// </summary>
        /// <param name="key">The key of the entity to delete.</param>
        /// <returns>The number of documents deleted</returns>
        long Delete(TKey key);

        /// <summary>
        /// Deletes the specified <paramref name="entity"/> from the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>The number of documents deleted</returns>
        long Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified <paramref name="entities"/> from the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be deleted.</param>
        /// <returns>The number of documents deleted</returns>
        long Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes all from collection that meet the specified <paramref name="criteria" />.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// <returns>The number of documents deleted</returns>
        long DeleteAll(Expression<Func<TEntity, bool>> criteria);
    }
}