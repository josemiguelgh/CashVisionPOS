using CV.POS.Entities;

namespace CV.POS.Business.Interfaces
{
    public interface IUnitRepository : IBaseRepository<Unit>
    {
        int GetUnitEquivalenceFactor(string lowerUnitAbbr, string higherUnitAbbr);
    }
}
