using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class SaleDocumentRepository 
        :BaseRepository<SaleDocument>, ISaleDocumentRepository
    {
        public SaleDocumentRepository(PosDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
