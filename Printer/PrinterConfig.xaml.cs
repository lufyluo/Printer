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
using Printer.Print;

namespace Printer
{
    /// <summary>
    /// PrinterConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PrinterConfig : UserControl
    {
        public IList<SelectItem> Printers { get; set; }
        public PrinterConfig()
        {
            InitializeComponent();
            InitPrinters();
        }

        private void InitPrinters()
        {
            Printers= PrinterFinder.GetAllPrinters();
            this.name1.ItemsSource = Printers;
        }

        private void PrintTest(string lp)
        {
            new TransportTickPrint().Print();
        }

        private void name1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrintTest(this.name1.SelectedValue.ToString());
        }
    }
}
