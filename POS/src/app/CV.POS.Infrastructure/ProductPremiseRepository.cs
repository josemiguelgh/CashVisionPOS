using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class ProductPremiseRepository 
        : BaseRepository<ProductPremise>, IProductPremiseRepository
    {
        public ProductPremiseRepository(PosDbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}
