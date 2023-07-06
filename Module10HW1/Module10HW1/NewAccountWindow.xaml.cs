using ModuleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Module10HW1
{
    /// <summary>
    /// Логика взаимодействия для NewAccountWindow.xaml
    /// </summary>
    public partial class NewAccountWindow : Window
    {
        Client currentClient;
        bool isVisiable = false;
        public NewAccountWindow(Client client)
        {
            InitializeComponent();
            currentClient = client;
            SetComboBox();
        }

        private void AccTypeSwitcher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isVisiable = !isVisiable;
            rate.Visibility = isVisiable ? Visibility.Hidden : Visibility.Visible;
        }
        private void SetComboBox()
        {
            AccTypeSwitcher.ItemsSource = new string[] { "Basic", "Deposit" };
            AccTypeSwitcher.SelectedIndex = 0;
        }

        private void canselButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            Type type = AccTypeSwitcher.SelectedIndex == 0 ? typeof(BasicAccount) : typeof(DepositAccount);
            string result = startAmount.Text;
            string rateResult = rate.Text == "Enter Interested Rate"? "0": rate.Text;
            try
            {
                currentClient.CreateAccount(type, result, rateResult);
                Close();

            }
            catch (ModuleException exc)
            {
                MessageBox.Show(exc.Message,
                                "error !",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
