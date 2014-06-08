namespace CV.POS.Business.Interfaces
{
    /// <summary>
    /// Interface for the Code Camper "Unit of Work"
    /// </summary>
    public interface IUow
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        ISessionRepository SessionRepository { get; }
        IUserRepository UserRepository { get; }
        ICashBoxRepository CashBoxRepository { get; }
        ICashBoxStatusRepository CashBoxStatusRepository { get; }
        IProductBaseRepository ProductBaseRepository { get; }
        IUnitRepository UnitRepository { get; }
        IProductMovementRepository ProductMovementRepository { get; }
        IProductPremiseRepository ProductPremiseRepository { get; }
        ICashMovementRepository CashMovementRepository { get ; }
        ISaleRepository SaleRepository { get ; }
        ISaleDocumentRepository SaleDocumentRepository { get ; }
        IGeneralConfigValuesRepository GeneralConfigValuesRepository { get ; }
        ISaleDetailsRepository SaleDetailsRepository { get ; }
    }
}
