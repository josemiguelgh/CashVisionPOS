using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        public UnitRepository(PosDbContext dbContext)
            :base(dbContext)
        {
            
        }

        public int GetUnitEquivalenceFactor(string lowerUnitAbbr, string higherUnitAbbr)
        {
            return (from x in Db.UnitEquivalence
                where x.LowerUnitAbbr == lowerUnitAbbr
                      && x.HigherUnitAbbr == higherUnitAbbr
                select x.EquivalenceFactor).Single();
        }
    }
}
