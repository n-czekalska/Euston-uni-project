using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Euston.Business
{
    [Export(typeof(IProcessMessage))]
    [ExportMetadata("MessageMetaData", "SMS")]
    public class SMSProcessMessage : IProcessMessage
    {
        CommonProcesses processes = new CommonProcesses();

        public List<string> ProcessMessage(string messageId, string messageBody)
        {
            List<string> smsBody = new List<string>();
            string[] splitMessage;


            messageBody = processes.ExpandTextWord(messageBody);
            splitMessage = processes.SplitAndCleanBody(messageBody);

            foreach (string line in splitMessage)
            {
                smsBody.Add(line);
            }

            SaveMessage(messageId, smsBody);
            return smsBody;
        }

        public List<string> RunValidation(string messageBody)
        {
            string[] message = processes.SplitAndCleanBody(messageBody);

            var result = new List<string>();

            if (!processes.Validate(message[0], new Regex(@"(\+|0{2})([0-9]{12})$")))
            {
                result.Add("Invalid phone number");
                return result;
            }

            int messageLenght = 0;
            for (int i = 1; i < message.Length; i++)
            {
                messageLenght += message[i].Length;
            }
            if (messageLenght > 140)
            {
                result.Add("Message too long");
                return result;
            }
            return result;
        }

        public void SaveMessage(string messageId, List<string> smsBody)
        {
            JSONHandler.Write(messageId, smsBody);
        }
    }
}



