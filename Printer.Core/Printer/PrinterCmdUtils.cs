using System;
using System.Drawing;
using System.Text;

namespace Printer.Core.Printer
{
    public class PrinterCmdUtils
    {

        public const byte ESC = 27;    // 换码
        public const byte FS = 28;    // 文本分隔符
        public const byte GS = 29;    // 组分隔符
        public const byte DLE = 16;    // 数据连接换码
        public const byte EOT = 4;    // 传输结束
        public const byte ENQ = 5;    // 询问字符
        public const byte SP = 32;    // 空格
        public const byte HT = 9;    // 横向列表
        public const byte LF = 10;    // 打印并换行（水平定位）
        public const byte CR = 13;    // 归位键
        public const byte FF = 12;    // 走纸控制（打印并回到标准模式（在页模式下） ）
        public const byte CAN = 24;    // 作废（页模式下取消打印数据 ）
        /**
         * 打印纸一行最大的字节
         */
        private const int LINE_BYTE_SIZE = 32;
        /**
         * 分隔符
         */
        private const String SEPARATOR = "$";
        private static StringBuilder sb = new StringBuilder();

        /**
         * 打印机初始化
         * 
         * @return
         */
        public static byte[] init_printer()
        {
            byte[] result = new byte[2];
            result[0] = ESC;
            result[1] = 64;
            return result;
        }

        /**
         * 打开钱箱
         * 
         * @return
         */
        public static byte[] open_money()
        {
            byte[] result = new byte[5];
            result[0] = ESC;
            result[1] = 112;
            result[2] = 48;
            result[3] = 64;
            result[4] = 0;
            return result;
        }

        /**
         * 换行
         * 
         * @param lineNum要换几行
         * @return
         */
        public static byte[] nextLine(int lineNum)
        {
            byte[] result = new byte[lineNum];
            for (int i = 0; i < lineNum; i++)
            {
                result[i] = LF;
            }

            return result;
        }


        /**
         * 绘制下划线（1点宽）
         * 
         * @return
         */
        public static byte[] underlineWithOneDotWidthOn()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 45;
            result[2] = 1;
            return result;
        }

        /**
         * 绘制下划线（2点宽）
         * 
         * @return
         */
        public static byte[] underlineWithTwoDotWidthOn()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 45;
            result[2] = 2;
            return result;
        }

        /**
         * 取消绘制下划线
         * 
         * @return
         */
        public static byte[] underlineOff()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 45;
            result[2] = 0;
            return result;
        }


        /**
         * 选择加粗模式
         * 
         * @return
         */
        public static byte[] boldOn()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 69;
            result[2] = 0xF;
            return result;
        }

        /**
         * 取消加粗模式
         * 
         * @return
         */
        public static byte[] boldOff()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 69;
            result[2] = 0;
            return result;
        }


        /**
         * 左对齐
         * 
         * @return
         */
        public static byte[] alignLeft()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 97;
            result[2] = 0;
            return result;
        }

        /**
         * 居中对齐
         * 
         * @return
         */
        public static byte[] alignCenter()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 97;
            result[2] = 1;
            return result;
        }

        /**
         * 右对齐
         * 
         * @return
         */
        public static byte[] alignRight()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 97;
            result[2] = 2;
            return result;
        }

        /**
         * 水平方向向右移动col列
         * 
         * @param col
         * @return
         */
        public static byte[] set_HT_position(byte col)
        {
            byte[] result = new byte[4];
            result[0] = ESC;
            result[1] = 68;
            result[2] = col;
            result[3] = 0;
            return result;
        }

        /**
         * 字体变大为标准的n倍
         * 
         * @param num
         * @return
         */
        public static byte[] fontSizeSetBig(int num)
        {
            byte realSize = 0;
            switch (num)
            {
                case 1:
                    realSize = 0;
                    break;
                case 2:
                    realSize = 17;
                    break;
                case 3:
                    realSize = 34;
                    break;
                case 4:
                    realSize = 51;
                    break;
                case 5:
                    realSize = 68;
                    break;
                case 6:
                    realSize = 85;
                    break;
                case 7:
                    realSize = 102;
                    break;
                case 8:
                    realSize = 119;
                    break;
            }
            byte[] result = new byte[3];
            result[0] = 29;
            result[1] = 33;
            result[2] = realSize;
            return result;
        }


        /**
         * 字体取消倍宽倍高
         * 
         * @return
         */
        public static byte[] fontSizeSetSmall()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 33;

            return result;
        }

        /**
         * 进纸并全部切割
         * 
         * @return
         */
        public static byte[] feedPaperCutAll()
        {
            byte[] result = new byte[4];
            result[0] = GS;
            result[1] = 86;
            result[2] = 65;
            result[3] = 0;
            return result;
        }

        /**
         * 进纸并切割（左边留一点不切）
         * 
         * @return
         */
        public static byte[] feedPaperCutPartial()
        {
            byte[] result = new byte[4];
            result[0] = GS;
            result[1] = 86;
            result[2] = 66;
            result[3] = 0;
            return result;
        }

        public static byte[] bmpToByte(Bitmap bmp)
        {
            int h = bmp.Height / 24 + 1;
            int w = bmp.Width;
            byte[][] all = new byte[4 + 2 * h + h * w][];

            all[0] = new byte[] { 0x1B, 0x33, 0x00 };

            Color pixelColor;
            // ESC * m nL nH 点阵图  
            byte[] escBmp = new byte[] { 0x1B, 0x2A, 0x21, (byte)(w % 256), (byte)(w / 256) };

            // 每行进行打印  
            for (int i = 0; i < h; i++)
            {
                all[i * (w + 2) + 1] = escBmp;
                for (int j = 0; j < w; j++)
                {
                    byte[] data = new byte[] { 0x00, 0x00, 0x00 };
                    for (int k = 0; k < 24; k++)
                    {
                        if (((i * 24) + k) < bmp.Height)
                        {
                            pixelColor = bmp.GetPixel(j, (i * 24) + k);
                            if (pixelColor.R == 0)
                            {
                                data[k / 8] += (byte)(128 >> (k % 8));
                            }
                        }
                    }
                    all[i * (w + 2) + j + 2] = data;
                }
                //换行  
                all[(i + 1) * (w + 2)] = PrinterCmdUtils.nextLine(1);
            }
            all[h * (w + 2) + 1] = PrinterCmdUtils.nextLine(2);
            all[h * (w + 2) + 2] = PrinterCmdUtils.feedPaperCutAll();
            all[h * (w + 2) + 3] = PrinterCmdUtils.open_money();

            return PrinterCmdUtils.byteMerger(all);
        }

        // ------------------------切纸-----------------------------
        public static byte[] byteMerger(byte[] byte_1, byte[] byte_2)
        {
            byte[] byte_3 = new byte[byte_1.Length + byte_2.Length];
            System.Array.Copy(byte_1, 0, byte_3, 0, byte_1.Length);
            System.Array.Copy(byte_2, 0, byte_3, byte_1.Length, byte_2.Length);
            return byte_3;
        }

        public static byte[] byteMerger(byte[][] byteList)
        {
            int Length = 0;
            for (int i = 0; i < byteList.Length; i++)
            {
                Length += byteList[i].Length;
            }
            byte[] result = new byte[Length];

            int index = 0;
            for (int i = 0; i < byteList.Length; i++)
            {
                byte[] nowByte = byteList[i];
                for (int k = 0; k < byteList[i].Length; k++)
                {
                    result[index] = nowByte[k];
                    index++;
                }
            }
            return result;
        }

        public static byte[][] byte20Merger(byte[] bytes)
        {
            int size = bytes.Length / 10 + 1;
            byte[][] result = new byte[size][];
            for (int i = 0; i < size; i++)
            {
                byte[] by = new byte[((i + 1) * 10) - (i * 10)];
                //从bytes中的第 i * 10 个位置到第 (i + 1) * 10 个位置;
                System.Array.Copy(bytes, i * 10, by, 0, (i + 1) * 10);
                result[i] = by;
            }
            return result;
        }
        public static byte[] lineSpace(byte space)
        {
            return new byte[] { ESC, space };
        }

        public static byte[] reset()
        {
            return new byte[] {27, 64};
        }

        public static byte[] setLineHeight(byte h)
        {
            return new byte[] { 27, 51, h };
        }
        public static byte[] resetLineHeight()
        {
            return new byte[] { 27, 50 };
        }
        public static byte[] setBold(byte n)
        {
            return new byte[] { 27, 69, n };
        }
        public static byte[] printNextLine(byte n)
        {
            return new byte[] { 27,100, n };
        }
    }
}
