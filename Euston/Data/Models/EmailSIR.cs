using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euston.Data.Models
{
    public class EmailSIR
    {
        public string CodeCenter { get; set; }
        public string Incident { get; set; }

        public EmailSIR(string codeCenter, string incident)
        {
            CodeCenter = codeCenter;
            Incident = incident;
        }

        public EmailSIR()
        {
        }
    }
}
