using Brand_Air.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Brand_Air.BusinessLogic
{
    class ConfigurationManagement
    {
        bool _isConfigured = false;
        bool _logoDetection = false;
        bool _adDetection = false;
        bool _keywordDetection = false;
        string _name = "";
        string _brandName = "";
        string _id = "";

        public string Name { get {
                return _name;
            } }
        public string BrandName { get {
                return _brandName;
            } }
        public string Id
        {
            get
            {
                return _id;
            }
        }
        static ConfigurationManagement _instance = null;
        ConfigurationManagement()
        {
        
        }
        ConfigurationManagement(string name,string brand)
        {
            _name = name;
            _brandName = brand;
            _isConfigured = true;

        }
        public bool IsConfigured { get { return _isConfigured; } }
        
        static public ConfigurationManagement GetInstance { get {
                if (_instance==null)
                {
                    _instance = new ConfigurationManagement();
                }
                return _instance;
            } }

        public void SetConfig(string id,string name, string brand)
        {
            if (_instance == null)
            {
                _instance = new ConfigurationManagement();
            }
            StaticConfig.id = id;
            StaticConfig.name = name;
            StaticConfig.brand = brand;
            _instance._id = id;
            _instance._name = name;
            _instance._brandName = brand;
            _isConfigured = true;
        }


    }
}
