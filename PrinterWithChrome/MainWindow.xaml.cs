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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;
using Printer.Framework.Config;
using Printer.Framework.Printer.QrPrinter;
using Printer.Framework.Printer.ServiceTickPrinter;
using PrinterWithChrome.Controls;

namespace PrinterWithChrome
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChromiumWebBrowser wb;
        private PrinterConfig config;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitControls();
           
        }

        private void InitControls()
        {
            wb = new ChromiumWebBrowser
            {
                Address = ConfigManager.GetSetting("Source")
            };
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            wb.RegisterJsObject("bound", new PrinterServiceTick());
            wb.RegisterJsObject("bound1", new PrinterQr());
            WBGrid.Children.Add(wb);
            config = new PrinterConfig();
            config.Width = 400;
            config.Height = 300;
            WBGrid.Children.Add(config);
            config.Visibility = Visibility.Hidden;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == KeyStates.Down && e.Key == Key.P)
            {
                config.Visibility = Visibility.Visible;
            }
        }
    }
}
