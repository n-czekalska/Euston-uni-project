using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Euston.Business
{
    public interface IProcessMessage
    {
        List<string> RunValidation(string messageBody);
        List<string> ProcessMessage(string messageId, string messageBody);

    }


}
