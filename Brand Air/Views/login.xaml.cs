using Brand_Air.BusinessLogic;
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
using System.Windows.Shapes;


namespace Brand_Air.Views
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }
      
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            btnLogin.IsEnabled = false;
            Brand_Air.Audio_Detection ad = new Brand_Air.Audio_Detection();
            await Common.authenticate(tbUsername.Text);
            
            if ( ConfigurationManagement.GetInstance.IsConfigured)
            {
                ad.Show();
                this.Hide();
            }
            else
            {
                btnLogin.IsEnabled = true;
                System.Windows.Forms.MessageBox.Show("Invlaid username or password");
            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
