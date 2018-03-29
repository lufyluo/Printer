using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Printer.Framework.Printer.ReceiptPrinterESC
{
    /// <summary>

    /// 小票打印类

    /// 使用方法:

    /// 1 GetPrinterList获取已经安装的所有打印机列表.

    ///  Open 打开指定打印机

    /// 2 控制打印机动作、执行打印内容之前，必须先调用StartPrint，准备向打印机发送控制指令

    /// 3 调用SetLeft, SetBold, SetAlignMode, SetFontSize ... ...设置打印参数

    /// 4  PrintText 打印内容.注意：打印该行内容后会自动换行(本类会在该行内容末尾添加一个换行符)

    ///   PrintImageFile 或 PrintBitMap打印图片

    /// 5 控制指令和打印内容都发送完毕后,调用 EndPrint执行真正打印动作

    /// 6 退出程序前调用Close

    /// </summary>

    //public class ReceiptHelper

    //{

    //    #region 指令定义



    //    private static Byte[] Const_Init = new byte[] { 0x1B, 0x40,

    //         0x20, 0x20, 0x20, 0x0A,

    //         0x1B, 0x64,0x10};



    //    //设置左边距

    //    private const string Const_SetLeft = "1D 4C ";





    //    //设置粗体

    //    private const string Const_SetBold = "1B 45 ";

    //    private const String Const_Bold_YES = "01";

    //    private const String Const_Bold_NO = "00";





    //    //设置对齐方式

    //    private const string Const_SetAlign = "1B 61 ";

    //    private const String Const_Align_Left = "30";

    //    private const String Const_Align_Middle = "31";

    //    private const String Const_Align_Right = "32";



    //    //设置字体大小,与 SetBigFont 不能同时使用

    //    private const string Const_SetFontSize = "1D 21 ";



    //    //设置是否大字体,等同于 SetFontSize = 2

    //    //private const String Const_SetBigFontBold = "1B 21 38";

    //    //private const String Const_SetBigFontNotBold = "1B 21 30";

    //    //private const String Const_SetCancelBigFont = "1B 21 00";



    //    /// <summary>

    //    /// 打印并走纸

    //    /// </summary>

    //    private static Byte[] Const_Cmd_Print = new byte[] { 0x1B, 0x4A, 0x00 };

    //    //走纸

    //    private const string Const_FeedForward = "1B 4A ";

    //    private const string Const_FeedBack = "1B 6A ";



    //    //切纸

    //    private static Byte[] Const_SetCut = new byte[] { 0x1D, 0x56, 0x30 };



    //    //查询打印机状态

    //    private static Byte[] Const_QueryID = new byte[] { 0x1D, 0x67, 0x61 };



    //    //回复帧以 ID 开头

    //    private static String Const_ResponseQueryID = "ID";



    //    /// <summary>

    //    /// 设置图标的指令

    //    /// </summary>

    //    private static Byte[] Const_SetImageCommand = new Byte[] { 0x1B, 0x2A, 0x21 };



    //    #endregion



    //    #region 常量定义



    //    /// <summary>

    //    /// 最大字体大小

    //    /// </summary>

    //    public const Int32 Const_MaxFontSize = 8;

    //    /// <summary>

    //    /// 最大走纸距离

    //    /// </summary>

    //    public const Int32 Const_MaxFeedLength = 5000;



    //    /// <summary>

    //    /// 最大高宽

    //    /// </summary>

    //    public const Int32 Const_MaxImageLength = 480;



    //    /// <summary>

    //    /// 每次通信最多打印的行数

    //    /// </summary>

    //    public const Int32 Const_OncePrintRowCount = 24;



    //    public const Int32 Const_BrightnessGate = 100;



    //    /// <summary>

    //    /// 无效句柄

    //    /// </summary>

    //    public const Int32 Const_InvalidHandle = -1;

    //    #endregion



    //    #region 私有成员



    //    /// <summary>

    //    /// 打印机句柄

    //    /// </summary>

    //    private int m_Handle = -1;



    //    /// <summary>

    //    /// 是否已经初始化

    //    /// </summary>

    //    private Boolean m_Inited = false;





    //    #endregion



    //    #region 私有函数



    //    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter,

    //        out Int32 hPrinter, IntPtr pd);



    //    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool StartDocPrinter(Int32 hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);



    //    [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool EndDocPrinter(Int32 hPrinter);



    //    [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool StartPagePrinter(Int32 hPrinter);



    //    [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool EndPagePrinter(Int32 hPrinter);



    //    [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool WritePrinter(Int32 hPrinter, Byte[] pBytes, Int32 dwCount, out Int32 dwWritten);



    //    [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

    //    public static extern bool ClosePrinter(Int32 hPrinter);





    //    /// <summary>

    //    /// 发送指令

    //    /// </summary>

    //    /// <param name="cmd"></param>

    //    /// <returns></returns>

    //    private Boolean SendCommand(Byte[] cmd)

    //    {

    //        if (m_Handle == Const_InvalidHandle || cmd == null || cmd.Length < 2)

    //        {

    //            return false;

    //        }



    //        int writelen = 0;

    //        Boolean bl = WritePrinter(m_Handle, cmd, cmd.Length, out writelen);



    //        if (!bl) return false;

    //        return (writelen >= cmd.Length);

    //    }



    //    /// <summary>

    //    /// 发送文本格式的指令

    //    /// </summary>

    //    /// <param name="cmd"></param>

    //    /// <returns></returns>

    //    private Boolean SendCommand(String hexstrcmd)

    //    {

    //        if (m_Handle == Const_InvalidHandle || hexstrcmd == null || hexstrcmd.Length < 4)

    //        {

    //            return false;

    //        }



    //        byte[] mybyte = null;

    //        Boolean bl = DataFormatProcessor.HexStringToBytes(hexstrcmd, out mybyte);

    //        bl = SendCommand(mybyte);

    //        return bl;

    //    }





    //    #endregion



    //    #region 内部处理 - 打印图片



    //    /// <summary>

    //    /// 把图片转换为指令字节,图片最大高宽不能超过480

    //    /// </summary>

    //    /// <param name="image"></param>

    //    /// <param name="bmpbytes"></param>

    //    /// <returns></returns>

    //    public static Boolean LoadImage(Bitmap image,

    //        ref Byte[] bitarray, ref Int32 datawidth, ref Int32 dataheight)

    //    {

    //        Int32 newwidth = 0;

    //        Int32 newheight = 0;

    //        Bitmap destimage = image;

    //        Boolean bl = false;



    //        //如果高度超过范围,或宽度超过范围,需要进行缩小

    //        if (image.Width > Const_MaxImageLength || image.Height > Const_MaxImageLength)

    //        {

    //            //按照高度和宽度，较大的那一边，进行缩放

    //            if (image.Width > image.Height)

    //            {

    //                newwidth = Const_MaxImageLength;

    //                newheight = (Int32)(image.Height * newwidth / (float)image.Width);

    //            }

    //            else

    //            {

    //                newheight = Const_MaxImageLength;

    //                newwidth = (Int32)(newheight * image.Width / (float)image.Height);

    //            }



    //            bl = ImageProcessor.ResizeImage(image, newwidth, newheight, ref destimage);

    //        }



    //        //把数据转换为字节数组

    //        bl = GetBitArray(image, ref bitarray, ref datawidth, ref dataheight);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 把图片转换为指令字节,图片最大高宽不能超过480

    //    /// 如果图片的高度不是24的整数倍,则修改为24的整数倍

    //    /// </summary>

    //    /// <param name="image"></param>

    //    /// <param name="bmpbytes"></param>

    //    /// <returns></returns>

    //    public static Boolean LoadImageFromFile(String imagefilename, ref Byte[] bmpbytes,

    //        ref Int32 width, ref Int32 height)

    //    {

    //        Bitmap img = ImageProcessor.LoadBitImage(imagefilename);

    //        if (img == null)

    //        {

    //            return false;

    //        }



    //        Boolean bl = LoadImage(img, ref bmpbytes, ref width, ref height);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 把图片转换为位图数组,每个字节的每个比特位,对应当前像素 是否需要打印

    //    /// </summary>

    //    /// <param name="img"></param>

    //    /// <param name="allbitary"></param>

    //    /// <returns></returns>

    //    public static Boolean GetBitArray(Bitmap img,

    //        ref Byte[] allbitary, ref Int32 width, ref Int32 height)

    //    {

    //        if (img == null)

    //        {

    //            return false;

    //        }



    //        //ESC指令格式规定：

    //        //1 打印图片时，每条指令最多只打印24行;不足24行的,也要用全0填充满数据字节

    //        //2 打印24行数据时，按照光栅模式纵向获取数据

    //        //  即先获取所有x=0的点（第0列）转换为3个字节；

    //        //  再获取所有x=1的点转换为3个字节；...直到获取到最右侧一列的点

    //        //3 打印完当前24行数据后，再获取后续24行的数据内容,直到所有的数据获取完毕



    //        //获取亮度数组

    //        Boolean[] briary = null;

    //        Boolean bl = ImageProcessor.ToBooleanArray(img, Const_BrightnessGate, ref briary);

    //        if (!bl)

    //        {

    //            return false;

    //        }



    //        height = img.Height;//如果图像高度不是24整数倍,设置为24的整数倍       

    //        if (height % Const_OncePrintRowCount != 0)

    //        {

    //            height = height + Const_OncePrintRowCount - height % Const_OncePrintRowCount;

    //        }



    //        width = img.Width;//如果图像宽度不是8的整数倍,设置为8的整数倍

    //        if (width % 8 != 0)

    //        {

    //            width = width + 8 - width % 8;

    //        }



    //        Int32 bytelen = height * width / 8;//每个像素对应1个比特位,因此总字节数=像素位数/8



    //        allbitary = new Byte[bytelen];



    //        Int32 byteidxInCol = 0;//当前列里首个像素,在目标字节数组里的下标

    //        Int32 byteidx = 0;//当前像素在目标数组里的字节下标

    //        Int32 bitidx = 0;//当前像素在目标数组里当前字节里的比特位下标

    //        Int32 pixidxInCol = 0;//当前像素在当前列里的第几个位置



    //        Int32 pixidx = 0;//当前像素在原始图片里的下标



    //        Int32 rowidx = 0; //当前 处理的像素点所在行,不能超过 图像高度

    //        Int32 curprocrows = 0;//当前需要处理的行数量

    //        while (rowidx < height)

    //        {

    //            //按照纵向次序,把当前列的24个数据,转换为3个字节

    //            for (Int32 colidx = 0; colidx < img.Width; ++colidx)

    //            {

    //                //如果当前还剩余超过24行没处理,处理24行

    //                if (rowidx + Const_OncePrintRowCount <= img.Height)

    //                {

    //                    curprocrows = Const_OncePrintRowCount;

    //                }

    //                else

    //                {

    //                    //已经不足24行,只处理剩余行数

    //                    curprocrows = img.Height - rowidx;

    //                }



    //                pixidxInCol = 0; //本列里从像素0开始处理

    //                for (Int32 y = rowidx; y < rowidx + curprocrows; ++y)

    //                {

    //                    //原始图片里像素位置

    //                    pixidx = y * img.Width + colidx;



    //                    //获取当前像素的亮度值.如果当前像素是黑点,需要把数组里的对应比特位设置为1

    //                    if (briary[pixidx])

    //                    {

    //                        bitidx = 7 - pixidxInCol % 8;//最高比特位对应首个像素.最低比特位对应末个像素

    //                        byteidx = byteidxInCol + pixidxInCol / 8; //由于最后一段可能不足24行,因此不能使用byteidx++



    //                        DataFormatProcessor.SetBitValue(bitidx, true, ref allbitary[byteidx]);

    //                    }

    //                    pixidxInCol++;

    //                }

    //                byteidxInCol += 3;//每列固定24个像素,3个字节

    //            }



    //            rowidx += Const_OncePrintRowCount;

    //        }



    //        return true;

    //    }



    //    #endregion



    //    #region 公开函数



    //    private static ReceiptHelper m_instance = new ReceiptHelper();



    //    /// <summary>

    //    /// 当前使用的打印机名称

    //    /// </summary>

    //    public String PrinterName

    //    {

    //        get; private set;

    //    }



    //    /// <summary>

    //    /// 单件模式

    //    /// </summary>

    //    /// <returns></returns>

    //    public static ReceiptHelper GetInstance()

    //    {

    //        return m_instance;

    //    }



    //    /// <summary>

    //    /// 获取本机安装的所有打印机

    //    /// </summary>

    //    /// <returns></returns>

    //    public static List<String> GetPrinterList()

    //    {

    //        List<String> ret = new List<String>();

    //        if (PrinterSettings.InstalledPrinters.Count < 1)

    //        {

    //            return ret;

    //        }



    //        foreach (String printername in PrinterSettings.InstalledPrinters)

    //        {

    //            ret.Add(printername);

    //        }

    //        return ret;

    //    }



    //    /// <summary>

    //    /// 打开打印机

    //    /// </summary>

    //    /// <param name="printername"></param>

    //    /// <returns></returns>

    //    public Boolean Open(String printername)

    //    {

    //        if (m_Inited)

    //        {

    //            return true;

    //        }

    //        Boolean bl = OpenPrinter(printername.Normalize(), out m_Handle, IntPtr.Zero);



    //        m_Inited = (bl && m_Handle != 0);

    //        return true;

    //    }



    //    /// <summary>

    //    /// 开始打印，在打印之前必须调用此函数

    //    /// </summary>

    //    /// <returns></returns>

    //    public Boolean StartPrint()

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        DOCINFOA di = new DOCINFOA();

    //        di.pDocName = "My C#.NET RAW Document";

    //        di.pDataType = "RAW";

    //        //Start a document.

    //        Boolean bl = StartDocPrinter(m_Handle, 1, di);

    //        if (!bl)

    //        {

    //            return false;

    //        }

    //        // Start a page.

    //        bl = StartPagePrinter(m_Handle);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 结束打印，在打印结束之后必须调用此函数

    //    /// </summary>

    //    /// <returns></returns>

    //    public Boolean EndPrint()

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        Boolean bl = EndPagePrinter(m_Handle);

    //        bl = EndDocPrinter(m_Handle);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 销毁

    //    /// </summary>

    //    /// <returns></returns>

    //    public Boolean Close()

    //    {

    //        if (!m_Inited)

    //        {

    //            return true;

    //        }

    //        m_Inited = false;



    //        //关闭设备句柄

    //        ClosePrinter(m_Handle);

    //        m_Handle = -1;

    //        return true;

    //    }



    //    /// <summary>

    //    /// 打印文本.在调用本函数之前必须先调用正确的 设置字体、左边距

    //    /// </summary>

    //    /// <param name="content"></param>

    //    /// <returns></returns>

    //    public Boolean PrintText(String content)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }



    //        byte[] bytes = null;

    //        if (content.Length < 1)

    //        {

    //            content = "  ";

    //        }



    //        if (content[content.Length - 1] != (char)0x0D &&

    //            content[content.Length - 1] != (char)0x0A)

    //        {

    //            content = content + (char)0x0A;

    //        }



    //        bytes = DataFormatProcessor.StringToBytes(content);

    //        bool bl = SendCommand(bytes);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 设置对齐方式

    //    /// </summary>

    //    /// <param name="left"></param>

    //    /// <returns></returns>

    //    public bool SetAlignMode(eTextAlignMode alignmode)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }



    //        String code = String.Empty;

    //        switch (alignmode)

    //        {

    //            case eTextAlignMode.Left:

    //                code = Const_Align_Left;

    //                break;

    //            case eTextAlignMode.Middle:

    //                code = Const_Align_Middle;

    //                break;

    //            case eTextAlignMode.Right:

    //                code = Const_Align_Right;

    //                break;

    //            default:

    //                code = Const_Align_Left;

    //                break;

    //        }



    //        //注意：先低字节后高字节

    //        string str = Const_SetAlign + code;

    //        bool bl = SendCommand(str);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 设置左边距

    //    /// </summary>

    //    /// <param name="left"></param>

    //    /// <returns></returns>

    //    public bool SetLeft(int left)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }



    //        //注意：先低字节后高字节

    //        String hexstr = left.ToString("X4");

    //        string str = Const_SetLeft + hexstr.Substring(2, 2) + hexstr.Substring(0, 2);

    //        bool bl = SendCommand(str);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 设置粗体

    //    /// </summary>

    //    /// <param name="bold"></param>

    //    /// <returns></returns>

    //    public Boolean SetBold(Boolean bold)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }



    //        //注意：先低字节后高字节

    //        String str = String.Empty;

    //        if (bold)

    //        {

    //            str = Const_SetBold + Const_Bold_YES;

    //        }

    //        else

    //        {

    //            str = Const_SetBold + Const_Bold_NO;

    //        }

    //        bool bl = SendCommand(str);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 切纸

    //    /// </summary>

    //    /// <returns></returns>

    //    public bool Cut()

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        bool bl = SendCommand(Const_SetCut);

    //        return bl;

    //    }





    //    /// <summary>

    //    /// 打印图片

    //    /// </summary>

    //    /// <param name="bitmap"></param>

    //    /// <returns></returns>

    //    public bool PrintImageFile(String imgfilename)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        Bitmap img = ImageProcessor.LoadBitImage(imgfilename);

    //        if (img == null)

    //        {

    //            return false;

    //        }



    //        Boolean bl = PrintBitmap(img);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 打印图片

    //    /// </summary>

    //    /// <param name="bitmap"></param>

    //    /// <returns></returns>

    //    public bool PrintBitmap(Bitmap bitmap)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }



    //        if (bitmap == null ||

    //            bitmap.Width > Const_MaxImageLength ||

    //            bitmap.Height > Const_MaxImageLength)

    //        {

    //            return false;

    //        }



    //        Byte[] bitary = null;

    //        Int32 width = 0;

    //        Int32 height = 0;

    //        Boolean bl = GetBitArray(bitmap, ref bitary, ref width, ref height);



    //        bl = PrintBitmapBytes(bitary, bitmap.Width, bitmap.Height);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 打印图片

    //    /// </summary>

    //    /// <param name="bitmap"></param>

    //    /// <returns></returns>

    //    public bool PrintBitmapBytes(Byte[] imgbitarray, Int32 width, Int32 height)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        Int32 bytes = width * height / 8;

    //        //检查是否尺寸符合要求

    //        if (width > Const_MaxImageLength || height > Const_MaxFeedLength ||

    //            width < 1 || height < 1 ||

    //            imgbitarray == null)

    //        {

    //            return false;

    //        }



    //        //每次获取24行的数据进行发送,这24行的字节数

    //        Int32 blockbytes = width * Const_OncePrintRowCount / 8;

    //        if (blockbytes < 1)

    //        {

    //            return false;

    //        }



    //        Boolean bl = false;



    //        //一共需要发送的块数量

    //        Int32 blocks = imgbitarray.Length / blockbytes;



    //        //每次发送的数据字节数 = 1B 2A 21 2字节长度 +　数据内容

    //        Byte[] cmdbytes = new Byte[5 + blockbytes];

    //        //指令

    //        Array.Copy(Const_SetImageCommand, cmdbytes, 3);

    //        //数据长度,即 每行的点数

    //        DataFormatProcessor.Int16ToBytes(width, ref cmdbytes, 3);

    //        //数据内容

    //        for (Int32 blockidx = 0; blockidx < blocks; ++blockidx)

    //        {

    //            Array.Copy(imgbitarray, blockidx * blockbytes, cmdbytes, 5, blockbytes);

    //            //发送当前指令

    //            bl = SendCommand(cmdbytes);

    //            if (!bl) return false;

    //            //休眠20毫秒

    //            Thread.Sleep(20);

    //            //发送 打印指令

    //            bl = SendCommand(Const_Cmd_Print);

    //            if (!bl) return false;

    //        }



    //        return bl;

    //    }



    //    /// <summary>

    //    /// 走纸

    //    /// </summary>

    //    /// <param name="length"></param>

    //    /// <returns></returns>

    //    public bool Feed(int length)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        if (length < 1)

    //            length = 1;

    //        if (length > Const_MaxFeedLength)

    //        {

    //            length = Const_MaxFeedLength;

    //        }

    //        string len = length.ToString("X2");

    //        len = Const_FeedForward + len;

    //        bool bl = SendCommand(len);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 回退走纸

    //    /// </summary>

    //    /// <param name="length"></param>

    //    /// <returns></returns>

    //    public bool FeedBack(int length)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }

    //        if (length < 1)

    //            length = 1;

    //        if (length > Const_MaxFeedLength)

    //        {

    //            length = Const_MaxFeedLength;

    //        }

    //        string len = length.ToString("X2");

    //        len = Const_FeedBack + len;

    //        bool bl = SendCommand(len);

    //        return bl;

    //    }



    //    /// <summary>

    //    /// 设置字体大小.本函数不可与SetBigFont同时使用

    //    /// </summary>

    //    /// <param name="sizerate">大小倍率,取值范围 1 - 8</param>

    //    /// <returns></returns>

    //    public bool SetFontSize(Int32 sizerate)

    //    {

    //        if (!m_Inited)

    //        {

    //            return false;

    //        }



    //        if (sizerate < 1)

    //        {

    //            sizerate = 1;

    //        }



    //        if (sizerate > Const_MaxFontSize)

    //        {

    //            sizerate = Const_MaxFontSize;

    //        }

    //        sizerate--;

    //        String sizecodestr = Const_SetFontSize + sizerate.ToString("X1") + sizerate.ToString("X1");

    //        bool bl = SendCommand(sizecodestr);

    //        return bl;

    //    }



    //    #endregion







    //}


}
