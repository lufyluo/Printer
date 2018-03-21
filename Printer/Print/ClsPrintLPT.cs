using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Printer.Print
{
    class ClsPrintLPT
    {
        private IntPtr iHandle;
        private FileStream fs;
        private StreamWriter sw;

        private string prnPort = ConfigurationSettings.AppSettings["PrinterConnection"];   //打印机端口

        public ClsPrintLPT()
        {

        }

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const int OPEN_EXISTING = 3;

        /// <summary>
        /// 打开一个vxd(设备)
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, int lpSecurityAttributes,
                                                int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

        /// <summary>
        /// 开始连接打印机
        /// </summary>
        private bool PrintOpen()
        {
            iHandle = CreateFile(prnPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle.ToInt32() == -1)
            {
                MessageBox.Show("没有连接打印机或者打印机端口不是LPT1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                fs = new FileStream(iHandle, FileAccess.ReadWrite);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);   //写数据
                return true;
            }
        }

        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="str">要打印的字符串</param>
        private void PrintLine(string str)
        {
            sw.WriteLine(str); ;
        }

        /// <summary>
        /// 关闭打印连接
        /// </summary>
        private void PrintEnd()
        {
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 打印票据
        /// </summary>
        /// <param name="ds">tb_Temp 全部字段数据集合</param>
        /// <returns>true：打印成功 false：打印失败</returns>
        public bool PrintDataSet(DataSet dsPrint)
        {
            try
            {
                if (PrintOpen())
                {
                    PrintLine(" ");
                    PrintLine("[XXXXXXXXXXXXXXXXXX超市]");
                    PrintLine("NO :      " + dsPrint.Tables[0].Rows[0][1].ToString());
                    PrintLine("XXXXXX: " + dsPrint.Tables[0].Rows[0][2].ToString());
                    PrintLine("XXXXXX: " + dsPrint.Tables[0].Rows[0][3].ToString());
                    PrintLine("XXXXXX: " + dsPrint.Tables[0].Rows[0][4].ToString());
                    PrintLine("XXXXXX: " + dsPrint.Tables[0].Rows[0][5].ToString());
                    PrintLine("操 作 员: " + dsPrint.Tables[0].Rows[0][6].ToString() + " " + dsPrint.Tables[0].Rows[0][7].ToString());
                    PrintLine("-------------------------------------------");
                }
                PrintEnd();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ESC/P 指令
        /// </summary>
        /// <param name="iSelect">0：退纸命令 1：进纸命令 2：换行命令</param>
        public void PrintESC(int iSelect)
        {
            string send;

            iHandle = CreateFile(prnPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle.ToInt32() == -1)
            {
                MessageBox.Show("没有连接打印机或者打印机端口不是LPT1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                fs = new FileStream(iHandle, FileAccess.ReadWrite);
            }

            byte[] buf = new byte[80];

            switch (iSelect)
            {
                case 0:
                    send = "" + (char)(27) + (char)(64) + (char)(27) + 'j' + (char)(255);    //退纸1 255 为半张纸长
                    send = send + (char)(27) + 'j' + (char)(125);    //退纸2
                    break;
                case 1:
                    send = "" + (char)(27) + (char)(64) + (char)(27) + 'J' + (char)(255);    //进纸
                    break;
                case 2:
                    send = "" + (char)(27) + (char)(64) + (char)(12);   //换行
                    break;
                default:
                    send = "" + (char)(27) + (char)(64) + (char)(12);   //换行
                    break;
            }

            for (int i = 0; i < send.Length; i++)
            {
                buf[i] = (byte)send[i];
            }

            fs.Write(buf, 0, buf.Length);
            fs.Close();
        }
    }
}
