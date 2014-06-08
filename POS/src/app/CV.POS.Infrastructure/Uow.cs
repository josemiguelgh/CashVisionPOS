using System;
using System.Security.Policy;
using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;
using CV.POS.Infrastructure.Helpers;

namespace CV.POS.Infrastructure
{
    /// <summary>
    /// The Code Camper "Unit of Work"
    ///     1) decouples the repos from the controllers
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a <see cref="Person"/>.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data layer (which is the EF DbContext in Code Camper).
    /// </remarks>
    public class Uow : IUow, IDisposable
    {
        public Uow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        // Code Camper repositories

        //public IRepository<Room> Rooms { get { return GetStandardRepo<Room>(); } }
        //public IRepository<TimeSlot> TimeSlots { get { return GetStandardRepo<TimeSlot>(); } }
        //public IRepository<Track> Tracks { get { return GetStandardRepo<Track>(); } }
        public ISessionRepository SessionRepository { get { return GetRepo<ISessionRepository>(); } }
        public IUserRepository UserRepository { get { return GetRepo<IUserRepository>(); } }
        public ICashBoxRepository CashBoxRepository { get { return GetRepo<ICashBoxRepository>(); } }
        public ICashBoxStatusRepository CashBoxStatusRepository { get { return GetRepo<ICashBoxStatusRepository>(); } }
        public IProductBaseRepository ProductBaseRepository { get { return GetRepo<IProductBaseRepository>(); } }
        public IUnitRepository UnitRepository { get { return GetRepo<IUnitRepository>(); } }
        public IProductMovementRepository ProductMovementRepository { get { return GetRepo<IProductMovementRepository>(); } }
        public IProductPremiseRepository ProductPremiseRepository { get { return GetRepo<IProductPremiseRepository>(); } }
        public ICashMovementRepository CashMovementRepository { get { return GetRepo<ICashMovementRepository>(); } }
        public ISaleRepository SaleRepository { get { return GetRepo<ISaleRepository>(); } }
        public ISaleDocumentRepository SaleDocumentRepository { get { return GetRepo<ISaleDocumentRepository>(); } }
        public IGeneralConfigValuesRepository GeneralConfigValuesRepository { get { return GetRepo<IGeneralConfigValuesRepository>(); } }
        public ISaleDetailsRepository SaleDetailsRepository { get { return GetRepo<ISaleDetailsRepository>(); } }
        //public IPersonsRepository Persons { get { return GetRepo<IPersonsRepository>(); } }
        //public IAttendanceRepository Attendance { get { return GetRepo<IAttendanceRepository>(); } }

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {
            //System.Diagnostics.Debug.WriteLine("Committed");
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new PosDbContext();
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IBaseRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private PosDbContext DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}