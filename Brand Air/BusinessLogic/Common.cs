using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Brand_Air.BusinessLogic
{
    public class Common
    {
        public static readonly string serverUrl= "http://127.0.0.1:8000";
        public static readonly string webUrl = "http://127.0.0.1:80";

        static readonly HttpClient client = new HttpClient();
        static string[] brands = new string[] {"HBL","Pepsi","Cocacola","Tapal Danedar","Oppo","Brighto","Jubilee","Haier","Cool&Cool","UBL" };
        public static int GetBrandId(string BrandName)
        {
            for (int i = 0; i < brands.Length; i++)
            {
                if (brands[i]==BrandName)
                {
                    return i;
                }
            }
            return -1;
        }
        public async static Task authenticate(string username)
        {
           await Request(webUrl+ "/authenticate_user/" + username);
        }
        static async Task Request(string uri)
        {
            try
            {
               var response= await client.GetStringAsync(uri);
                JObject json = JObject.Parse(response);
                ConfigurationManagement.GetInstance.SetConfig(json.SelectToken("id").ToString(), json.SelectToken("name").ToString(),json.SelectToken("brand").ToString());
              

            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                //Console.ReadLine();
            }
        }

    }
}
