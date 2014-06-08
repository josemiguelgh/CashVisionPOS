using System.Linq;
using CV.POS.Entities;

namespace CV.POS.Business.Interfaces
{
    public interface IProductBaseRepository
         : IBaseRepository<ProductBase>
    {
        IQueryable<ProductBase> GetProductsWithDependenciesByName(int premiseId, string productName);
        ProductPremise GetStock(short productBaseId, int premiseId);
        ProductBase GetProductsWithGeneralInfoById(int premiseId, short productBaseId);
    }
}
