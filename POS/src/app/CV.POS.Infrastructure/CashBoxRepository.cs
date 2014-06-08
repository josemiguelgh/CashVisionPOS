using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class CashBoxRepository 
        : BaseRepository<Cashbox>, ICashBoxRepository
    {
        public CashBoxRepository(PosDbContext dbContext)
            : base(dbContext) { }
    }
}
