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
    /// Логика взаимодействия для FillUpWindow.xaml
    /// </summary>
    public partial class FillUpWindow : Window
    {
        int clientIndex;
        int accountIndex;
        public FillUpWindow(int clientIndex, int accountIndex)
        {
            InitializeComponent();
            this.clientIndex = clientIndex;
            this.accountIndex = accountIndex;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Repository.Clients[clientIndex].MyAccs.FillUp(Repository.Clients[clientIndex].MyAccs.Accounts[accountIndex], Amount.Text);
                Close();
            } 
            catch (ModuleException exc)
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
