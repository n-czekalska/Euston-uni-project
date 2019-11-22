using Euston.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Euston.Business
{
    public class CommonProcesses
    {
        public string[] SplitAndCleanBody(string emailBody)
        {
            string[] splitBody = Regex.Split(emailBody, "\r\n");
            splitBody = splitBody.Where(val => val.Length > 0).ToArray();
            return splitBody;
        }


        public bool Validate(string text, Regex regex)
        {
            if (regex.IsMatch(text))
            {
                return true;
            }
            return false;
        }

        public string ExpandTextWord(string message)
        {
            Dictionary<string, string> textwords = GetTextwords();

            string[] m = Regex.Split(message,"\r\n|\\s");
            foreach (var textword in textwords)
            {
                // if used Contains without splitting sometimes it would mistake abbrev. example B4N <- will assume B4
                foreach (var word in m)
                {
                    if (word == textword.Key)
                    {
                        message = message.Replace(textword.Key, textword.Key + "<" + textword.Value + ">");
                        Console.WriteLine(message);
                        continue;
                    }
                }
            }

            return message;
        }



        private Dictionary<string, string> GetTextwords()
        {
            Dictionary<string, string> textwords = new Dictionary<string, string>();

            var csvTextwords = Resources.textwords;
            string[] abbrs = Regex.Split(csvTextwords, "\r\n");

            foreach (string abb in abbrs)
            {
                int index = abb.IndexOf(',');

                string abbreviation = abb.Substring(0, index);
                string fullForm = abb.Substring(index + 1).TrimEnd();

                textwords.Add(abbreviation, fullForm);
            }
            return textwords;
        }
    }
}
