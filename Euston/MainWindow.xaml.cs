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
using Euston.Business;
using System.Collections.ObjectModel;
using Euston.Data.Models;
using MahApps.Metro.Controls;
using System.IO;
using Microsoft.Win32;

namespace Euston
{

    public partial class MainWindow
    {
        public ObservableCollection<URL> UrlList
        {
            get { return EmailProcessMessage.Instance.ListURLs; }
        }

        public ObservableCollection<EmailSIR> SirList
        {
            get { return EmailProcessMessage.Instance.ListSIRs; }
        }

        public ObservableCollection<TwitterID> MentionsList
        {
            get { return TweetProcessMessage.Instance.ListIDs; }
        }

        public ObservableCollection<Hashtags> TrendingList
        {
            get { return TweetProcessMessage.Instance.ListHashtags; }
        }

        MessageType messageType = new MessageType();
        ProcessMessageHelper helper;

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            MessageBody.AcceptsReturn = true;
        }

        private void ProcessMessage_Click(object sender, RoutedEventArgs e)
        {
            String Id;

            if (String.IsNullOrWhiteSpace(MessageId.Text.ToString()))
            {
                MessageBox.Show("Message ID has to be provided");
            }
            else
            {
                Id = MessageId.Text;
                MessageType.Type type;
                    try
                    {
                        type = messageType.GetMessageType(Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                Process(type);

            }


        }

        private void Process(MessageType.Type type)
        {
            helper = new ProcessMessageHelper();
            helper.AssembleComponents();
            List<string> result = helper.Execute(MessageId.Text, MessageBody.Text, type);
            if (result.Any())
            {
              Display display = new Display(MessageId.Text, result);
              display.Show();
            }
            
        }

        private void ImportMessage_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (var reader = new StreamReader(openFileDialog.FileName))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                string[] message = line.Split(',');
                                MessageId.Text = message[0];
                                foreach (var item in message.Skip(1))
                                {
                                    MessageBody.Text += item +"\r\n";
                                }
                                MessageType.Type type;
                                type = messageType.GetMessageType(MessageId.Text);
                                Process(type);
                                MessageId.Text = "";
                                MessageBody.Text = "";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("could not read the file you chose");
                }
            }
        }

        private void ClearInput_Click(object sender, RoutedEventArgs e)
        {
            MessageId.Text = "";
            MessageBody.Text = "";
        }

        
    }
}
