using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class CashMovementRepository 
        : BaseRepository<CashMovement>, ICashMovementRepository
    {
        public CashMovementRepository(PosDbContext dbContext)
            :base(dbContext)
        {
            
        }
    }
}
