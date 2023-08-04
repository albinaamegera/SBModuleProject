using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Loading.xaml
    /// </summary>
    public partial class Loading : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? LoadingComplete;
        public Loading()
        {
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            this.Show();
            await Load();
        }
        private async Task Load()
        {
            var random = new Random();
            for (int i = 0; i <= 100; i += random.Next(1, 5))
            {
                progressBar.Value += i;
                PropertyChanged?.Invoke(progressBar.Value, new PropertyChangedEventArgs(nameof(progressBar)));
                await Task.Delay(100);
            }
            LoadingComplete?.Invoke();
            Close();
        }

    }
}
