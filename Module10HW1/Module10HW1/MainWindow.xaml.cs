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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using ModuleLibrary;
using System.Windows.Controls.Primitives;

namespace Module10HW1
{
    public partial class MainWindow : Window
    {
        public IWorker User;
        EditField field;
        PopupController? ppHolder;
        
        public MainWindow(IWorker user)
        {
            InitializeComponent();
            User = user;
            SetHolder();
            SetHeader();
            SetLogs();
            SetEditingProperties();
            Repository.Save();
        }
        #region Methods
        private void SetLogs()
        {
            ClientList.ItemsSource = Repository.Clients;
            OperationsLog.ItemsSource = Repository.Operations;
            if (User.GetType() == typeof(Consultant))
            {
                PassportInfo.DisplayMemberBinding = new Binding("HiddenPassport");
            } 
            else
            {
                PassportInfo.DisplayMemberBinding = new Binding("Passport");
            }
            SetAccountList();
        }
        private void SetEditingProperties()
        {
            if (User.GetType() == typeof(Consultant))
            {
                EditingValue.ItemsSource = new String[]
                {
                    "Phone Number",
                };
            }
            else
            {
                EditingValue.ItemsSource = new String[]
                {
                    "Name",
                    "Second Name",
                    "Patronymic",
                    "Phone Number",
                    "Passport",
                };
                AddNewButton.Visibility = Visibility.Visible;
                CreateAccountBatton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
            }
        }
        private void SetHeader()
        {
            Session.Text = $"{User} session";
        }
        private void SetHolder()
        {
            ppHolder = new(User);
            ppHolder.ShowPopup += ShowPopup;
        }
        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetAccountList();
        }
        private void SetAccountList()
        {
            AccountList.ItemsSource = Repository.Clients[ClientList.SelectedIndex].MyAccs.Accounts;
            AccountList.SelectedIndex = 0;
        }
        private void ShowPopup(string msg, DateTime time)
        {
            var popup = new Popup();
            var textBlock = new TextBlock();
            textBlock.Text = $"{msg}\n{time.ToString("g")}";
            textBlock.Background = Brushes.LightGray;
            textBlock.Foreground = Brushes.Black;
            popup.Child = textBlock;
            popup.PlacementTarget = Tab;
            popup.MinWidth = Tab.RenderSize.Width;
            popup.MinHeight = 50;
            popup.Placement = PlacementMode.Bottom;
            popup.VerticalOffset = -50;
            popup.StaysOpen = false;
            popup.AllowsTransparency = true;
            popup.PopupAnimation = PopupAnimation.Slide;
            popup.IsOpen = true;
        }
        #endregion

        #region Buttons Click Events
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NewValue.Text))
            {
                MessageBox.Show("enter new value !",
                    "Empty line",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            switch(EditingValue.SelectedValue)
            {
                case "Name": field = EditField.Name; break;
                case "Second Name": field = EditField.SecondName; break;
                case "Patronymic": field = EditField.Patronymic; break;
                case "Phone Number": field = EditField.Phone; break;
                case "Passport": field = EditField.Passport; break;
                default: break;
            }
            User.EditProfile(Repository.Clients[ClientList.SelectedIndex], field, NewValue.Text);
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            Profile profileWindow = new Profile();
            profileWindow.Show();
        }
        private void CreateAccountBatton_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow window = new NewAccountWindow(Repository.Clients[ClientList.SelectedIndex]);
            window.Show();
        }
        private void TransactButton_Click(object sender, RoutedEventArgs e)
        {
            if(AccountList.SelectedItem == null || !Repository.Clients[ClientList.SelectedIndex].HasAccount<BasicAccount>())
            {
                MessageBox.Show("open basic account first !",
                                "warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            var account = Repository.Clients[ClientList.SelectedIndex].MyAccs;
            var clients = Repository.GetList();
            clients.RemoveAt(ClientList.SelectedIndex);
            var window = new TransactToWindow(clients.FindAll(e => e.HasAccount<BasicAccount>()), account);
        }
        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountList.SelectedItem == null)
            {
                MessageBox.Show("no account to fill up ...",
                                "warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            } 
            else
            {
                var window = new FillUpWindow(ClientList.SelectedIndex, AccountList.SelectedIndex);
                window.Show();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if(AccountList.SelectedItem == null)
            {
                return;
            }
            Repository.Clients[ClientList.SelectedIndex].MyAccs.Close(AccountList.SelectedIndex);
        }
        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            var startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ListView Sorting Click Events
        private void NameColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.Name);
        }
        private void SecondNameColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.SecondName);
        }
        private void PatronymicColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.Patronymic);
        }
        private void PhoneColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.PhoneNumber);
        }
        private void PassportColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.Passport);
        }
        private void LCColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.LastChange);
        }
        private void TypeColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.ChangeType);
        }
        private void FieldColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.ChangedField);
        }
        private void ChangerColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            Repository.Sort(SortBy.ChangeBy);
        }




        #endregion

        
    }
}
