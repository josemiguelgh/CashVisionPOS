using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CV.POS.Business;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.View.SimpleSale
{
    /// <summary>
    /// Description for ProductSelection.
    /// </summary>
    public partial class ProductSelection : SingleInstanceWindow
    {
        /// <summary>
        /// Initializes a new instance of the ProductSelection class.
        /// </summary>
        public ProductSelection()
        {
            InitializeComponent();

            Closing += OnWindowClosing;
            Messenger.Default.Register<string>(this,
               Constants.Token11, NotifyStatus);
            Messenger.Default.Register<string>(this,
                Constants.Token9, CloseView);
        }

        private void CloseView(string obj)
        {
            Close();
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            UnregisterMessengers();
            DeleteViewModel();
        }

        private void DeleteViewModel()
        {
            DataContext = null;
            GC.Collect();
        }

        private void UnregisterMessengers()
        {
            Messenger.Default.Unregister<string>(this, Constants.Token11);
            Messenger.Default.Unregister<string>(this, Constants.Token9);
            Messenger.Default.Unregister<List<short>>(this, Constants.Token10);
        }
    }
}