using System;
using System.Collections.Generic;
using System.Configuration;
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
using Printer.Print;

namespace Printer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public PrinterConfig pcConfig = new PrinterConfig();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.Width = this.ActualWidth;
            this.WebBrowser.Height = this.ActualHeight;
            this.WebBrowser.Source = new Uri(ConfigurationSettings.AppSettings["Source"]);
            this.WebBrowser.ObjectForScripting = new HandlerForScriptCalling(this);
            this.Grid.Children.Add(pcConfig);
            pcConfig.HorizontalAlignment = HorizontalAlignment.Center;
            pcConfig.VerticalAlignment = VerticalAlignment.Center;
        }

        private void Na_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == KeyStates.Down && e.Key == Key.P)
            {
                var c =this.Grid.Children;
                this.Grid.Children.Remove(WebBrowser);
            }
        }
    }
}
