using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class SaleDetailsRepository 
        : BaseRepository<SaleDetails>, ISaleDetailsRepository
    {
        public SaleDetailsRepository(PosDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
