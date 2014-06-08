/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:CV.POS.Wpf.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using CV.POS.Business;
using CV.POS.Business.Interfaces;
using CV.POS.Infrastructure;
using CV.POS.Infrastructure.Helpers;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using CV.POS.Wpf.Model;

namespace CV.POS.Wpf.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<RepositoryFactories>();
            SimpleIoc.Default.Register<IRepositoryProvider, RepositoryProvider>();
            SimpleIoc.Default.Register<IUow, Uow>();
            SimpleIoc.Default.Register<IUserService, UserService>();
            SimpleIoc.Default.Register<ICashBoxService, CashBoxService>();
            SimpleIoc.Default.Register<IProductService, ProductService>();
            SimpleIoc.Default.Register<IUnitService, UnitService>();
            SimpleIoc.Default.Register<IGeneralService, SaleService>();
            SimpleIoc.Default.Register<IUserRepository, UserRepository>();
            SimpleIoc.Default.Register<ICashBoxRepository, CashBoxRepository>();
            SimpleIoc.Default.Register<ICashBoxStatusRepository, CashBoxStatusRepository>();
            SimpleIoc.Default.Register<IProductBaseRepository, ProductBaseRepository>();
            SimpleIoc.Default.Register<IUnitRepository, UnitRepository>();
            SimpleIoc.Default.Register<IProductMovementRepository, ProductMovementRepository>();
            SimpleIoc.Default.Register<IProductPremiseRepository, ProductPremiseRepository>();
            SimpleIoc.Default.Register<ICashMovementRepository, CashMovementRepository>();
            SimpleIoc.Default.Register<ISaleRepository, SaleRepository>();
            SimpleIoc.Default.Register<ISaleDocumentRepository, SaleDocumentRepository>();
            SimpleIoc.Default.Register<IGeneralConfigValuesRepository, GeneralConfigValuesRepository>();
            SimpleIoc.Default.Register<ISaleDetailsRepository, SaleDetailsRepository>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<BasicLoginViewModel>();
            SimpleIoc.Default.Register<CashBoxOpeningViewModel>();
            SimpleIoc.Default.Register<SimpleSaleViewModel>();
            SimpleIoc.Default.Register<ProductSelectionViewModel>();
            
            //SimpleIoc.Default.Register<ISessionRepository, SessionRepository>();

            //SimpleIoc.Default.Register<PosDbContext>();
        }

       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public BasicLoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BasicLoginViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public CashBoxOpeningViewModel CashBoxOpening
        {
            get
            {
                return SimpleIoc.Default.GetInstance<CashBoxOpeningViewModel>(Guid.NewGuid().ToString());

                //return ServiceLocator.Current.GetInstance<CashBoxOpeningViewModel>();

                //if (!SimpleIoc.Default.ContainsCreated<CashBoxOpeningViewModel>())
                //    SimpleIoc.Default.Register<CashBoxOpeningViewModel>();
                //return ServiceLocator.Current.GetInstance<CashBoxOpeningViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SimpleSaleViewModel SimpleSale
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SimpleSaleViewModel>(Guid.NewGuid().ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ProductSelectionViewModel ProductSelection
        {
            get
            {
                //if (!SimpleIoc.Default.ContainsCreated<ProductSelectionViewModel>())
                //    SimpleIoc.Default.Register<ProductSelectionViewModel>();
                return SimpleIoc.Default.GetInstance<ProductSelectionViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            //// TODO Clear the ViewModels     
            //if (SimpleIoc.Default.IsRegistered<CashBoxOpeningViewModel>())
                //SimpleIoc.Default.Unregister<CashBoxOpeningViewModel>();
        }
    }
}