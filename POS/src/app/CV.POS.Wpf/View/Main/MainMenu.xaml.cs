using System;
using System.Windows;
using System.Windows.Controls;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.View.CashBox;

namespace CV.POS.Wpf.View.Main
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public sealed partial class MainMenu : UserControl
    {
        private static int RoleId
        {
            get { return Convert.ToInt32(Application.Current.Properties["RoleId"]); }
        }

        public MainMenu()
        {
            InitializeComponent();
            CreateStatusBar();
            CreateMenuOptions();
        }

        private void CreateStatusBar()
        {
            UserName.Text = Application.Current.Properties["FirstName"].ToString();
        }

        private void CreateMenuOptions()
        {
            switch (RoleId)
            {
                case Constants.AdminRoleId:
                    CreateAdminMenu();
                    break;
                case Constants.SalesPersonRoleId:
                    CreateSalesPersonMenu();
                    break;
                case Constants.FullRoleId:
                    CreateFullMenu();
                    break;
                default:
                    MessageBox.Show(Constants.Error4);
                    break;
            }
        }

        private void CreateSalesPersonMenu()
        {
            Menu.Children.Add(CreateMenuButton("Apertura de Caja", BtnCashBoxOpening_Click));
            Menu.Children.Add(CreateMenuButton("Cierre de Caja", BtnCashBoxOpening_Click));
            Menu.Children.Add(CreateMenuButton("Realizar Venta", BtnSimpleSale_Click));
        }

        private void CreateAdminMenu()
        {
            Menu.Children.Add(CreateMenuButton("Apertura de Caja", BtnCashBoxOpening_Click));
            Menu.Children.Add(CreateMenuButton("Cierre de Caja", BtnCashBoxOpening_Click));
            Menu.Children.Add(CreateMenuButton("Realizar Venta", BtnSimpleSale_Click));
            Menu.Children.Add(CreateMenuButton("Mantenimiento de Productos", BtnCashBoxOpening_Click));
        }

        private void CreateFullMenu()
        {
            Menu.Children.Add(CreateMenuButton("Apertura de Caja", BtnCashBoxOpening_Click));
            Menu.Children.Add(CreateMenuButton("Cierre de Caja", BtnCashBoxOpening_Click));
            Menu.Children.Add(CreateMenuButton("Realizar Venta", BtnSimpleSale_Click));
            Menu.Children.Add(CreateMenuButton("Mantenimiento de Productos", BtnCashBoxOpening_Click));
        }

        public Button CreateMenuButton(string content, RoutedEventHandler handler)
        {
            var menuButton = new Button
            {
                Width = 140,
                Height = 90,
                Content = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = content, TextAlignment = TextAlignment.Center }
            };
            menuButton.Click += handler;
            return menuButton;
        }

        private void BtnCashBoxOpening_Click(object sender, RoutedEventArgs e)
        {
            SingleWindowsManager.CreateWindow<CashBoxOpening>();
        }

        private void BtnSimpleSale_Click(object sender, RoutedEventArgs e)
        {
            SingleWindowsManager.CreateWindow<SimpleSale.SimpleSale>();
        }
    }

    public static class SingleWindowsManager
    {
        public static void CreateWindow<T>() where T : SingleInstanceWindow, new()
        {
            var newWindow = new T();
            if (!string.IsNullOrEmpty(newWindow.CreationStatus) && newWindow.CreationStatus == Constants.ShouldBeClosed)
            {
                newWindow.Close();
                return;
            }
            newWindow.Show();
        }
    }
}
