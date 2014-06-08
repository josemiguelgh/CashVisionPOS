using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using CV.POS.Business;
using CV.POS.Entities;
using CV.POS.Wpf.Common;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.ViewModel
{
    public sealed class ProductSelectionViewModel
        : ViewModelCustomBase<ProductSelectionViewModel>
    {
        #region Fields and Properties

        private ProductBase selectedProduct;
        private ObservableCollection<ProductBase> filteredProducts;
        private readonly IProductService productService;
        private readonly IUnitService unitService;
        private readonly ICommand searchProductCommand;
        private readonly ICommand addProductCommand;

        private string productSearchPattern;
        public string ProductSearchPattern
        {
            get { return productSearchPattern; }
            set
            {
                if (productSearchPattern == value)
                    return;
                productSearchPattern = value;
                RaisePropertyChanged("ProductSearchPattern");
            }
        }

        private string productName;
        public string ProductName
        {
            get { return productName; }
            set
            {
                if (productName == value)
                    return;
                productName = value;
                RaisePropertyChanged(vm=>vm.ProductName);
            }
        }

        private short quantity;
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.Error6)]
        [RegularExpression(Constants.NumericRegEx, ErrorMessage = Constants.Error13)]
        public short Quantity
        {
            get { return quantity; }
            set
            {
                if (value == quantity)
                    return;
                quantity = value;
                RaisePropertyChanged(vm => vm.Quantity);
                RaisePropertyChanged(vm => vm.LineOrderPrice);
            }
        }

        private string unit;
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.Error6)]
        [RegularExpression(Constants.AlphabeticRegEx, ErrorMessage = Constants.Error10)]
        //[CustomValidation(typeof(ProductSelectionViewModel),"IsInRegisteredUnits")]
        public string Unit
        {
            get { return unit; }
            set
            {
                if (value == unit)
                    return;
                unit = value;
                RaisePropertyChanged(vm => vm.Unit);
                try
                {
                    if (!ValidateUnitAbbreviation(unit))
                        throw new Exception();
                    UpdateUnitPrice(unit);
                }
                catch (Exception)
                {
                    throw new ArgumentException(Constants.Error11);
                }
            }
        }

        private bool ValidateUnitAbbreviation(string currentUnitAbbreviation)
        {
            return unitService.GetAllUnitsAbbreviation().Any(x => x.Equals(currentUnitAbbreviation, StringComparison.CurrentCultureIgnoreCase));
        }

        private decimal unitPrice;
        public decimal UnitPrice
        {
            get { return unitPrice; }
            set
            {
                if (value == unitPrice)
                    return;
                unitPrice = value;
                RaisePropertyChanged(vm => vm.UnitPrice);
                RaisePropertyChanged(vm=>vm.LineOrderPrice);
            }
        }
        
        public decimal LineOrderPrice
        {
            get { return UnitPrice * Quantity; }
        }

        public ObservableCollection<ProductBase> FilteredProducts
        {
            get { return filteredProducts; }
            set
            {
                if (value == filteredProducts)
                    return;
                filteredProducts = value;
                RaisePropertyChanged(vm=>vm.FilteredProducts);
            }
        }

        public ProductBase SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (value == selectedProduct)
                    return;
                selectedProduct = value;
                productService.SetProductBaseWithDependencies(value);
                RaisePropertyChanged(vm=>vm.SelectedProduct);
                UpdateProductSelection(value);
            }
        }

        private void UpdateProductSelection(ProductBase productBase)
        {
            Quantity = 1;
            if (productBase == null)
            {
                ProductName = "";
                Unit = "";
                UnitPrice = 0;
            }
            else
            {
                ProductName = productBase.Name;
                Unit = productBase.SaleDefaultUnitAbbr;
                UpdateUnitPrice(productBase.SaleDefaultUnitAbbr);
            }
        }

        public ICommand SearchProductCommand
        {
            get { return searchProductCommand; }
        }

        public ICommand AddProductCommand
        {
            get { return addProductCommand; }
        }

        private List<short> productsInSaleList;

        #endregion
        

        public ProductSelectionViewModel(IProductService productService, IUnitService unitService)
        {
            Messenger.Default.Register<List<short>>(this,
                Constants.Token10, SetProductsInSaleList);
            this.productService = productService;
            this.unitService = unitService;
            searchProductCommand = new RelayCommand(SearchProduct);
            addProductCommand = new RelayCommand(AddProductToSale);
            SearchProduct();
        }

        private void SetProductsInSaleList(List<short> saleListIds)
        {
            productsInSaleList = saleListIds;
        }

        private void SearchProduct()
        {
            var productSearchList = productService.GetProductsWithUnitPremise(GlobalAppValues.PremiseId, ProductSearchPattern);
            MapSearchToViewModel(productSearchList);
        }

        private void MapSearchToViewModel(List<ProductBase> productSearchList)
        {
            FilteredProducts = new ObservableCollection<ProductBase>(productSearchList);
            SelectedProduct = FilteredProducts.Count > 0 ? FilteredProducts[0] : null;
        }

        private void UpdateUnitPrice(string unitAbbreviation)
        {
            try
            {
                UnitPrice = productService.GetProductSalePrice(unitAbbreviation);
            }
            catch (Exception ex)
            {
                NotifyStatus(ex.Message);
            }
        }

        private void AddProductToSale()
        {
            if (!ValidateObject())
                return;
            if(!ViewModelCustomValidation())
                return;
            if (SelectedProductIsInSaleList())
                return;
            SendSelectedProductToParent();
            CloseView();
        }

        private bool SelectedProductIsInSaleList()
        {
            if (productsInSaleList.Contains(selectedProduct.ProductBaseId))
            {
                NotifyStatus(Constants.Error12);
                return true;
            }
            return false;
        }

        private void CloseView()
        {
            PropagateMessage("", Constants.Token9);
        }

        private void SendSelectedProductToParent()
        {
            var simpleSaleLine = new SimpleSaleLineViewModel
            {
                ProductBaseId = selectedProduct.ProductBaseId,
                ProductName = selectedProduct.Name,
                Unit = Unit,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                LinePrice = LineOrderPrice
            };

            PropagateMessage(simpleSaleLine, Constants.Token8);
        }

        private bool ViewModelCustomValidation()
        {
            if (!ValidateUnitAbbreviation(unit)) 
            {
                NotifyStatus(Constants.Error11);
                return false;
            }
            return true;
        }

        private void NotifyStatus(string message)
        {
            PropagateMessage(message, Constants.Token11);
        }

        //public ValidationResult IsInRegisteredUnits(object saleListIds, ValidationContext context)
        //{
        //    var productSelectionViewModel = (ProductSelectionViewModel)context.ObjectInstance;

        //    if (!unitService.GetAllUnitsAbbreviation().Any(x => x.Equals(productSelectionViewModel.Unit, StringComparison.CurrentCultureIgnoreCase)))
        //        return new ValidationResult("La unidad ingresada no es válida", new List<string> { "Unit" });
        //    return ValidationResult.Success;
        //}
    }
}
