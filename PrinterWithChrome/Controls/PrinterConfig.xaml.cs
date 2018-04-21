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
using Printer.Framework.Config;
using Printer.Framework.Printer;
using Printer.Framework.Printer.Model;

namespace PrinterWithChrome.Controls
{
    /// <summary>
    /// PrinterConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PrinterConfig : UserControl
    {
        private List<SelectItem> printers = new List<SelectItem>();
        public PrinterConfig()
        {
            InitializeComponent();
        }

        private void Load_Loaded(object sender, RoutedEventArgs e)
        {
            InitSelections();
        }

        public void InitSelections()
        {
            printers = Finder.GetAllPrinters();
            this.StickPrinter.ItemsSource = printers;
            this.ReceiptPrinter.ItemsSource = printers;
            SetSelected(nameof(StickPrinter), StickPrinter);
            SetSelected(nameof(ReceiptPrinter), ReceiptPrinter);
        }

        private void SetSelected(string key, ComboBox cb)
        {
            cb.SelectedIndex = 0;
            foreach (var combox in cb.Items)
            {
                var selector = combox as SelectItem;
                if (selector?.Id.ToString() == ConfigManager.GetSetting(key)&& selector.Value == ConfigManager.GetSetting(key+"Path"))
                {
                    cb.SelectedItem = combox;
                }
            }
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            var stickPrinterSelectedValue = GetSelectedValue(StickPrinter.SelectedValue?.ToString() ?? "");
            
            ConfigManager.UpdateSetting(nameof(StickPrinter), stickPrinterSelectedValue);

            var receiptPrinterSelectedValue = GetSelectedValue(ReceiptPrinter.SelectedValue?.ToString() ?? "");

            ConfigManager.UpdateSetting(nameof(ReceiptPrinter), receiptPrinterSelectedValue);

            PrinterBase.SetPrinters();

            var stickPrinterPath = GetPrinterUsbPath(stickPrinterSelectedValue);
            ConfigManager.UpdateSetting(nameof(StickPrinter)+"Path", stickPrinterPath);

            
            var receiptPrinterPath = GetPrinterUsbPath(receiptPrinterSelectedValue);
            ConfigManager.UpdateSetting(nameof(ReceiptPrinter) + "Path", receiptPrinterPath);
            this.Visibility = Visibility.Hidden;
        }

        private string GetSelectedValue(string path)
        {
            var printerConfig = printers.FirstOrDefault(n => n.Value == path);
            return printerConfig?.Id.ToString() ?? "1";
        }

        private string GetPrinterUsbPath(string port)
        {
            if (string.IsNullOrEmpty(port)|| port=="-1")
            {
                return "";
            }
            return PrinterBase.printerPaths[int.Parse(port)].Path;
        }
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            SetSelected(nameof(StickPrinter), StickPrinter);
            SetSelected(nameof(ReceiptPrinter), ReceiptPrinter);
            this.Visibility = Visibility.Hidden;
        }
    }
}
