﻿using System;
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
        public string Name1Select { get; set; }
        public PrinterConfig()
        {
            InitializeComponent();
        }

        private void Load_Loaded(object sender, RoutedEventArgs e)
        {
            var printers = Finder.GetAllPrinters();
            this.StickPrinter.ItemsSource = printers;
            this.ReceiptPrinter.ItemsSource = printers;
            SetSelected(nameof(StickPrinter), StickPrinter);
            SetSelected(nameof(ReceiptPrinter), ReceiptPrinter);
        }

        private void SetSelected(string key, ComboBox cb)
        {
            foreach (var combox in cb.Items)
            {
                var selector = combox as SelectItem;
                if (selector?.Value == ConfigManager.GetSetting(key))
                {
                    cb.SelectedItem = combox;
                }
            }
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigManager.UpdateSetting(nameof(StickPrinter), StickPrinter.SelectedValue?.ToString() ?? "");
            ConfigManager.UpdateSetting(nameof(ReceiptPrinter), ReceiptPrinter.SelectedValue?.ToString() ?? "");
            this.Visibility = Visibility.Hidden;
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            SetSelected(nameof(StickPrinter), StickPrinter);
            SetSelected(nameof(ReceiptPrinter), ReceiptPrinter);
            this.Visibility = Visibility.Hidden;
        }
    }
}