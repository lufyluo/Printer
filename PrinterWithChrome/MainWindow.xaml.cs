﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;
using Printer.Core.Printer.ServiceTickPrinter;
using Printer.Framework.Config;
using Printer.Framework.Log;
using Printer.Framework.VersionCheck;
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
            try
            {
                Logger.Info("start");
                if (!Checker.IsNeedUpdate())
                {
                    InitializeComponent();
                }
                else
                {
                    OpenUpdatedExe();
                }
                Logger.Info("end");
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
            }
           
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitControls();
        }

        private void InitControls()
        {
            try
            {
                wb = new ChromiumWebBrowser
                {
                    Address = ConfigManager.GetSetting("Source")
                };
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                wb.RegisterJsObject("transBound", new TransportReceiptPrinter());
                wb.RegisterJsObject("receiptBound", new ReceiptPrinter());
                wb.RegisterJsObject("logisticsBound", new LogisticsPrinter());
                wb.RegisterJsObject("qrBound", new QrPrinter());
                WBGrid.Children.Add(wb);
                config = new PrinterConfig();
                config.Width = 400;
                config.Height = 300;
                WBGrid.Children.Add(config);
                config.Visibility = Visibility.Hidden;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
            }
            
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == KeyStates.Down && e.Key == Key.P)
            {
                config.InitSelections();
                config.Visibility = Visibility.Visible;
            }
        }

        private void OpenUpdatedExe()
        {
            try
            {
                Process openupdatedexe = new Process();
                openupdatedexe.StartInfo.FileName = "AutoUpdater-LUFY.exe";
                if(openupdatedexe.Start())
                    this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("打开更新后程序出错：" + ex.Message);
            }
        }
    }
}
