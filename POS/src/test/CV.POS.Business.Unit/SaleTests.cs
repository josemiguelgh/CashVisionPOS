using System;
using System.Collections.Generic;
using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;
using Moq;
using NUnit.Framework;

namespace CV.POS.Business.Unit
{
    [TestFixture]
    public class SaleTests
    {
        [Test]
        public void CreateSaleService_WithUow_ShouldPerform()
        {
            var saleService = new SaleService(GetValidUowStubBase().Object);
            Assert.IsNotNull(saleService);
        }

        [Test]
        public void PerformSale_WithoutProducts_ThrowsEx()
        {
            var saleService = new SaleService(GetValidUowStubBase().Object);
            var ex = Assert.Throws<ArgumentNullException>(() => saleService.PerformSale(1, null, 1, "", "", ""));
            Assert.AreEqual("saleLineDtos", ex.ParamName);
        }

        [Test]
        public void PerformSale_OneItemAndWithoutStock_ReturnsFalse()
        {
            var saleLineDtos = new List<SaleLineDto> {new SaleLineDto(1, "Lapicero 043", "UND", 10000, new decimal(0.50), new decimal(1.50))};
            
            var saleService = new SaleService(GetValidUowStubFull().Object);
            var operationResult = saleService.PerformSale(1, saleLineDtos, 1, "Boleta", "", "");
            Assert.IsFalse(operationResult.Succeed);
        }

        [Test]
        public void PerformSale_OneItemAndWithStock_ReturnsTrue()
        {
            var saleLineDtos = new List<SaleLineDto> { new SaleLineDto(1, "Lapicero 043", "UND", 2, new decimal(0.50), new decimal(1.50)) };

            var saleService = new SaleService(GetValidUowStubFull().Object);
            var operationResult = saleService.PerformSale(1, saleLineDtos, 1, "Boleta", "", "");

            Assert.IsTrue(operationResult.Succeed);
        }

        private static Mock<IUow> GetValidUowStubBase()
        {
            //var unitsAsQueryable = Enumerable.Empty<Entities.Unit>().AsQueryable();
            var unitsAsQueryable = new List<Entities.Unit>
            {
                new Entities.Unit {UnitId = 1, Name = "Unidad", Abbreviation = "UND"},
                new Entities.Unit {UnitId = 2, Name = "Docena", Abbreviation = "DOC"},
                new Entities.Unit {UnitId = 3, Name = "Ciento", Abbreviation = "CTO"},
                new Entities.Unit {UnitId = 4, Name = "Millar", Abbreviation = "MLL"}
            }.AsQueryable();

            var unitRepoStub = new Mock<IUnitRepository>();
            unitRepoStub.Setup(x => x.GetAll()).Returns(unitsAsQueryable);
            unitRepoStub.Setup(x => x.GetUnitEquivalenceFactor(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(1);

            var uowStub = new Mock<IUow>();
            uowStub.SetupGet(x => x.UnitRepository).Returns(unitRepoStub.Object);
            return uowStub;
        }

        private static Mock<IUow> GetValidUowStubFull()
        {
            var productBaseRepoStub = GetProductBaseRepoStub();
            var productMovementRepoStub = GetProductMovementRepoStub();
            var productPremiseRepoStub = GetProductPremiseRepoStub();
            var cashBoxRepoStub = GetCashBoxRepoStub();
            var cashMovementRepoStub = new Mock<ICashMovementRepository>();
            var saleRepoStub = new Mock<ISaleRepository>();
            var saleDocumentRepoStub = new Mock<ISaleDocumentRepository>();
            var saleDetailsRepoStub = new Mock<ISaleDetailsRepository>();
            var generalConfigValuesRepoStub = GetGeneralConfigValuesRepoStub();

            var uowStub = GetValidUowStubBase();
            uowStub.SetupGet(x => x.ProductBaseRepository).Returns(productBaseRepoStub.Object);
            uowStub.SetupGet(x => x.ProductMovementRepository).Returns(productMovementRepoStub.Object);
            uowStub.SetupGet(x => x.ProductPremiseRepository).Returns(productPremiseRepoStub.Object);
            uowStub.SetupGet(x => x.CashBoxRepository).Returns(cashBoxRepoStub.Object);
            uowStub.SetupGet(x => x.CashMovementRepository).Returns(cashMovementRepoStub.Object);
            uowStub.SetupGet(x => x.SaleRepository).Returns(saleRepoStub.Object);
            uowStub.SetupGet(x => x.SaleDocumentRepository).Returns(saleDocumentRepoStub.Object);
            uowStub.SetupGet(x => x.SaleDetailsRepository).Returns(saleDetailsRepoStub.Object);
            uowStub.SetupGet(x => x.GeneralConfigValuesRepository).Returns(generalConfigValuesRepoStub.Object);
            return uowStub;
        }

        private static Mock<IGeneralConfigValuesRepository> GetGeneralConfigValuesRepoStub()
        {
            var result = new Mock<IGeneralConfigValuesRepository>();
            result.Setup(x => x.SearchFor(y => y.Name == "GrupoBoleta"))
                .Returns(new List<GeneralConfigValues>()
                {
                    new GeneralConfigValues(){Name = "GrupoBoleta", Value = "001"}
                }.AsQueryable());
            result.Setup(x => x.SearchFor(y => y.Name == "UlitmoNroBoleta"))
                .Returns(new List<GeneralConfigValues>()
                {
                    new GeneralConfigValues(){Name = "UltimoNroBoleta", Value = "45"}
                }.AsQueryable());
            result.Setup(x => x.SearchFor(y => y.Name == "GrupoFactura"))
                .Returns(new List<GeneralConfigValues>()
                {
                    new GeneralConfigValues(){Name = "GrupoFactura", Value = "001"}
                }.AsQueryable());
            result.Setup(x => x.SearchFor(y => y.Name == "UltimoNroFactura"))
                .Returns(new List<GeneralConfigValues>()
                {
                    new GeneralConfigValues(){Name = "UltimoNroFactura", Value = "10"}
                }.AsQueryable());
            
            return result;
        }

        private static Mock<IProductPremiseRepository> GetProductPremiseRepoStub()
        {
            return new Mock<IProductPremiseRepository>();
        }

        private static Mock<IProductMovementRepository> GetProductMovementRepoStub()
        {
            return new Mock<IProductMovementRepository>();
        }

        private static Mock<ICashBoxRepository> GetCashBoxRepoStub()
        {
            var cashBoxRepoStub = new Mock<ICashBoxRepository>();
            cashBoxRepoStub
                .Setup(x => x.GetById(1))
                .Returns(new Cashbox
                {
                    CashboxId = 1,
                    CurrentAmount = 1000,
                    Name = "DefaultCashbox",
                    PremiseId = 1
                });
            return cashBoxRepoStub;
        }

        private static Mock<IProductBaseRepository> GetProductBaseRepoStub()
        {
            var productBaseRepoStub = new Mock<IProductBaseRepository>();
            productBaseRepoStub
                .Setup(x => x.GetStock(It.IsAny<short>(), It.IsAny<int>()))
                .Returns(new ProductPremise()
                {
                    ProductId = 1,
                    PremiseId = 1,
                    CurrentStock = 1000,
                    MinimunStock = 200,
                    StockDefaultUnitAbbr = "UND"
                });

            var productWithGeneralInfo = new ProductBase
            {
                Product =
                    new List<Product>
                    {
                        new Product
                        {
                            ProductId = 1,
                            ProductPremise = new List<ProductPremise>
                                {
                                    new ProductPremise
                                    {
                                        ProductId = 1,
                                        PremiseId = 1,
                                        CurrentStock = 100,
                                        MinimunStock = 10,
                                        StockDefaultUnitAbbr = "UND"
                                    }
                                }
                        }
                    }
            };

            productBaseRepoStub
                .Setup(x => x.GetProductsWithGeneralInfoById(It.IsAny<int>(), It.IsAny<short>()))
                .Returns(productWithGeneralInfo);
            return productBaseRepoStub;
        }
    }
}
