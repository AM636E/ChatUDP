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

using Client;
using Common;

namespace Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string login = _login.Text;
                string ip = _ip.Text;
                int port = Convert.ToInt32(_port.Text);
                ChatClient client = new ChatClient(login, ip, port);
                new MainWindow(client).Show();
                Close();
               
            }
            catch(FormatException ex)
            {
                MessageBox.Show("Wrong port or ip " + ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to login " + ex.Message);
            }
        }
    }
}
