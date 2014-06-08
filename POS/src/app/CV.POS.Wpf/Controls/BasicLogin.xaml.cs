using System.Windows;
using System.Windows.Controls;
using CV.POS.Business.Helpers;
using CV.POS.Wpf.Common;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for BasicLogin.xaml
    /// </summary>
    public partial class BasicLogin : UserControl
    {
        public BasicLogin()
        {
            InitializeComponent();

            Messenger.Default.Register<OperationResult>(this,
                Constants.Token1, UpdateView);
        }

        private void UpdateView(OperationResult operationResult)
        {
            if (!operationResult.Succeed)
            {
                UsernameTextBox.Focus();
                MessageBox.Show(operationResult.Message);
            }
        }
    }
}
