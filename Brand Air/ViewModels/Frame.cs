using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brand_Air.ViewModels
{
    public class Frame
    {

        public int id { get; set; }
        public int logo_frame_count { get; set; }
        public Bitmap frame_image { get; set; }
        public Bitmap frame_detected_image { get; set; }
        public string ad_frame_detected { get; set; }
        public Bitmap Blob { get; set; }
        //public List<Logo> Logos { get; set; }
        public string response { get; set; }
        
    }
}
