using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class CashBoxStatusRepository 
        : BaseRepository<CashboxStatus>, ICashBoxStatusRepository
    {
        public CashBoxStatusRepository(PosDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
