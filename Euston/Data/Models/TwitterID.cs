using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euston.Data.Models
{
    public class TwitterID
    {
        public TwitterID(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
