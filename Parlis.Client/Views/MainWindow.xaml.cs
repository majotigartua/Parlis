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

namespace Parlis.Client.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateMatchButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void JoinMatchButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GoBackPictureMouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        private void ProfilePictureMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void SettingsPictureMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
