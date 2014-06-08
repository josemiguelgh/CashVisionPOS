using System;
using System.Collections.Generic;
using System.Data.Entity;
using CV.POS.Business.Interfaces;
using CV.POS.Data;

namespace CV.POS.Infrastructure.Helpers
{
    /// <summary>
    /// A maker of Code Camper Repositories.
    /// </summary>
    /// <remarks>
    /// An instance of this class contains repository factory functions for different types.
    /// Each factory function takes an EF <see cref="DbContext"/> and returns
    /// a repository bound to that DbContext.
    /// <para>
    /// Designed to be a "Singleton", configured at web application start with
    /// all of the factory functions needed to create any type of repository.
    /// Should be thread-safe to use because it is configured at app start,
    /// before any request for a factory, and should be immutable thereafter.
    /// </para>
    /// </remarks>
    public class RepositoryFactories
    {
        /// <summary>
        /// Return the runtime Code Camper repository factory functions,
        /// each one is a factory for a repository of a particular type.
        /// </summary>
        /// <remarks>
        /// MODIFY THIS METHOD TO ADD CUSTOM CODE CAMPER FACTORY FUNCTIONS
        /// </remarks>
        private Dictionary<Type, Func<PosDbContext, object>> GetCodeCamperFactories()
        {
            return new Dictionary<Type, Func<PosDbContext, object>>
                {
                   {typeof(ISessionRepository), dbContext => new SessionRepository(dbContext)},
                   {typeof(IUserRepository), dbContext => new UserRepository(dbContext)},
                   {typeof(ICashBoxRepository), dbContext => new CashBoxRepository(dbContext)},
                   {typeof(ICashBoxStatusRepository), dbContext => new CashBoxStatusRepository(dbContext)},
                   {typeof(IProductBaseRepository), dbContext => new ProductBaseRepository(dbContext)},
                   {typeof(IUnitRepository), dbContext => new UnitRepository(dbContext)},
                   {typeof(IProductMovementRepository), dbContext => new ProductMovementRepository(dbContext)},
                   {typeof(IProductPremiseRepository), dbContext => new ProductPremiseRepository(dbContext)},
                   {typeof(ISaleRepository), dbContext => new SaleRepository(dbContext)},
                   {typeof(ICashMovementRepository), dbContext => new CashMovementRepository(dbContext)},
                   {typeof(ISaleDocumentRepository), dbContext => new SaleDocumentRepository(dbContext)},
                   {typeof(IGeneralConfigValuesRepository), dbContext => new GeneralConfigValuesRepository(dbContext)},
                   {typeof(ISaleDetailsRepository), dbContext => new SaleDetailsRepository(dbContext)},
                };
        }

        /// <summary>
        /// Constructor that initializes with runtime Code Camper repository factories
        /// </summary>
        public RepositoryFactories()  
        {
            _repositoryFactories = GetCodeCamperFactories();
        }

        /// <summary>
        /// Get the repository factory function for the type.
        /// </summary>
        /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
        /// <returns>The repository function if found, else null.</returns>
        /// <remarks>
        /// The type parameter, T, is typically the repository type 
        /// but could be any type (e.g., an entity type)
        /// </remarks>
        public Func<PosDbContext, object> GetRepositoryFactory<T>()
        {

            Func<PosDbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        /// <summary>
        /// Get the factory for <see cref="IBaseRepository{T}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="T">The root type of the repository, typically an entity type.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="IBaseRepository{T}"/>, given an EF <see cref="DbContext"/>.
        /// </returns>
        /// <remarks>
        /// Looks first for a custom factory in <see cref="_repositoryFactories"/>.
        /// If not, falls back to the <see cref="DefaultEntityRepositoryFactory{T}"/>.
        /// You can substitute an alternative factory for the default one by adding
        /// a repository factory for type "T" to <see cref="_repositoryFactories"/>.
        /// </remarks>
        public Func<PosDbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        /// <summary>
        /// Default factory for a <see cref="IBaseRepository{T}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="T">Type of the repository's root entity</typeparam>
        protected virtual Func<PosDbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new BaseRepository<T>(dbContext);
        }

        /// <summary>
        /// Get the dictionary of repository factory functions.
        /// </summary>
        /// <remarks>
        /// A dictionary key is a System.Type, typically a repository type.
        /// A value is a repository factory function
        /// that takes a <see cref="DbContext"/> argument and returns
        /// a repository object. Caller must know how to cast it.
        /// </remarks>
        private readonly Dictionary<Type, Func<PosDbContext, object>> _repositoryFactories;

    }
}
