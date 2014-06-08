using System;
using System.Linq;
using CV.POS.Business.Interfaces;

namespace CV.POS.Business.Helpers.EntityHelpers
{
    internal interface ISaleDocumentType
    {
        string Name { get; }
        string GetNextDocumentNumber(IGeneralConfigValuesRepository repository);
        decimal? GetSubTotalForDocument(decimal saleAmount);
        decimal? GetIgvForDocument(decimal saleAmount);
    }

    internal class Boleta : ISaleDocumentType
    {
        public string Name { get { return "Boleta"; } }

        public string GetNextDocumentNumber(IGeneralConfigValuesRepository repository)
        {
            var boletaPrefix = repository.SearchFor(x => x.Name == "GrupoBoleta").Single().Value;
            var nroBoleta = Convert.ToInt32(repository.SearchFor(x => x.Name == "UlitmoNroBoleta").Single().Value);

            return string.Format("{0}-{1}", boletaPrefix, nroBoleta.ToString("0000000"));
        }

        public decimal? GetSubTotalForDocument(decimal saleAmount)
        {
            return null;
        }

        public decimal? GetIgvForDocument(decimal saleAmount)
        {
            return null;
        }
    }

    internal class Factura : ISaleDocumentType
    {
        public string Name { get { return "Factura"; } }
        private decimal igvPercentage;

        public Factura(decimal igvPercentage)
        {
            this.igvPercentage = igvPercentage;
        }

        public string GetNextDocumentNumber(IGeneralConfigValuesRepository repository)
        {
            var boletaPrefix = repository.SearchFor(x => x.Name == "GrupoFactura").Single().Value;
            var nroBoleta = Convert.ToInt32(repository.SearchFor(x => x.Name == "UlitmoNroFactura").Single().Value);

            return string.Format("{0}-{1}", boletaPrefix, nroBoleta.ToString("0000000"));
        }

        public decimal? GetIgvForDocument(decimal saleAmount)
        {
            return saleAmount - GetSubTotalForDocument(saleAmount);
        }

        public decimal? GetSubTotalForDocument(decimal saleAmount)
        {
            return Math.Round(saleAmount / (1 + GetIgvDecimal()), 2);
        }

        private decimal GetIgvDecimal()
        {
            return igvPercentage/100;
        }
    }

    internal class NoDocument : ISaleDocumentType
    {
        public string Name { get { return "NoDocument"; } }

        public string GetNextDocumentNumber(IGeneralConfigValuesRepository repository)
        {
            return null;
        }

        public decimal? GetIgvForDocument(decimal saleAmount)
        {
            return null;
        }

        public decimal? GetSubTotalForDocument(decimal saleAmount)
        {
            return null;
        }
    }
}
