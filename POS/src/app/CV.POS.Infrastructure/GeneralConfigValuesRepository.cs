using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class GeneralConfigValuesRepository
        :BaseRepository<GeneralConfigValues>, IGeneralConfigValuesRepository
    {
        public GeneralConfigValuesRepository(PosDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
