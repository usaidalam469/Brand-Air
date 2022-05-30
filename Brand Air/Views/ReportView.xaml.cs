using Brand_Air.BusinessLogic;
using Brand_Air.Reports;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Brand_Air.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Window
    {
        ConfigurationManagement userConfig = ConfigurationManagement.GetInstance;
        public DataSet _ds = null;
        public DataTable _logodDt = null;
        public DataTable _keywordDt = null;
        public ReportView(DataSet ds) 
        {
            InitializeComponent();
            _ds = ds;
           
        }
        public ReportView(ReportGenerator rg)
        {
            InitializeComponent();
            if (rg.LogoDataTable!=null)
            {
                _logodDt = rg.LogoDataTable;
            }
            if (rg.KeywordDataTable!=null)
            {
                _keywordDt = rg.KeywordDataTable;
            }

        }


        private void reportViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            crReport cr = new crReport();
            cr.Database.Tables["LogoDataTable"].SetDataSource(_logodDt);
            cr.SetParameterValue("pTotalLogoCount", _logodDt.Rows.Count>0? _logodDt.Rows[_logodDt.Rows.Count-1]["Count"].ToString():"0");
            cr.SetParameterValue("pTotalAdCount","0");
            cr.SetParameterValue("pTotalKeywordCount", "0");
            cr.SetParameterValue("pName", userConfig.Name);
            cr.SetParameterValue("pBrandName", userConfig.BrandName);
            cr.SetParameterValue("pEmail", "usaidalam469@gmail.com");
            reportViewer.ViewerCore.ReportSource = cr;
        }
    }
}
