using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class SaleRepository 
        : BaseRepository<Sale>, ISaleRepository
    {
        public SaleRepository(PosDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
