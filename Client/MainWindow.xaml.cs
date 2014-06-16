using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Common;
namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatClient _client;
        ObservableCollection<Data> _messages = new ObservableCollection<Data>();

        public ObservableCollection<Data> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public MainWindow(ChatClient client)
            : this()
        {
            _client = client;

            _client.Received += _client_Received;
        }

        void _client_Received(object o, ChatEventArgs e)
        {
            this.Dispatcher.Invoke(() => {_messages.Add(e.received); });
            
        //    MessageBox.Show(e.received.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _client.SendMessage(_message.Text);
                _message.Text = "";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
