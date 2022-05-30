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
using System.Windows.Forms.Integration;
using Microsoft.Win32;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;

namespace Brand_Air
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string path { get; set; }
        VideoCapture cap;
        Image<Bgr, byte> currentFrame = null;
        public MainWindow()
        {
            InitializeComponent();
            path = System.AppDomain.CurrentDomain.BaseDirectory + "/sound.wav";
        }

        private void btnSelVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog()==true)
                {
                    tbVideoPath.Text = ofd.FileName;
                }
           
        }

        private void btnDetect_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //if (ofd.ShowDialog() == true)
            //{
            //    pbframe.Image = System.Drawing.Image.FromFile(ofd.FileName);

            //    Bitmap bitmap = new Bitmap(pbframe.Image);
            //    Bitmap bitmap1 = BrandingManagement.detectLogos(bitmap);
            //    pbframe.Image = bitmap1;
            //    //bitmap1.Save("Frames/" + frame_no.ToString(), ImageFormat.Jpeg);
            //    //frame_no++;
            //}
        }

        private void btnDetectVideo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cap = new VideoCapture(tbVideoPath.Text);
                cap.ImageGrabbed += Cap_ImageGrabbed;
                cap.Start();
                if (!cap.IsOpened)
                {
                    MessageBox.Show("Ended");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Cap_ImageGrabbed(object sender, EventArgs e)
        {
            //Mat m = new Mat();
            //cap.Retrieve(m, 0);
            //currentFrame = m.ToImage<Bgr, byte>();
            //Bitmap bmp = currentFrame.ToBitmap();
            ////for frames comparing
            //Bitmap bmp1 = currentFrame.ToBitmap();
            ////MessageBox.Show(BrandingManagement.compareFrames(bmp, bmp1));
            ////bmp = BrandAirCV.detectLogos(bmp);
            //bmp = BrandingManagement.detectLogos(bmp);
            //pbframe.Image = bmp;

        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {

            //try
            //{
            //    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //    ffMpeg.ConvertMedia(tbVideoPath.Text.Trim(), path.Trim(), "wav");
            //    byte[] audioBytes = File.ReadAllBytes(path.Trim());
            //    ClientSocket.ConnectToServer();
            //    ClientSocket.SendBytes(audioBytes);
            //    string response = ClientSocket.ReceiveResponse();
            //    //int occurences = BoyerMooreAlgorithm.search(response, tbWord.Text);
            //    MessageBox.Show(response);
            //}
            //catch (Exception)
            //{

            //    MessageBox.Show("Invalid video path");
            //}

        }
    }
}
