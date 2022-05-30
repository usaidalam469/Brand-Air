using Brand_Air.BusinessLogic;
using Brand_Air.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace Brand_Air.Views
{
    /// <summary>
    /// Interaction logic for Fileupload.xaml
    /// </summary>
    public delegate void updateUI();
    public partial class Fileupload : UserControl
    {
        ConfigurationManagement userConfig;
        static readonly HttpClient client = new HttpClient();
        BrandingManagement brandManager = new BrandingManagement();
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        //System.Threading.Timer timer;
        List<ViewModels.Frame> frames = new List<ViewModels.Frame>();
        List<string> responses = new List<string>();
        string adResponses = "";
        updateUI _updateDel;
        ReportModel reportModel = new ReportModel();
        string path = "";
        //string path = "video2.mp4";
        bool link = false;
        bool flag = true;
        int class_id = 0;
        string videoPath = "";
        public Fileupload()
        {
            userConfig = ConfigurationManagement.GetInstance;
            InitializeComponent();
            //cbClass.Items.Add("HBL");
            //cbClass.Items.Add("Pepsi");
            //cbClass.Items.Add("Cocacola");
            //cbClass.Items.Add("Tapal Danedar");
            //cbClass.Items.Add("Oppo");
            //cbClass.Items.Add("Brighto");
            //cbClass.Items.Add("Jubilee");
            //cbClass.Items.Add("Haier");
            //cbClass.Items.Add("Cool & Cool");
            //cbClass.Items.Add("UBL");
            //cbClass.SelectedIndex = 0;
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = string.Format("{0}Resource\\Player.jpg", System.IO.Path.GetFullPath(System.IO.Path.Combine(RunningPath, @"..\..\")));
            System.Drawing.Image image = System.Drawing.Image.FromFile(FileName);
            image = resizeImage(image, 450, 850);
            pbframe.Image= new Bitmap(image);
            class_id = 0;
            //lblLog.Content = 0;
        }
        public static System.Drawing.Image resizeImage(System.Drawing.Image image, int new_height, int new_width)
        {
            Bitmap new_image = new Bitmap(new_width, new_height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)new_image);
            g.InterpolationMode = InterpolationMode.High;
            g.DrawImage(image, 0, 0, new_width, new_height);
            return new_image;
        }
        //private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    if (ofd.ShowDialog() == true)
        //    {
        //        tbVideoPath.Text = "video4.mp4";
        //        //brandManager.uploadVideo(tbVideoPath.Text);
        //        System.Windows.Forms.MessageBox.Show("Video uploaded");

        //        myTimer.Tick += new EventHandler(TimerEventProcessor);
        //        myTimer.Interval = 500;
        //        myTimer.Start();
        //        //timer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerEventProcessor), null, 1000, 1000);
        //    }
        //}
        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                userConfig = ConfigurationManagement.GetInstance;
                class_id = Common.GetBrandId(userConfig.BrandName);
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    responses = new List<string>();
                    link = false;
                    //tbVideoPath.Text = ofd.FileName;
                    Request(Common.serverUrl + "/api/upload_file/" + userConfig.Id + userConfig.Name);
                    _updateDel = new updateUI(Set);
                    brandManager.uploadFile(ofd.FileName, "video");
                    videoPath = userConfig.Id + userConfig.Name;
                    System.Windows.Forms.MessageBox.Show("Video uploaded");
                    await Request(Common.serverUrl + "/api/stop");
                    brandManager.StopServer();
                    brandManager = new BrandingManagement();
                    Request(Common.serverUrl + "/api/start_video_processing");
                    myTimer.Tick += new EventHandler(TimerEventProcessor);
                    myTimer.Interval = 500;
                    myTimer.Start();
                    //timer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerEventProcessor), null, 1000, 1000);
                }

            }
            catch (Exception)
            {

                System.Windows.Forms.MessageBox.Show("Something went wrong.");
            }
          
        }
        //void TimerEventProcessor(object ob)
        //{
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        BrandingManagement brandManager = BrandingManagement.GetInstance;
        //        var newframe = brandManager.ProcessVideo(tbVideoPath.Text, flag, link, "continue", class_id);
        //        flag = false;
        //        //frames.Add(newframe);
        //        responses.Add(newframe.response);
        //        //Log.frames = frames;
        //        //lblLog.Content = "Logs: " + frames.Count;
        //        if (newframe.frame_detected_image != null)
        //        {
        //            pbframe.Image = newframe.frame_detected_image;
        //        }
        //    });


        //}
        private async void TimerEventProcessor(Object myObject,
                                            EventArgs myEventArgs)
        {
            //BrandingManagement brandManager = BrandingManagement.GetInstance;
            //var newframe = brandManager.ProcessVideo(tbVideoPath.Text, flag, link, "continue", class_id);
            //flag = false;
            ////frames.Add(newframe);
            //responses.Add(newframe.response);
            ////Log.frames = frames;
            ////lblLog.Content = "Logs: " + frames.Count;
            //if (newframe.frame_detected_image != null)
            //{
            //    pbframe.Image = newframe.frame_detected_image;
            //}
            await ProcessingAsync(_updateDel);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Request("http://192.168.100.20:12347");
            try
            {
                //BrandingManagement brandManager = BrandingManagement.GetInstance;
                var newframe = brandManager.ProcessVideo(videoPath, flag, link, "stop", class_id);
                //frames.Add(newframe);
                responses.Add(newframe.response);
                //Log.frames = frames;
                //lblLog.Content = "Logs: " + frames.Count;
                if (newframe.frame_detected_image != null)
                {
                    pbframe.Image = newframe.frame_detected_image;
                }
                flag = true;

                myTimer.Stop();
                GenerateReport();

            }
            catch (Exception)
            {

            }
          
            //timer.Change(Timeout.Infinite, Timeout.Infinite);

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            gdOption.Visibility = Visibility.Visible;
            gdUrl.Visibility = Visibility.Hidden;
        }
        private void btnUrl_Click(object sender, RoutedEventArgs e)
        {
            gdOption.Visibility = Visibility.Hidden;
            gdUrl.Visibility = Visibility.Visible;
        }
        //private void btnProcess_Click(object sender, RoutedEventArgs e)
        //{
        //    if (tbVideoPath.Text!="")
        //    {
        //        path = tbVideoPath.Text;
        //        Uri uriResult;
        //        bool result = Uri.TryCreate(path, UriKind.Absolute, out uriResult)
        //            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        //        if (result)
        //        {
        //            link = true;

        //            myTimer.Tick += new EventHandler(TimerEventProcessor);
        //            myTimer.Interval = 500;
        //            myTimer.Start();
        //            //timer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerEventProcessor), null, 1000, 1000);
        //        }
        //        else
        //        {
        //            System.Windows.Forms.MessageBox.Show("Invalid URL");
        //        }
               
        //    }
            
        //}

        private async void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            if (tbVideoPath.Text != "")
            {
                responses = new List<string>();
                Request(Common.serverUrl + "/api/start_video_processing");
                videoPath = tbVideoPath.Text;
                Uri uriResult;
                bool result = Uri.TryCreate(videoPath, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (result)
                {
                    link = true;
                    _updateDel = new updateUI(Set);
                    myTimer.Tick += new EventHandler(TimerEventProcessor);
                    myTimer.Interval = 500;
                    myTimer.Start();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Invalid URL");
                }

            }

        }
        void Set()
        {
            //BrandingManagement brandManager = BrandingManagement.GetInstance;
            var newframe = brandManager.ProcessVideo(videoPath, flag, link, "continue", class_id);
            flag = false;
            //frames.Add(newframe);
            adResponses=newframe.ad_frame_detected.Split('%')[1];
            responses.Add(newframe.response);
            //Log.frames = frames;
            //lblLog.Content = "Logs: " + frames.Count;
            if (newframe.frame_detected_image != null)
            {
                pbframe.Image = newframe.frame_detected_image;
            }

        }
        async Task ProcessingAsync(updateUI del)
        {
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(del);
            });
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (btnplay.Kind.Equals(MaterialDesignThemes.Wpf.PackIconKind.Play))
            {
                btnplay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                myTimer.Stop();
            }
            else
            {
                btnplay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                myTimer.Start();
            }
           
        }

       
        static async Task Request(string uri)
        {
            try
            {
                await client.GetStringAsync(uri);
            
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                //Console.ReadLine();
            }
        }


        void GenerateReport()
        {
            Request(Common.serverUrl + "/api/stop");
            brandManager.StopServer();
            reportModel.LogoResponses = responses;
            reportModel.AdResponses = adResponses;
            brandManager = new BrandingManagement();
            ReportGenerator rg = new ReportGenerator(reportModel);
            ReportView rv = new ReportView(rg);
            rv.Show();

        }
    }
}
