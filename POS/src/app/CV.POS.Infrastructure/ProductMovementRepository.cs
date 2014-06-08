using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class ProductMovementRepository 
        : BaseRepository<ProductMovement>, IProductMovementRepository
    {
        public ProductMovementRepository(PosDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
