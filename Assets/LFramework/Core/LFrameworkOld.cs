using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LFramework
{
    public enum DeviceType
    {
        设备,
        电源,
    }

    public enum MouseFlags
    {
        //移动鼠标 
        MouseMove = 0x0001,

        //模拟鼠标左键按下 
        MouseLeftDown = 0x0002,

        //模拟鼠标左键抬起 
        MouseLeftUp = 0x0004,

        //模拟鼠标右键按下 
        MouseRightDown = 0x0008,

        //模拟鼠标右键抬起 
        MouseRightUp = 0x0010,

        //模拟鼠标中键按下 
        MouseMiddleDown = 0x0020,

        //模拟鼠标中键抬起 
        MouseMiddleUp = 0x0040,

        //标示是否采用绝对坐标 
        IsAbsolute = 0x8000,

        //模拟鼠标滚轮滚动操作，滚轮滑动数值为形参delta的值
        MouseWheel = 0x0800,
    }

    [Serializable]
    public enum KeyType
    {
        vKeyLButton = 0x1,   // 鼠标左键
        vKeyRButton = 0x2,   // 鼠标右键
        vKeyCancel = 0x3,    // CANCEL 键
        vKeyMButton = 0x4,   // 鼠标中键
        vKeyBack = 0x8,      // BACKSPACE 键
        vKeyTab = 0x9,       // TAB 键
        vKeyClear = 0xC,     // CLEAR 键
        vKeyReturn = 0xD,    // ENTER 键
        vKeyShift = 0x10,    // SHIFT 键
        vKeyControl = 0x11,  // CTRL 键
        vKeyAlt = 18,        // Alt 键  (键码18)
        vKeyMenu = 0x12,     // MENU 键
        vKeyPause = 0x13,    // PAUSE 键
        vKeyCapital = 0x14,  // CAPS LOCK 键
        vKeyEscape = 0x1B,   // ESC 键
        vKeySpace = 0x20,    // SPACEBAR 键
        vKeyPageUp = 0x21,   // PAGE UP 键
        vKeyEnd = 0x23,      // End 键
        vKeyHome = 0x24,     // HOME 键
        vKeyLeft = 0x25,     // LEFT ARROW 键
        vKeyUp = 0x26,       // UP ARROW 键
        vKeyRight = 0x27,    // RIGHT ARROW 键
        vKeyDown = 0x28,     // DOWN ARROW 键
        vKeySelect = 0x29,   // Select 键
        vKeyPrint = 0x2A,    // PRINT SCREEN 键
        vKeyExecute = 0x2B,  // EXECUTE 键
        vKeySnapshot = 0x2C, // SNAPSHOT 键
        vKeyDelete = 0x2E,   // Delete 键
        vKeyHelp = 0x2F,     // HELP 键
        vKeyNumlock = 0x90,  // NUM LOCK 键

        //字母键A到Z
        vKeyA = 65,
        vKeyB = 66,
        vKeyC = 67,
        vKeyD = 68,
        vKeyE = 69,
        vKeyF = 70,
        vKeyG = 71,
        vKeyH = 72,
        vKeyI = 73,
        vKeyJ = 74,
        vKeyK = 75,
        vKeyL = 76,
        vKeyM = 77,
        vKeyN = 78,
        vKeyO = 79,
        vKeyP = 80,
        vKeyQ = 81,
        vKeyR = 82,
        vKeyS = 83,
        vKeyT = 84,
        vKeyU = 85,
        vKeyV = 86,
        vKeyW = 87,
        vKeyX = 88,
        vKeyY = 89,
        vKeyZ = 90,

        //数字键盘0到9
        vKey0 = 48, // 0 键
        vKey1 = 49, // 1 键
        vKey2 = 50, // 2 键
        vKey3 = 51, // 3 键
        vKey4 = 52, // 4 键
        vKey5 = 53, // 5 键
        vKey6 = 54, // 6 键
        vKey7 = 55, // 7 键
        vKey8 = 56, // 8 键
        vKey9 = 57, // 9 键


        vKeyNumpad0 = 0x60,   //0 键
        vKeyNumpad1 = 0x61,   //1 键
        vKeyNumpad2 = 0x62,   //2 键
        vKeyNumpad3 = 0x63,   //3 键
        vKeyNumpad4 = 0x64,   //4 键
        vKeyNumpad5 = 0x65,   //5 键
        vKeyNumpad6 = 0x66,   //6 键
        vKeyNumpad7 = 0x67,   //7 键
        vKeyNumpad8 = 0x68,   //8 键
        vKeyNumpad9 = 0x69,   //9 键
        vKeyMultiply = 0x6A,  // MULTIPLICATIONSIGN(*)键
        vKeyAdd = 0x6B,       // PLUS SIGN(+) 键
        vKeySeparator = 0x6C, // ENTER 键
        vKeySubtract = 0x6D,  // MINUS SIGN(-) 键
        vKeyDecimal = 0x6E,   // DECIMAL POINT(.) 键
        vKeyDivide = 0x6F,    // DIVISION SIGN(/) 键


        //F1到F12按键
        vKeyF1 = 0x70,  //F1 键
        vKeyF2 = 0x71,  //F2 键
        vKeyF3 = 0x72,  //F3 键
        vKeyF4 = 0x73,  //F4 键
        vKeyF5 = 0x74,  //F5 键
        vKeyF6 = 0x75,  //F6 键
        vKeyF7 = 0x76,  //F7 键
        vKeyF8 = 0x77,  //F8 键
        vKeyF9 = 0x78,  //F9 键
        vKeyF10 = 0x79, //F10 键
        vKeyF11 = 0x7A, //F11 键
        vKeyF12 = 0x7B, //F12 键
    }

    public static partial class DllTool
    {
        /// <summary>
        /// 判断是否开启了大写
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        public static bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];

                GetKeyboardState(bs);

                return (bs[0x14] == 1);
            }
        }

        //导入判断网络是否连接的 .dll
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        //判断网络状况的方法,返回值true为连接，false为未连接
        public static extern bool InternetGetConnectedState(int conState, int reder);

        /// <summary>
        /// 判断网络是否连接
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectedInternet()
        {
            return InternetGetConnectedState(0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DestIP"></param>
        /// <param name="SrcIP"></param>
        /// <param name="MacAddr"></param>
        /// <param name="PhyAddrLen"></param>
        /// <returns></returns>
        [DllImport("Iphlpapi.dll")]
        public static extern int SendARP(int DestIP, int SrcIP, ref long MacAddr, ref int PhyAddrLen);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <returns></returns>
        [DllImport("Ws2_32.dll")]
        public static extern int inet_addr(string ipaddr);

        /// <summary>
        /// 倒计时关闭提示框
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="text">提示内容</param>
        /// <param name="caption">提示标题</param>
        /// <param name="type">弹框样式</param>
        /// <param name="languageId">函数扩展，一般传0</param>
        /// <param name="timeOut">自动关闭时间，毫秒</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "MessageBoxTimeoutA")]
        public static extern int MessageBoxTimeout(IntPtr hWnd, String text, String caption, uint type, int languageId,
            int timeOut);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 按键键盘
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        private static Dictionary<string, int> KeyTypeDic => typeof(KeyType).GetEnumDic();

        /// <summary>
        /// 枚举转字典 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnumDic(this Type type)
        {
            var enumDic = new Dictionary<string, int>();

            var enumKey = Enum.GetNames(type);
            var enumValue = Enum.GetValues(type);
            for (var i = 0; i < enumKey.Length; i++)
            {
                enumDic.Add(enumKey[i], (int)enumValue.GetValue(i));
            }

            return enumDic;
        }

        /// <summary>
        /// 键盘 按下 keyName 名字
        /// </summary>
        /// <param name="keyName">字节方式</param>
        public static void KeyPress(byte keyName) //定义“按一下”方法
        {
            keybd_event(keyName, 0, 0, 0);
            keybd_event(keyName, 0, 2, 0);
        }

        /// <summary>
        /// 键盘 按下 keyName 名字
        /// </summary>
        /// <param name="keyType">枚举方式</param>
        public static void KeyPress(KeyType keyType) //定义“按一下”方法
        {
            keybd_event((byte)KeyTypeDic[keyType.ToString()], 0, 0, 0);
            keybd_event((byte)KeyTypeDic[keyType.ToString()], 0, 2, 0);
        }

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// 模拟鼠标事件
        /// </summary>
        /// <param name="flag">执行的键值</param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="delta"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int mouse_event(int flag, int dx, int dy, int delta, int info);
    }

    public static partial class DataProcessingTool
    {
        //16进制 转2进制
        public static string HexString2BinString(string hexString)
        {
            string result = string.Empty;
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                // 去掉格式串中的空格，即可去掉每个4位二进制数之间的空格，
                result += string.Format("{0:d4}", v2);
            }

            return result;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public static byte[] GetCRC(byte[] byteData)
        {
            byte[] CRC = new byte[2];

            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001; //异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8); //高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);        //低位在前
            return CRC;
        }


        /// <summary>
        /// 字符串转16进制
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] PackData(string msg)
        {
            string hexString = msg.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }

            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            }

            return returnBytes;
        }

        /// <summary>
        /// 16进制转字符串
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UnpackData(byte[] date, int count)
        {
            string returnStr = "";
            if (date != null)
            {
                for (int i = 0; i < count; i++)
                {
                    returnStr += date[i].ToString("X2");
                    if (i != count)
                    {
                        returnStr += " ";
                    }
                }
            }

            return returnStr;
        }


        public static string UnpackData(byte[] date)
        {
            string returnStr = "";
            foreach (var item in date)
            {
                returnStr += item;
            }

            return returnStr;
        }


        /// <summary>
        /// LRC和校验 8路时序器测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string LRC(byte[] data)
        {
            int lrc = 0;
            foreach (byte c in data)
            {
                lrc += c;
            }

            string str = lrc.ToString("x8");
            return str[str.Length - 2].ToString() + str[str.Length - 1];
        }

        /// <summary>
        /// LRC和校验 8路时序器测试
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string LRC(string data2)
        {
            byte[] data = PackData(data2);

            int lrc = 0;
            foreach (byte c in data)
            {
                lrc += c;
            }

            string str = lrc.ToString("x8");
            return str[str.Length - 2].ToString() + str[str.Length - 1];
        }


        /// <summary>
        /// CRC16校验
        /// </summary>
        public static string[] CRC16_Calibration(string str)
        {
            byte[] vs = PackData(str);
            vs = GetCRC(vs);
            return UnpackData(vs, vs.Length).Split(' ');
        }

        /// <summary>
        /// CRC16校验
        /// </summary>
        public static string CRC16_CalibrationToString(string str)
        {
            byte[] vs = PackData(str);
            vs = GetCRC(vs);
            return UnpackData(vs, vs.Length);
        }


        /// <summary>
        /// 从十进制转换到十六进制
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        public static string Ten2Hex(string ten)
        {
            ulong tenValue = Convert.ToUInt64(ten);
            ulong divValue, resValue;
            string hex = "";
            do
            {
                //divValue = (ulong)Math.Floor(tenValue / 16);

                divValue = (ulong)Math.Floor((decimal)(tenValue / 16));

                resValue = tenValue % 16;
                hex = tenValue2Char(resValue) + hex;
                tenValue = divValue;
            }
            while (tenValue >= 16);

            if (tenValue != 0)
            {
                hex = tenValue2Char(tenValue) + hex;
            }

            if (hex.Length == 1)
            {
                hex = "0" + hex;
            }

            return hex;
        }


        public static float To16Float(string s)
        {
            MatchCollection matches = Regex.Matches(s, @"[0-9A-Fa-f]{2}");
            byte[] bytes = new byte[matches.Count];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(matches[i].Value, System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            float m = BitConverter.ToSingle(bytes.Reverse().ToArray(), 0);
            return m;
        }


        public static string tenValue2Char(ulong ten)
        {
            switch (ten)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return ten.ToString();
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return "";
            }
        }
    }

    public static partial class TCPTool
    {
        /// <summary>
        /// 删除路径内的文件
        /// </summary>
        /// <param name="dic"></param>
        public static void DeleteDirectory(string dic)
        {
            Directory.Delete(dic, true);
            Directory.CreateDirectory(dic);
        }

        /// <summary>
        /// 根据名字退出运行的程序
        /// </summary>
        /// <param name="name"></param>
        public static void ExitProgram(string name)
        {
            var apps = Process.GetProcessesByName(name);
            if (apps.Length <= 0)
            {
                return;
            }

            if (!apps[0].CloseMainWindow())
            {
                apps[0].Kill();
            }
        }


        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream();
            formatter.Serialize(rems, obj);
            return rems.GetBuffer();
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object BytesToObject(byte[] Bytes, int index, int count)
        {
            try
            {
                var formatter = new BinaryFormatter();
                var rems = new MemoryStream(Bytes, index, count);
                Bytes = null;
                return formatter.Deserialize(rems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + $"反序列");
            }

            return null;
        }


        public static T BytesToObject<T>(byte[] Bytes, int index, int count)
        {
            return (T)BytesToObject(Bytes, index, count);
        }


        public static List<byte[]> GetChunkList(byte[] full, int size)
        {
            var allChunks = new List<byte[]>(); // 声明可返回数组
            var chunk = new List<byte>();       // 声明字节块

            for (var chunkIndex = 0; chunkIndex < full.Length / size + 1; chunkIndex++)
            {
                for (var i = chunkIndex * size; i < Clamp((chunkIndex + 1) * size, 0, full.Length); i++)
                    chunk.Add(full[i]); // 向区块数组添加字节

                //if (full[i] != 0)
                //    _notZeroBytesCount++;                                        // 需要检查客户收到的图片
                allChunks.Add(chunk.ToArray());
                chunk = new List<byte>();
            }

            return allChunks;
        }

        public static int Clamp(int value, int min, int max)
        {
            int i;
            if (value < min)
                i = min;
            else if (value > max)
                i = max;
            else
                i = value;

            return i;
        }


        ///<summary>
        /// SendArp获取MAC地址
        ///</summary>
        ///<param name="RemoteIP">目标机器的IP地址如(192.168.1.1)</param>
        ///<returns>目标机器的mac 地址</returns>
        public static string GetMacAddress(string RemoteIP)
        {
            var macAddress = new StringBuilder();

            try
            {
                var remote = DllTool.inet_addr(RemoteIP);
                var macInfo = new long();
                var length = 6;
                DllTool.SendARP(remote, 0, ref macInfo, ref length);
                var temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();
                var x = 12;
                for (var i = 0; i < 6; i++)
                {
                    if (i == 5)
                        macAddress.Append(temp.Substring(x - 2, 2));
                    else
                        macAddress.Append(temp.Substring(x - 2, 2) + "-");

                    x -= 2;
                }

                return macAddress.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return macAddress.ToString();
            }
        }


        /// <summary>
        /// 截取前面 r 位 为新数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static byte[] CutBuffer(byte[] buffer, int r)
        {
            var bytes = new byte[r];
            //for (int i = 0; i < r; i++)
            //{
            //    bytes[i] = (byte)buffer[i];
            //}
            Array.ConstrainedCopy(buffer, 0, bytes, 0, r);
            return bytes;
        }


        public static byte[] HeadPack(byte[] bufferBytes, byte h)
        {
            var list = new List<byte>();
            list.Add(h);
            list.AddRange(bufferBytes);
            return list.ToArray();
        }

        public static byte[] HeadPack(byte[] bufferBytes, byte[] h)
        {
            var list = new List<byte>();
            list.AddRange(h);
            list.AddRange(bufferBytes);
            return list.ToArray();
        }

        public static byte[] HeadRePack(byte[] bufferBytes, int cont = 1)
        {
            var list = bufferBytes.ToList();
            for (int i = 0; i < cont; i++)
            {
                list.RemoveAt(0);
            }

            return list.ToArray();
        }


        /// <summary>
        /// 检查临时文件是否存在 ,返回当前断点       (服务端使用)
        /// </summary>
        public static SufixType CheckSufix(string path, int count, out int currentPoint)
        {
            currentPoint = 0;
            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // 读取续传信息
                    // 读取头部    
                    var counqt = fs.ReadByte();
                    if (counqt != count)
                    {
                        //current = counqt;
                        //currentPoint = 0;
                        return SufixType.FenDuanErro;
                    }

                    // 找到断点 
                    var bfBytes = new byte[count];

                    fs.Read(bfBytes, 0, count);

                    for (var i = 0; i < count; i++)
                        if (bfBytes[i] != 0)
                            currentPoint = i;

                    if (currentPoint == 0)
                    {
                        currentPoint = 0;
                        return SufixType.FenDuanZero;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("文件流出错");
            }

            return SufixType.FenDuanOk;
        }


        public static string BytesToStringByEncoding(byte[] buffer, int index, int lenth, StringType codeType)
        {
            var rec = "";
            switch (codeType)
            {
                case StringType.UTF8:
                    rec = Encoding.UTF8.GetString(buffer, index, lenth);
                    break;
                case StringType.GB2312:
                    rec = Encoding.GetEncoding("GB2312").GetString(buffer, index, lenth);
                    break;
                case StringType.GBK:
                    rec = Encoding.GetEncoding("GBK").GetString(buffer, index, lenth);
                    break;
                case StringType.ASCII:
                    rec = Encoding.ASCII.GetString(buffer, index, lenth);
                    break;
                case StringType.Unicode:
                    rec = Encoding.Unicode.GetString(buffer, index, lenth);
                    break;
                case StringType.Hexadecimal:
                    rec = DataProcessingTool.UnpackData(buffer, lenth);
                    break;
                default:
                    rec = Encoding.Default.GetString(buffer, index, lenth);
                    break;
            }

            return rec;
        }

        public static byte[] StringToBytesByEncoding(string str, StringType codeType)
        {
            byte[] buffer;
            switch (codeType)
            {
                case StringType.UTF8:
                    buffer = Encoding.UTF8.GetBytes(str);
                    break;
                case StringType.GB2312:
                    buffer = Encoding.GetEncoding("GB2312").GetBytes(str);
                    break;
                case StringType.GBK:
                    buffer = Encoding.GetEncoding("GBK").GetBytes(str);
                    break;
                case StringType.ASCII:
                    buffer = Encoding.ASCII.GetBytes(str);
                    break;
                case StringType.Unicode:
                    buffer = Encoding.Unicode.GetBytes(str);
                    break;
                case StringType.Hexadecimal:
                    buffer = DataProcessingTool.PackData(str);
                    break;
                default:
                    buffer = Encoding.Default.GetBytes(str);
                    break;
            }

            return buffer;
        }

        /// <summary>
        /// 转换long类型到时间格式字符串
        /// </summary>
        /// <param name="longTime"></param>
        /// <returns></returns>
        public static string SetLongToTime(long longTime)
        {
            int time = (int)(longTime / 1000);
            DateTime fromTimeDate = DateTime.Today + new TimeSpan(0, 0, time);
            return fromTimeDate.TimeOfDay.ToString();
            ;
        }

        /// <summary>
        /// 服务端使用
        /// </summary>
        public enum SufixType
        {
            /// <summary>
            /// 分段对不上
            /// </summary>
            FenDuanErro,

            /// <summary>
            /// 分段点位是零0
            /// </summary>
            FenDuanZero,

            /// <summary>
            /// 已经获取到了分段  fenduanPoint
            /// </summary>
            FenDuanOk
        }
    }


    public enum StringType
    {
        UTF8,
        UTF7,
        UTF32,
        GB2312,
        GBK,
        ASCII,
        Unicode,
        BigEndianUnicode,
        Hexadecimal
    }


    public class Loom : MonoBehaviour
    {
        public static int maxThreads = 6;
        static int numThreads;
        private static Loom _current;
        private int _count;

        public static Loom Current

        {
            get
            {
                Initialize();
                return _current;
            }
        }


        //void Awake()
        //{
        //    _current = this;
        //    initialized = true;

        //}

        static bool initialized;

        public static void Initialize()
        {
            if (!initialized)
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                initialized = true;
                GameObject g = new GameObject("Loom");
                DontDestroyOnLoad(g);
                _current = g.AddComponent<Loom>();
            }
        }


        private List<Action> _actions = new List<Action>();

        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }

        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        public static void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        public static void QueueOnMainThread(Action action, float time)

        {
            if (time != 0)
            {
                if (Current != null)
                {
                    lock (Current._delayed)
                    {
                        Current._delayed.Add
                        (
                            new DelayedQueueItem
                            {
                                time = Time.time + time,
                                action = action
                            }
                        );
                    }
                }
            }
            else
            {
                if (Current != null)
                {
                    lock (Current._actions)
                    {
                        Current._actions.Add(action);
                    }
                }
            }
        }


        public static Thread RunAsync(Action a)
        {
            Initialize();
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }

            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, a);
            return null;
        }


        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch { }

            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
        }


        void OnDisable()
        {
            if (_current == this)
            {
                _current = null;
            }
        }


        List<Action> _currentActions = new List<Action>();


        // Update is called once per frame

        void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }

            foreach (var a in _currentActions)
            {
                a();
            }

            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                foreach (var item in _currentDelayed)
                {
                    _delayed.Remove(item);
                }
            }

            foreach (var delayed in _currentDelayed)
            {
                delayed.action();
            }
        }
    }

}