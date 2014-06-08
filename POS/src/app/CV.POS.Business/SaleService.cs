using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using CV.POS.Business.Common;
using CV.POS.Business.Helpers;
using CV.POS.Business.Helpers.EntityHelpers;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;

namespace CV.POS.Business
{
    public interface IGeneralService
    {
        OperationResult PerformSale(byte premiseId, List<SaleLineDto> saleLineDtos, int sessionId, string saleDocumentType, string name, string dniRuc);
    }

    public class SaleService : IGeneralService
    {
        private IUow uow;
        private readonly UnitHelper unitHelper;
        private decimal igvPercentage;

        public SaleService(IUow uow)
        {
            if(uow == null)
                throw new ArgumentNullException("uow");
            this.uow = uow;
            unitHelper = new UnitHelper(uow.UnitRepository.GetAll().ToList());
            igvPercentage = 18;
        }

        public OperationResult PerformSale(byte premiseId, List<SaleLineDto> saleLineDtos, int sessionId, string saleDocumentType, string customerName, string dniRuc)
        {
            if(saleLineDtos == null || saleLineDtos.Count == 0)
                throw new ArgumentNullException("saleLineDtos");

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
            {
                var operationResult = CheckStockForProducts(premiseId, saleLineDtos);
                if (!operationResult.Succeed)
                {
                    scope.Dispose();
                    return operationResult;
                }
                //Movements
                var productMovements = CreateSaleProductMovements(premiseId, saleLineDtos, sessionId);
                DiscountProductsPremiseStock(premiseId, saleLineDtos);
                //Cash
                DiscountAmountForCashbox(GetSaleAmount(saleLineDtos));
                var cashMovementCreated = CreateCashMovement(sessionId, GetSaleAmount(saleLineDtos), 
                    Constants.CashMovementCategory.Sale,Constants.CashMovementType.In);
                //Sale and Sale Document
                CreateSaleInfo(sessionId, cashMovementCreated, GetSaleDocumentObject(saleDocumentType), 
                    customerName, saleLineDtos, dniRuc, productMovements);
                uow.Commit();
                scope.Complete();
            }
            //TODO: Pending perform printing actions!!
            return new OperationResult { Succeed = true };
        }

        #region private methods

        private OperationResult CheckStockForProducts(int premiseId, IEnumerable<SaleLineDto> saleLineDtos)
        {
            foreach (var saleLine in saleLineDtos)
            {
                var productOnPremise = uow.ProductBaseRepository.GetStock(saleLine.ProductBaseId, premiseId);
                var quantityWithControlUnits = ConvertToQuantityWithControlUnits(productOnPremise.StockDefaultUnitAbbr, saleLine.Unit, saleLine.Quantity);
                if (productOnPremise.CurrentStock < quantityWithControlUnits)
                    return new OperationResult { Succeed = false, Message = string.Format("No hay suficiente stock para vender el producto {0}", saleLine.ProductName) };
            }
            return new OperationResult { Succeed = true };
        }

        private int ConvertToQuantityWithControlUnits(string stockDefaultUnitAbbr, string currentUnitAbbr, short quantity)
        {
            var unitEquivalenceFactor = uow.UnitRepository.GetUnitEquivalenceFactor(stockDefaultUnitAbbr, currentUnitAbbr);
            return quantity * unitEquivalenceFactor;
        }

        private IEnumerable<ProductMovement> CreateSaleProductMovements(byte premiseId, IEnumerable<SaleLineDto> saleLineDtos, int sessionId)
        {
            var productMovements = new List<ProductMovement>();
            foreach (SaleLineDto saleLine in saleLineDtos)
            {
                int productId = uow.ProductBaseRepository
                    .GetProductsWithGeneralInfoById(premiseId, saleLine.ProductBaseId)
                    .Product.First().ProductId;

                //var unitId = uow.UnitRepository.SearchFor(x => x.Abbreviation == saleLine.Unit).Single().UnitId;
                productMovements.Add(
                InsertProductMovement(premiseId, sessionId, productId, saleLine.Quantity,
                    unitHelper.GetUnitByAbbreviation(saleLine.Unit).UnitId, Constants.ProductMovementCategory.Sale,
                    Constants.ProductMovementType.Out));
            }
            return productMovements;
        }

        private ProductMovement InsertProductMovement(byte premiseId, int sessionId, int productId, int quantity, byte unitId,
            string movementCategory, string movementType)
        {
            var productMovement = new ProductMovement
            {
                PremiseId = premiseId,
                MovementCategory = movementCategory,
                MovementType = movementType,
                //TODO: change to int
                ProductId = productId,
                UnitId = unitId,
                Quantity = quantity,
                //TODO: change to datetime
                MovementDate = DateTime.Now,
                SessionId = sessionId
            };
            uow.ProductMovementRepository.Insert(productMovement);
            return productMovement;
        }

        private void DiscountProductsPremiseStock(byte premiseId, IEnumerable<SaleLineDto> saleLineDtos)
        {
            foreach (SaleLineDto saleLine in saleLineDtos)
            {
                var productPremise = uow.ProductBaseRepository
                    .GetProductsWithGeneralInfoById(premiseId, saleLine.ProductBaseId)
                    .Product.First()
                    .ProductPremise.Single();

                int quantityToDiscount = ConvertToQuantityWithControlUnits(productPremise.StockDefaultUnitAbbr, saleLine.Unit,
                    saleLine.Quantity);
                productPremise.CurrentStock = productPremise.CurrentStock - quantityToDiscount;
                uow.ProductPremiseRepository.Update(productPremise);
            }
        }

        private void DiscountAmountForCashbox(decimal saleAmount)
        {
            var cashBoxService = new CashBoxService(uow);
            var defatultCashbox = cashBoxService.GetDefatultCashbox();
            defatultCashbox.CurrentAmount = defatultCashbox.CurrentAmount - saleAmount;
            uow.CashBoxRepository.Update(defatultCashbox);
        }

        private CashMovement CreateCashMovement(int sessionId, decimal saleAmount, string movementCategory,
            string movementType)
        {
            var cashBoxService = new CashBoxService(uow);

            var cashMovement = new CashMovement
            {
                MovementCategory = movementCategory,
                MovementType = movementType,
                MovementDate = DateTime.Now,
                Amount = saleAmount,
                MovementStatus = Constants.CashMovementStatus.Ok,
                CashboxId = cashBoxService.GetDefatultCashbox().CashboxId,
                SessionId = sessionId
            };
            uow.CashMovementRepository.Insert(cashMovement);
            return cashMovement;
        }

        private decimal GetSaleAmount(IEnumerable<SaleLineDto> saleLineDtos)
        {
            return saleLineDtos.Aggregate<SaleLineDto, decimal>
                (0, (current, saleLineDto) => current + (saleLineDto.UnitPrice * saleLineDto.Quantity));
        }

        private ISaleDocumentType GetSaleDocumentObject(string saleDocumentType)
        {
            switch (saleDocumentType)
            {
                case Constants.SaleDocumentType.Boleta:
                    return new Boleta();
                case Constants.SaleDocumentType.Factura:
                    return new Factura(igvPercentage);
                case Constants.SaleDocumentType.NoDocument:
                    return new NoDocument();
                default:
                    return new NoDocument();
            }
        }

        private void CreateSaleInfo(int sessionId, CashMovement cashMovementCreated, 
            ISaleDocumentType saleDocumentType, string customerName,List<SaleLineDto> saleLineDtos, 
            string ruc, IEnumerable<ProductMovement> productMovements)
        {
            var saleAmount = GetSaleAmount(saleLineDtos);

            var sale = new Sale
            {
                Date = DateTime.Now,
                Status = Constants.SaleStatus.Created,
                SessionId = sessionId,
                CashMovement = cashMovementCreated
            };
            uow.SaleRepository.Insert(sale);

            var saleDocument = new SaleDocument
            {
                DocumentType = saleDocumentType.Name,
                Number = saleDocumentType.GetNextDocumentNumber(uow.GeneralConfigValuesRepository),
                Date = DateTime.Now,
                CustomerName = customerName,
                RUC = ruc,
                SubTotal = saleDocumentType.GetSubTotalForDocument(saleAmount),
                IGV = saleDocumentType.GetIgvForDocument(saleAmount),
                Total = saleAmount,
                Status = Constants.SaleDocumentStatus.Created,
                Sale = sale
            };
            uow.SaleDocumentRepository.Insert(saleDocument);

            var zippedCollection = saleLineDtos.Zip(productMovements, (x, y) => new {SaleLineDto = x, ProductMovement = y});
            foreach (var zippedItem in zippedCollection)
            {
                var saleDetail = new SaleDetails()
                {
                    //TODO: Ingresar correlativo en linea del documento ([SaleDetailNumber])
                    ProductMovement = zippedItem.ProductMovement,
                    Quantity = zippedItem.SaleLineDto.Quantity,
                    SaleDocument = saleDocument,
                    SinglePrice = zippedItem.SaleLineDto.UnitPrice,
                    TotalPrice = zippedItem.SaleLineDto.LinePrice
                };
               uow.SaleDetailsRepository.Insert(saleDetail);
            }
        }

        #endregion
    }

    public class SaleLineDto
    {
        public short ProductBaseId { get; private set; }
        public string ProductName { get; private set; }
        public string Unit { get; private set; }
        public short Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal LinePrice { get; private set; }

        public SaleLineDto(short productBaseId, string productName, string unit, short quantity, decimal unitPrice, decimal linePrice)
        {
            ProductBaseId = productBaseId;
            ProductName = productName;
            Unit = unit;
            Quantity = quantity;
            UnitPrice = unitPrice;
            LinePrice = linePrice;
        }
    }
}
