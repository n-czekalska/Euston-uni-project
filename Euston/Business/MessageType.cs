using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euston.Business
{
    public class MessageType
    {
        public Type type { get; private set; }


        public Type GetMessageType(string messageId)
        {

            if ((messageId.Contains("S")|| messageId.Contains("s")) && messageId.Length == 10)
            {
                return Type.SMS;
            }
            else if ((messageId.Contains("E") || messageId.Contains("e")) && messageId.Length == 10)
            {
                return Type.Email;
            }
            else if ((messageId.Contains("T") || messageId.Contains("t")) && messageId.Length == 10)
            {
                return Type.Tweet;
            }
            else
            {
                throw new Exception("Invalid ID!");
            }
        }

        public enum Type
        {
            SMS,
            Email,
            Tweet,
        }
    }
}

