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
using ModuleLibrary;

namespace Module10HW1
{
    /// <summary>
    /// Логика взаимодействия для TransactToWindow.xaml
    /// </summary>
    public partial class TransactToWindow : Window
    {
        private List<Client> clients;
        private MyAccounts currentAccount;
        public TransactToWindow(List<Client> clients, MyAccounts currentAccount)
        {
            InitializeComponent();
            this.clients = clients;
            this.currentAccount = currentAccount;
            CheckClients();
            SetComboBox();
        }
        private void SetComboBox()
        {
            Clients.ItemsSource = clients;
            Clients.SelectedIndex = 0;
        }
        private void CheckClients()
        {
            if (this.clients.Count == 0)
            {
                MessageBox.Show("another clients have no account to transact to",
                                "error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                this.Close();
            }
            else
            {
                Show();
            }
        }
        private void TransactButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentAccount.TransactTo(clients[Clients.SelectedIndex].MyAccs, TransactionAmount.Text);
                Close();

            } 
            catch(ModuleException exc)
            {
                MessageBox.Show(exc.Message,
                            "warning",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
