using System;
using System.Collections.Generic;

namespace Estimator.Core.Providers
{
    /// <summary>
    /// An interface for common data opertions.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntityRepository<TEntity, TKey> : IEntityQuery<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Inserts the specified entity to the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>The entity that was inserted.</returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts the specified entities to the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Inserts the specified entities in a batch opertation to the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        void InsertBatch(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified entity in the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The entity that was updated.</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates the specified entities in the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be updated.</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Saves the specified entity in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The entity that was updated.</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Saves the specified entities in the underlying data repository by inserting if doesn't exist, or updating if it does.
        /// </summary>
        /// <param name="entities">The entities to be updated.</param>
        void Save(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes an entity with the specified key from the underlying data repository.
        /// </summary>
        /// <param name="key">The key of the entity to delete.</param>
        void Delete(TKey key);

        /// <summary>
        /// Deletes the specified entity from the underlying data repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified entities from the underlying data repository.
        /// </summary>
        /// <param name="entities">The entities to be deleted.</param>
        void Delete(IEnumerable<TEntity> entities);
    }
}