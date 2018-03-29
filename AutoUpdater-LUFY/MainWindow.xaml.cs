using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using ICSharpCode.SharpZipLib.Zip;

namespace AutoUpdater_LUFY
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑 
    /// </summary>
    public partial class MainWindow : Window
    {
        //更新包地址  
        private string url = "http://localhost/AnswerControl.zip";
        //文件名字  
        private string filename = "";
        //下载文件存放全路径  
        private string filepath = "";
        //更新后打开的程序名  
        string startexe = nameof(TestConsole)+".exe";
        //新版本号  
        string version = "20180327002";
        private BackgroundWorker m_BackgroundWorker;// 申明后台对象
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            m_BackgroundWorker = new BackgroundWorker(); // 实例化后台对象

            m_BackgroundWorker.WorkerReportsProgress = true; // 设置可以通告进度
            m_BackgroundWorker.WorkerSupportsCancellation = true; // 设置可以取消

            m_BackgroundWorker.DoWork += UpdateTask;
            m_BackgroundWorker.RunWorkerCompleted += CompletedWork;

            m_BackgroundWorker.RunWorkerAsync();
          
            
        }

        private void CompletedWork(object sender, RunWorkerCompletedEventArgs e)
        {
            writeLog("更新成功！");
            this.Close();
        }

        private void UpdateTask(object sender, DoWorkEventArgs e)
        {
            filename = url.Substring(url.LastIndexOf("/") + 1);
            //下载文件存放在临时文件夹中  
            filepath = Environment.GetEnvironmentVariable("TEMP") + @"/" + filename;

            if (filename != "")
            {
                try
                {
                    KillExeProcess();
                    SetprogressBar(5);
                    DownloadFile();
                    SetprogressBar(25);
                    UnZipFile();
                    SetprogressBar(10);
                    UpdateVersionInfo();
                    SetprogressBar(10);
                    OpenUpdatedExe();
                    SetprogressBar(10);
                   
                }
                catch (Exception ex)
                {
                    writeLog(ex.Message);
                }

            }
            else
            {
                writeLog("更新失败：下载的文件名为空！");
                return;
            }
        }

        private void KillExeProcess()
        {
            //后缀起始位置  
            int startpos = -1;

            try
            {
                if (startexe != "")
                {
                    if (startexe.EndsWith(".EXE"))
                    {
                        startpos = startexe.IndexOf(".EXE");
                    }
                    else if (startexe.EndsWith(".exe"))
                    {
                        startpos = startexe.IndexOf(".exe");
                    }
                    foreach (Process p in Process.GetProcessesByName(startexe.Remove(startpos)))
                    {
                        p.Kill();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("清杀原程序进程出错：" + ex.Message);
            }
        }

        /// <summary>  
        /// 下载更新包  
        /// </summary>  
        public void DownloadFile()
        {

            WebClient client = new WebClient();
            try
            {
                Uri address = new Uri(url);

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                client.DownloadFile(address, filepath);

            }
            catch (Exception ex)
            {
                throw new Exception("下载更新文件出错：" + ex.Message);
            }

        }


        private void UpdateVersionInfo()
        {
            try
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(startexe);
                cfa.AppSettings.Settings["Version"].Value = version;
                cfa.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("更新版本信息出错：" + ex.Message);
            }

        }

        /// <summary>  
        /// 打开更新后的程序  
        /// </summary>  
        private void OpenUpdatedExe()
        {
            try
            {
                if (ConfigurationManager.AppSettings["StartAfterUpdate"] == "true" && startexe != "")
                {
                    Process openupdatedexe = new Process();
                    openupdatedexe.StartInfo.FileName = startexe;
                    openupdatedexe.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("打开更新后程序出错：" + ex.Message);
            }
        }

        #region 解压zip  
        /// <summary>  
        /// 解压压缩包，格式必须是*.zip,否则不能解压  
        /// </summary>  
        /// <returns></returns>  
        private void UnZipFile()
        {

            try
            {
                using (ZipInputStream zis = new ZipInputStream(File.OpenRead(filepath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = zis.GetNextEntry()) != null)
                    {
                        string directoryName = System.IO.Path.GetDirectoryName(theEntry.Name);
                        string zipfilename = System.IO.Path.GetFileName(theEntry.Name);

                        if (directoryName.Length > 0 && !Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        if (zipfilename != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = zis.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("解压缩更新包出错：" + ex.Message);
            }

        }
        #endregion

        private void writeLog(string str)
        {
            using (StreamWriter errorlog = new StreamWriter(System.IO.Path.Combine(Environment.CurrentDirectory, @"log.txt"), true))
            {
                string strLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + str + "/r/n";
                errorlog.Write(strLog);
                errorlog.Flush();
                //errorlog.Close();
            }
        }
        private delegate void SetprogressBarHandle(int vaule);
        private void SetprogressBar(int vaule)
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new SetprogressBarHandle(this.SetprogressBar), vaule);
            }
            else
            {
                ProgressBar.Value = ProgressBar.Value +vaule;
                Thread.Sleep(1000);
            }
        }
    }
}
