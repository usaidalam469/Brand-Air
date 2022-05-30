using Brand_Air.Reports;
using Brand_Air.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brand_Air.BusinessLogic
{
    public class ReportGenerator
    {
        public List<object> MyProperty { get; set; }
        DataTable _logoDT = new DataTable();
        //DataTable _adDT = new DataTable();
        DataTable _keywordDT = new DataTable();
        DataSet _ds = new DataSet();
        ConfigurationManagement userConfig = ConfigurationManagement.GetInstance;
        public ReportGenerator(ReportModel model)
        {
            if (model.LogoResponses!=null)
            {
                SetLogoData(model.LogoResponses);
                SetAdData(model.AdResponses);
            }
            if (model.Keywords!=null)
            {
                SetKeywordData(model.Keywords);
            }
           
            
        }
        public DataSet ReportData { get {
                return _ds;
            } }
        public DataTable LogoDataTable
        {
            get
            {
                return _logoDT;
            }
        }
       
        public DataTable KeywordDataTable
        {
            get
            {
                return _keywordDT;
            }
        }
       
         void SetLogoData(List<string> responses)
        {
            _logoDT.Columns.Add("Label");
            _logoDT.Columns.Add("Time");
            _logoDT.Columns.Add("FPS");
            _logoDT.Columns.Add("Count");
            _logoDT.Columns.Add("CPF");
            string[] responseArray;
            int sec = 25;
            int a = 1;
            for (int i = 0; i < responses.Count; i++)
            {
                responseArray = responses[i].Split('%')[1].Split('/');
                DataRow row;
                row = _logoDT.NewRow();
                if (i == sec)
                {
                    row["Label"] = userConfig.BrandName;
                    row["Time"] = a;
                    row["FPS"] = responseArray[1];
                    row["Count"] = responseArray[2];
                    row["CPF"] = responseArray[3];
                    _logoDT.Rows.Add(row);
                    sec += 25;
                    a++;
                }

            }
            _ds.Tables.Add(_logoDT);
        }

         void SetAdData(string responses)
        {
           
        }
         void SetKeywordData(List<Keyword> responses)
        {
            _keywordDT.Columns.Add("Keyword");
            _keywordDT.Columns.Add("Count");
            for (int i = 0; i < responses.Count; i++)
            {
                DataRow row;
                row = _logoDT.NewRow();
                
                row["Keyword"] = responses[i].keyword;
                row["Count"] = responses[i].count;

                _keywordDT.Rows.Add(row);
               
            }

        }
       
       
    }
}
