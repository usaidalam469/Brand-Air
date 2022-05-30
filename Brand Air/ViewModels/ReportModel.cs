using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brand_Air.ViewModels
{
    public class ReportModel
    {
        public List<string> LogoResponses { get; set; }
        public string AdResponses { get; set; }
        public List<Keyword> Keywords { get; set; }
    }
}
