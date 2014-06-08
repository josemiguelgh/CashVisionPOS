using System.Windows;
using CV.POS.Business.Helpers;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Messenger.Default.Register<OperationResult>(this,
               Constants.Token3, ShowError);

            Messenger.Default.Register<string>(this,
               Constants.Token5, ShowMessage);
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void ShowError(OperationResult operationResult)
        {
            if (!operationResult.Succeed)
               MessageBox.Show(operationResult.Message);
        }
    }
}