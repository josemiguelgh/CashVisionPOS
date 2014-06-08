using System;
using System.Collections.Generic;
using System.Linq;
using CV.POS.Entities;

namespace CV.POS.Business.Helpers.EntityHelpers
{
    public class UnitHelper
    {
        private List<Unit> allUnits;

        public UnitHelper(List<Unit> allUnits)
        {
            this.allUnits = allUnits;
        }

        public List<Unit> GetAllUnits()
        {
            return allUnits;
        }

        public List<string> GetAllUnitsAbbreviation()
        {
            return (from x in GetAllUnits()
                    select x.Abbreviation).ToList();
        }

        public Unit GetUnitByAbbreviation(string abbreviation)
        {
            return allUnits.Single(x => x.Abbreviation.Equals(abbreviation, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
