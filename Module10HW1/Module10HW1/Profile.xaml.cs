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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Profile : Window
    {
        IAdmin user = new Manager();
        String defaultValue = "Unknown";
        public Profile()
        {
            InitializeComponent();
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            user.AddNewProfile(GetName(), GetSecondName(), GetPatronymic(), GetPhone(), GetPassport());
            this.Close();
        }
        private string GetName()
        {
            return String.IsNullOrEmpty(FirstNameBox.Text) ? defaultValue : FirstNameBox.Text;
        }
        private string GetSecondName()
        {
            return String.IsNullOrEmpty(SecondNameBox.Text) ? defaultValue : SecondNameBox.Text;
        }
        private string GetPatronymic()
        {
            return String.IsNullOrEmpty(PatronymicBox.Text) ? defaultValue : PatronymicBox.Text;
        }
        private string GetPhone()
        {
            return String.IsNullOrEmpty(PhoneBox.Text) ? defaultValue : PhoneBox.Text;
        }
        private string GetPassport()
        {
            return String.IsNullOrEmpty(PassportBox.Text) ? defaultValue : PassportBox.Text;
        }
    }
}
