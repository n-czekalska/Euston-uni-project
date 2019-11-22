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

namespace Euston
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display
    {
        public Display(string id, List<string> body)
        {
            InitializeComponent();
            MessageID.Text = "Message ID:  " + id;
            MessageBody.Text = "Sender:  ";
            foreach (var line in body)
            {
                MessageBody.Text += line +"\r\n";
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
