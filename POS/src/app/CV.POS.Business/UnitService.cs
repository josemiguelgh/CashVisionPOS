using System;
using System.Collections.Generic;
using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;

namespace CV.POS.Business
{

    public interface IUnitService
    {
        List<Unit> GetAllUnits();
        List<string> GetAllUnitsAbbreviation();
        Unit GetUnitByAbbreviation(string abbreviation);
    }

    public sealed class UnitService : IUnitService
    {
        private List<Unit> units;

        public IUow Uow { get; set; }
        
        public UnitService(IUow uow)
        {
            Uow = uow;
        }

        public List<Unit> GetAllUnits()
        {
            SetUnits();
            return units;
        }

        public List<string> GetAllUnitsAbbreviation()
        {
            return (from x in GetAllUnits()
                    select x.Abbreviation).ToList();
        }

        public Unit GetUnitByAbbreviation(string abbreviation)
        {
            SetUnits();
            return units.Single(x => x.Abbreviation.Equals(abbreviation, StringComparison.CurrentCultureIgnoreCase));
        }

        private void SetUnits()
        {
            if (units == null)
                units = Uow.UnitRepository.GetAll().ToList();
        }
    }
}
