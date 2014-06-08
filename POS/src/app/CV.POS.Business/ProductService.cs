using System;
using System.Collections.Generic;
using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;

namespace CV.POS.Business
{
    public interface IProductService
    {
        List<ProductBase> GetProductsWithUnitPremise(int premiseId, string productSearchNamePattern);
        ProductBase GetProductBaseWithPremise(int premiseId);
        void SetProductBaseWithDependencies(ProductBase productGeneralInfo);
        decimal GetProductSalePrice(string unitAbbreviation);
        short? GetProductId(byte premiseId);
    }

    public sealed class ProductService:IProductService
    {
        private IUow uow;
        private IUnitService unitService;
        private ProductBase productBase;
        private ProductBase productBaseWithDependencies;
        
        public ProductService(IUow uow, IUnitService unitService)
        {
            this.uow = uow;
            this.unitService = unitService;
        }

        public void SetProductBase(ProductBase productBaseInfo)
        {
            productBase = productBaseInfo;
        }

        public void SetProductBaseWithDependencies(ProductBase productGeneralInfo)
        {
            productBaseWithDependencies = productGeneralInfo;
        }

        public ProductBase GetProductBaseWithPremise(int premiseId)
        {
            if (productBase == null)
                throw new NullReferenceException("productBaseWithDependencies is null in GetProductSalePrice");
            return uow.ProductBaseRepository
                .GetProductsWithGeneralInfoById(premiseId, productBase.ProductBaseId);
        }

        public List<ProductBase> GetProductsWithUnitPremise(int premiseId, string productSearchNamePattern)
        {
            return uow.ProductBaseRepository
                .GetProductsWithDependenciesByName(premiseId, productSearchNamePattern)
                .ToList();
        }

        public decimal GetProductSalePrice(string unitAbbreviation)
        {
            if(productBaseWithDependencies == null)
                throw new NullReferenceException("productBaseWithDependencies is null in GetProductSalePrice");
            var unit = unitService.GetUnitByAbbreviation(unitAbbreviation);
            return productBaseWithDependencies.Product.First().ProductUnit.Single(x => x.UnitId == unit.UnitId).SalePrice;
        }

        public short? GetProductId(byte premiseId)
        {
            return (short?)GetProductBaseWithPremise(premiseId)
                .Product.First().ProductId;
        }
    }
}
