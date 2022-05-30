using Brand_Air.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Frame = Brand_Air.ViewModels.Frame;

namespace Brand_Air.Views
{
    /// <summary>
    /// Interaction logic for Video_Detection.xaml
    /// </summary>
    public partial class Video_Detection : UserControl
    {
        List<Frame> logs = new List<Frame>();
        public Video_Detection()
        {
            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#17b489");
            var Fill = brush;

            InitializeComponent();
             logs = Log.frames;
            

            if (logs!=null)
            {
                for (int i = 0; i < logs.Count; i++)
                {

                    Button bt = new Button();
                    bt.Content = "Frame-" + i;
                    bt.Background = Fill;
                    bt.BorderBrush = Fill;
                    bt.BorderThickness = new Thickness(5, 10, 15, 20);
                    bt.Click += Bt_Click;
                    sp.Children.Add(bt);
                }
            }



        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var Frame = logs[int.Parse(btn.Content.ToString().Split('-')[1].ToString())];
            pbDetected.Image = Frame.frame_detected_image;
            pbFrame.Image = Frame.frame_image;
            tbResponse.Text = Frame.response;
            pbBlob.Image = Frame.Blob;
            pb_video_frame.Text = Frame.ad_frame_detected;
            //pb_ad_Detected.Image = Frame.ad_frame_detected;
            
        }
    }
}
