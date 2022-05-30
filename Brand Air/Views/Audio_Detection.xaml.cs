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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Net.Http;
using Brand_Air.BusinessLogic;

namespace Brand_Air.Views
{
    /// <summary>
    /// Interaction logic for Audio_Detection.xaml
    /// </summary>
    public partial class Audio_Detection 
    {
        ConfigurationManagement userConfig ;
        List<Keyword> keywords = new List<Keyword>();
        BrandingManagement brandManager =BrandingManagement.GetInstance;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        bool flag = true;
        static readonly HttpClient client = new HttpClient();
        ReportModel reportModel = new ReportModel();

        public Audio_Detection()
        {
            InitializeComponent();
            
        }
        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            userConfig = ConfigurationManagement.GetInstance;
            var a = StaticConfig.brand;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    Request(Common.serverUrl + "/api/upload_file/" + userConfig.Id+userConfig.Name);
                    brandManager.uploadFile(ofd.FileName, "audio");
                    System.Windows.Forms.MessageBox.Show("Audio uploaded");
                    await Request(Common.serverUrl + "/api/stop");
                    brandManager.StopServer();
                    brandManager = new BrandingManagement();
                    Request(Common.serverUrl + "/api/start_audio_processing");
                    myTimer.Tick += new EventHandler(TimerEventProcessor);
                    myTimer.Interval = 500;
                    myTimer.Start();
                    btnReset.IsEnabled = false;
                }
                catch (Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show("Sorry something went wrong!");
                }
                
            }

        }
        private async void TimerEventProcessor(Object myObject,
                                           EventArgs myEventArgs)
        {
            var response = "";
            response = brandManager.ProcessAudio("continue",flag, userConfig.Id + userConfig.Name).Result;
            flag = false;
            TextBlock tb = new TextBlock();
            tb.Text = response;
            tb.Margin = new Thickness(10, 0, 0, 0);
            matchKeyword(response);
            sp.Children.Add(tb);
            refresh();
        }

        private void btnAddkey_Click(object sender, RoutedEventArgs e)
        {
            TextBlock tb = new TextBlock();
            tb.Margin = new Thickness(10, 0, 0, 0);
            
            keywords.Add(new Keyword(tbkeywords.Text));
            tb.Text = tbkeywords.Text;
            TextBlock tb_c = new TextBlock();
            tb_c.Text = "0";
            spKeywordCount.Children.Add(tb_c);
            spKeyword.Children.Add(tb);
            tbkeywords.Text = "";
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            var response = "";
          
            response = brandManager.ProcessAudio("stop", flag, userConfig.Id + userConfig.Name).Result;
            btnReset.IsEnabled = true;
            myTimer.Stop();
            Request(Common.serverUrl + "/api/stop");
            brandManager.StopServer();
            brandManager = new BrandingManagement();
            TextBlock tb = new TextBlock();
            tb.Text = response;
            tb.Margin = new Thickness(10, 0, 0, 0);
            matchKeyword(response);
            sp.Children.Add(tb);
            refresh();
        }
        public void refresh()
        {

            spKeyword.Children.Clear();
            spKeywordCount.Children.Clear();
            for (int i = 0; i < keywords.Count; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Margin = new Thickness(10, 0, 0, 0);
                tb.Text = keywords[i].keyword;
                TextBlock tb_c = new TextBlock();
                tb_c.Text = keywords[i].count.ToString();
                spKeywordCount.Children.Add(tb_c);
                spKeyword.Children.Add(tb);
            }
        }
        private void matchKeyword(string response)
        {
            string[] words = response.Split();
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < keywords.Count; j++)
                {
                    if (words[i].ToLower()==keywords[j].keyword.ToLower())
                    {
                        keywords[j].count += 1;
                        
                    }
                }
              
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //None_res = 0;
            keywords.Clear();
            spKeyword.Children.Clear();
            spKeywordCount.Children.Clear();
            sp.Children.Clear();
        }
        static async Task Request(string uri)
        {
            try
            {
                await client.GetStringAsync(uri);

            }
            catch (HttpRequestException e)
            {
                System.Windows.Forms.MessageBox.Show("Something went wrong!");
            }
        }
        void GenerateReport()
        {
            Request(Common.serverUrl + "/api/stop");
            brandManager.StopServer();
            reportModel.Keywords = keywords;
            brandManager = new BrandingManagement();
            ReportGenerator rg = new ReportGenerator(reportModel);
            ReportView rv = new ReportView(rg);
            rv.Show();

        }
    }
}
