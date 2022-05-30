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
using System.Windows.Shapes;

namespace Brand_Air
{
    /// <summary>
    /// Interaction logic for Audio_Detection.xaml
    /// </summary>
    public partial class Audio_Detection : Window
    {
        public Audio_Detection()
        {
            InitializeComponent();

            DataContext = new BaseViewModel();
        }
        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void ButtonFechar_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Audio_Clicked(object sender, RoutedEventArgs e) => DataContext = new Audio_DetectionViewModel();
        
        private void Upload_Video_Clicked(object sender, RoutedEventArgs e) => DataContext = new FileuploadViewModel();
        
        private void System_Logs_Clicked(object sender, RoutedEventArgs e) => DataContext = new Video_DetectionViewModel();
        private void Icon_Clicked(object sender, RoutedEventArgs e) => DataContext = new BaseViewModel();
        
    }
}
