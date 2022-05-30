using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brand_Air.ViewModels
{
    public class Keyword
    {
        public string keyword { get; set; }
        public int count { get; set; }
        public Keyword(string key)
        {
            this.keyword = key;
            this.count = 0;
        }
    }

}
