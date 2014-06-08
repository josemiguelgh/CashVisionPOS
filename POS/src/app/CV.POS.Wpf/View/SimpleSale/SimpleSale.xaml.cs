using System.ComponentModel;
using System.Windows;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.View.Main;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.View.SimpleSale
{
    public sealed partial class SimpleSale : SingleInstanceWindow
    {
        public SimpleSale()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
            Messenger.Default.Register<string>(this,
              Constants.Token7, NotifyStatus);
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ApplicationClosingActions();
        }

        private void ApplicationClosingActions()
        {
            Messenger.Default.Unregister<string>(this, Constants.Token7);
        }
    }
}