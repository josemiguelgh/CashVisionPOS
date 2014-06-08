using System.ComponentModel;
using System.Windows;
using CV.POS.Wpf.Common;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.View.CashBox
{
    /// <summary>
    /// Description for CashBoxOpening.
    /// </summary>
    public sealed partial class CashBoxOpening : SingleInstanceWindow
    {
        public CashBoxOpening()
        {
            InitializeComponent();
            
            Closing += OnWindowClosing;
            Messenger.Default.Register<string>(this,
               Constants.Token6, NotifyStatusAndClose);
        }

        private void ButtonCancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ApplicationClosingActions();
        }

        private void ApplicationClosingActions()
        {
            //ViewModel.ViewModelLocator.Cleanup();
            Messenger.Default.Unregister<string>(this, Constants.Token6);
        }
    }
}