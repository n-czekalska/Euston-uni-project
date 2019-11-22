using Euston.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euston.Data.Models
{
    public class Hashtags: PropertyChangedBase
    {
        private string hashtag;
        private int count;

        public string Hashtag
        {
            get { return hashtag; }
            set { SetField(ref hashtag, value, () => Hashtag); }
        }
        public int Count
        {
            get { return count; }
            set { SetField(ref count, value, () => Count); }
        }


        public Hashtags(string hashtag, int count)
        {
            Hashtag = hashtag;
            Count = count;
        }

    }
}
