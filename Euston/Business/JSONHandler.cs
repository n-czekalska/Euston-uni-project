using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euston.Business
{
    static class JSONHandler
    {
        public static void WriteEmail(string messageId, List<string> message, bool isSir)
        {
            JObject text;
            string body = "";
            if (isSir)
            {
                foreach (var item in message.Skip(4))
                {
                    body += item;
                }

                text = new JObject(
                     new JProperty("MessageID", messageId),
                     new JProperty("Sender:", message[0]),
                     new JProperty("Subject:", message[1]),
                     new JProperty("Sport Center Code:", message[2]),
                     new JProperty("Nature of Incident:", message[3]),
                     new JProperty("Text:", body));
            }
            else
            {
                foreach (var item in message.Skip(2))
                {
                    body += item;
                }

                text = new JObject(
                     new JProperty("MessageID", messageId),
                     new JProperty("Sender:", message[0]),
                     new JProperty("Subject:", message[1]),
                     new JProperty("Text:", body));
            }

            File.WriteAllText(@"E:\Messages\" + messageId + ".json", text.ToString());

        }

        public static void Write(string messageId, List<string> message)
        {
            string body = "";
            foreach (var item in message.Skip(1))
            {
                body += item;
            }
            JObject text = new JObject(
                new JProperty("MessageID:", messageId),
                     new JProperty("Sender:", message[0]),
                     new JProperty("Text:", body));

            File.WriteAllText(@"E:\Messages\" + messageId + ".json", text.ToString());
        }
    }
}
