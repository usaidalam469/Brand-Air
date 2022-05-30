using Brand_Air.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brand_Air
{
    public class BrandingManagement
    {
        ClientSocket CSocket;
        static BrandingManagement _instance = null;
        public static BrandingManagement GetInstance { get {
                if (_instance==null)
                {
                    _instance = new BrandingManagement();
                }   
               return _instance;               
            } }
         public BrandingManagement()
        {
            CSocket = new ClientSocket();
        }
        public string uploadFile(string path,string type)
        {
            CSocket.ConnectToServer();
            //Sending file type
            CSocket.SendBytes(Encoding.UTF8.GetBytes(type));
            //CSocket.SendBytes(Encoding.UTF8.GetBytes(type.Length.ToString() + "   " + type));
            using (Stream file = File.OpenRead(path))
            {
                string file_length = file.Length.ToString();
                // Sending file length

                byte[] buffer = new byte[4096];
                string Header = file_length;
                while (Header.Length<10)
                {
                    Header += " ";
                }
                 CSocket.SendBytes(Encoding.UTF8.GetBytes(Header));
                
                while (true)
                {
                    int bytesRead = file.Read(buffer, 0, buffer.Length);
                    if (bytesRead < 4096)
                    {
                        var lastbuffer = buffer.Take(bytesRead).ToArray();
                        CSocket.SendBytes(lastbuffer);
                        break;
                    }
                    CSocket.SendBytes(buffer);

                }
            }
            return CSocket.ReceiveResponse();
            //CSocket.Exit();
        }

       
        public async Task<string> ProcessAudio(string status,bool flag,string filename)
        {
            string response = "None";

            if (CSocket.ConnectToServer())
            {   
                string Header = filename.Length.ToString();
                while (Header.Length < 10)
                {
                    Header += " ";
                }
                if (flag)
                {
                    CSocket.SendBytes(Encoding.UTF8.GetBytes(Header + filename));
                }
                response = Encoding.ASCII.GetString(CSocket.Receive());
                Header = status.Length.ToString();
                while (Header.Length < 10)
                {
                    Header += " ";
                }
                CSocket.SendBytes(Encoding.UTF8.GetBytes(Header+ status));
            }

            return response;
        }
        public Frame ProcessVideo(string path, bool flag, bool link, string status, int class_id)
        {
            string Header;
            if (CSocket.ConnectToServer())
            {
                if (flag)
                {
                    string data = "";
                    if (link)
                    {
                         data = "link-" + path+"-"+class_id;
                        //CSocket.SendBytes(Encoding.UTF8.GetBytes(data.Length.ToString() + "   " + data)); 
                    }
                    else
                    {
                         data = "video-" + path + "-" + class_id;
                        //CSocket.SendBytes(Encoding.UTF8.GetBytes(data.Length.ToString() + "   " + data));
                    }
                     Header = data.Length.ToString();
                    while (Header.Length<10)
                    {
                        Header += " ";
                    }

                    CSocket.SendBytes(Encoding.UTF8.GetBytes(Header+data));
                }

                Frame frame = new Frame();
                var frame_image =  byteToImage(CSocket.Receive());
                if (frame_image != null)
                {
                    frame.frame_image = new Bitmap(frame_image);
                }
                else
                {
                    frame.frame_image = null;
                }
                //var blob = byteToImage(CSocket.Receive()); 
                //if (blob != null)
                //{
                //    frame.Blob = new Bitmap(blob);
                //}
                //else
                //{
                //    frame.Blob = null;
                //}
                frame.response = Encoding.ASCII.GetString(CSocket.Receive()); 
                frame.ad_frame_detected = Encoding.ASCII.GetString(CSocket.Receive()); 
                var frame_detected = byteToImage(CSocket.Receive()); 
                if (frame_detected != null)
                {
                    frame.frame_detected_image = new Bitmap(frame_detected);
                }
                else
                {
                    frame.frame_detected_image = null;
                }
                Header = status.Length.ToString();
                while (Header.Length < 10)
                {
                    Header += " ";
                }
                CSocket.SendBytes(Encoding.UTF8.GetBytes(Header+ status));
                return frame;
            }
            else
            {
                return null;
            }

        }
        public static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
        }
        public System.Drawing.Image byteToImage(byte[] response)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(response))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    return img;
                }
            }
            catch (Exception)
            {

                return null;
            }


        }
        public void StopServer()
        {
            CSocket.Exit();
        }




    }
}
