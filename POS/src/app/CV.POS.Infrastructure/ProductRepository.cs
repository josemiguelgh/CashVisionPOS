using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class ProductRepository 
        : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(PosDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
