using Euston.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Euston.Business
{
    [Export(typeof(IProcessMessage))]
    [ExportMetadata("MessageMetaData", "Email")]
    public class EmailProcessMessage : IProcessMessage
    {
        private ObservableCollection<EmailSIR> SIRs = new ObservableCollection<EmailSIR>();
        public ObservableCollection<EmailSIR> ListSIRs
        {
            get { return SIRs; }
        }

        private ObservableCollection<URL> URLs = new ObservableCollection<URL>();
        public ObservableCollection<URL> ListURLs
        {
            get { return URLs; }
        }

        private static EmailProcessMessage instance;
        public static EmailProcessMessage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmailProcessMessage();
                }
                return instance;
            }

        }

        CommonProcesses processes = new CommonProcesses();

        public List<string> ProcessMessage(string messageId, string messageBody)
        {
            List<string> emailBody = new List<string>();
            string[] splitMessage;
            bool isSir = false;

            messageBody = ReplaceURLS(messageBody);
            splitMessage = processes.SplitAndCleanBody(messageBody);
            isSir = CheckIfSir(splitMessage[1]);
            if (isSir)
            {
                CreateSIR(splitMessage);
            }
            foreach (string line in splitMessage)
            {
                emailBody.Add(line);
            }

            SaveMessage(messageId, emailBody, isSir);

            return emailBody;
        }

        public List<string> RunValidation(string messageBody)
        {
            string[] message = processes.SplitAndCleanBody(messageBody);
            string[] sir = { "sir", "Sir", "SIR" };
            bool isSIR = CheckIfSir(message[1]);

            var result = new List<string>();

            if (!processes.Validate(message[0], new Regex(@"(?<email>\w+@\w+\.[a-z]{0,3})")))
            {
                result.Add("Invalid email address");
                return result;
            }

            if (message[1].Length > 20)
            {
                result.Add("Subject too long");
                return result;
            }
            int messageLenght = 0;
            for (int i = 2; i < message.Length; i++)
            {
                messageLenght += message[i].Length;
            }
            if (messageLenght > 1028)
            {
                result.Add("Message too long");
                return result;
            }
            if (isSIR)
            {
                if (!processes.Validate(message[1], new Regex(@"(\d+)[-.\/](\d+)[-.\/](\d+)")))
                {
                    result.Add("Invalid date format");
                    return result;
                }
                if (!processes.Validate(message[2], new Regex(@"^\d{2}-\d{3}-\d{2}")))
                {
                    result.Add("Invalid center code");
                    return result;
                }
                string[] validReports = { "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };
                if (!(validReports.Any(message[3].Contains)))
                {
                    result.Add("Invalid Nature of Incident");
                    return result;
                }
            }
            return result;
        }

        private bool CheckIfSir(string message)
        {
            string[] sir = { "sir", "Sir", "SIR" };
            return sir.Any(message.Contains);

        }

        private string ReplaceURLS(string emailBody)
        {
            Regex regex = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(emailBody);

            foreach (Match urlAddress in matches)
            {
                try
                {
                    Instance.URLs.Add(new URL(urlAddress.Value));
                    emailBody = emailBody.Replace(urlAddress.Value, "<URL Quarantined>");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return emailBody;
        }

        private void CreateSIR(string[] emailBody)
        {
            EmailSIR sirItem = new EmailSIR
            {
                CodeCenter = emailBody[2],
                Incident = emailBody[3]
            };
            Instance.SIRs.Add(sirItem);
        }

        public void SaveMessage(string messageId, List<string> emailBody, bool isSir)
        {
            JSONHandler.WriteEmail(messageId, emailBody, isSir);
        }
    }
}


