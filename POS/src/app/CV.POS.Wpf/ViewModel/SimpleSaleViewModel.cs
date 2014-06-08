using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using CV.POS.Business;
using CV.POS.Business.Helpers.EntityHelpers;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.View.Main;
using CV.POS.Wpf.View.SimpleSale;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.ViewModel
{
    
    public sealed class SimpleSaleViewModel : ViewModelCustomBase<SimpleSaleViewModel>
    {
        #region Fields and Properties
        private readonly IGeneralService generalService;

        private readonly ICommand selectProductCommand;
        public ICommand SelectProductCommand
        {
            get { return selectProductCommand; }
        }

        private readonly ICommand performSaleCommand;
        public ICommand PerformSaleCommand
        {
            get { return performSaleCommand; }
        }

        private ObservableCollection<SimpleSaleLineViewModel> saleList; 
        public ObservableCollection<SimpleSaleLineViewModel> SaleList 
        {
            get { return saleList; }
            set
            {
                if (saleList == value)
                    return;
                saleList = value;
                RaisePropertyChanged(vm => vm.SaleList);
                RaisePropertyChanged(vm => vm.TotalSale);
            }
        }

        public decimal TotalSale
        {
            get { return saleList.Aggregate<SimpleSaleLineViewModel, decimal>(0, (current, t) => current + t.LinePrice); }
        }

        private string dniRuc;
        [RegularExpression(Constants.NumericRegEx, ErrorMessage = Constants.Error13)]
        public string DniRuc
        {
            get { return dniRuc; }
            set
            {
                if (dniRuc == value)
                    return;
                dniRuc = value;
                RaisePropertyChanged(vm=>vm.DniRuc);
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;
                name = value;
                RaisePropertyChanged(vm=>vm.Name);
            }
        }

        private string documentType = Constants.TicketType.NoDocument;

        public string DocumentType
        {
            get { return documentType; }
            set
            {
                if (documentType == value)
                    return;
                documentType = value;
                RaisePropertyChanged(vm=>vm.DocumentType);
            }
        }

        public bool NoDocument
        {
            get { return DocumentType == Constants.TicketType.NoDocument; }
            set { DocumentType = value ? Constants.TicketType.NoDocument : DocumentType; }
        }

        public bool Boleta
        {
            get { return DocumentType == Constants.TicketType.Boleta; }
            set { DocumentType = value ? Constants.TicketType.Boleta : DocumentType; }
        }

        public bool Factura
        {
            get { return DocumentType == Constants.TicketType.Factura; }
            set { DocumentType = value ? Constants.TicketType.Factura : DocumentType; }
        }

        #endregion

        public SimpleSaleViewModel(IGeneralService generalService)
        {
            this.generalService = generalService;
            performSaleCommand = new RelayCommand(PerformSale);
            selectProductCommand = new RelayCommand(SelectProduct);

            Messenger.Default.Register<SimpleSaleLineViewModel>(this,
                Constants.Token8, AddProductToSaleList);
            SaleList = new ObservableCollection<SimpleSaleLineViewModel>();
            //SaleList.Add(new SimpleSaleLineViewModel{LinePrice = 2,ProductBaseId = 1,ProductName = "Demo Product",Quantity = 4,Unit = "UND", UnitPrice = 1});
            //SaleList.Add(new SimpleSaleLineViewModel{LinePrice = 2,ProductBaseId = 1,ProductName = "Demo Product",Quantity = 4,Unit = "UND", UnitPrice = 1});
            //SaleList.Add(new SimpleSaleLineViewModel{LinePrice = 2,ProductBaseId = 1,ProductName = "Demo Product",Quantity = 4,Unit = "UND", UnitPrice = 1});
            //SaleList.Add(new SimpleSaleLineViewModel{LinePrice = 2,ProductBaseId = 1,ProductName = "Demo Product",Quantity = 4,Unit = "UND", UnitPrice = 1});
            //SaleList.Add(new SimpleSaleLineViewModel{LinePrice = 2,ProductBaseId = 1,ProductName = "Demo Product",Quantity = 4,Unit = "UND", UnitPrice = 1});
            //SaleList.Add(new SimpleSaleLineViewModel{LinePrice = 2,ProductBaseId = 1,ProductName = "Demo Product",Quantity = 4,Unit = "UND", UnitPrice = 1});

            SaleList.CollectionChanged += OnSaleListCollectionChanged;
        }

        private void SelectProduct()
        {
            SingleWindowsManager.CreateWindow<ProductSelection>();
            Messenger.Default.Send(saleList.Select(x => x.ProductBaseId).ToList(), Constants.Token10);
        }

        private void AddProductToSaleList(SimpleSaleLineViewModel simpleSaleLineViewModel)
        {
            SaleList.Add(simpleSaleLineViewModel);
        }

        private void PerformSale()
        {
            if (!ValidateObject())
                return;
            if (!ViewModelCustomValidation())
                return;

            var saleLineDtos = GetSaleLineDtos();

            try
            {
                var operationResult = generalService.PerformSale(GlobalAppValues.PremiseId, 
                    saleLineDtos, GlobalAppValues.SessionId, DocumentType, Name, dniRuc);

                if (!operationResult.Succeed)
                {
                    NotifyStatus(operationResult.Message);
                }
            }
            catch (Exception ex)
            {
                NotifyStatus(ex.Message);
            }
        }

        private List<SaleLineDto> GetSaleLineDtos()
        {
           return saleList
                .Select(line => new SaleLineDto(line.ProductBaseId, line.ProductName, line.Unit, line.Quantity, line.UnitPrice, line.LinePrice))
                .ToList();
        }

        void OnSaleListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                RaisePropertyChanged(vm => vm.TotalSale);

            if (e.OldItems != null)
                RaisePropertyChanged(vm => vm.TotalSale);
        }

        private bool ViewModelCustomValidation()
        {
            if (!ValidatDocumentType())
            {
                NotifyStatus(Constants.Error14);
                return false;
            }
            return true;
        }

        private bool ValidatDocumentType()
        {
            if (DocumentType == Constants.TicketType.Factura)
            {
                if (String.IsNullOrWhiteSpace(DniRuc) || String.IsNullOrWhiteSpace(Name))
                    return false;
            }
            return true;
        }

        private void NotifyStatus(string message)
        {
            PropagateMessage(message, Constants.Token7);
        }
    }
}