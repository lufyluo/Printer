﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace PrinterWithChrome
{
    public class BoundObject
    {
        public int MyProperty { get; set; }

        public string MyReadOnlyProperty { get; internal set; }
        public Type MyUnconvertibleProperty { get; set; }
        public SubBoundObject SubObject { get; set; }

        public uint[] MyUintArray
        {
            get { return new uint[] { 7, 8 }; }
        }

        public int[] MyIntArray
        {
            get { return new[] { 1, 2, 3, 4, 5, 6, 7, 8 }; }
        }

        public Array MyArray
        {
            get { return new short[] { 1, 2, 3 }; }
        }

        public byte[] MyBytes
        {
            get { return new byte[] { 3, 4, 5 }; }
        }

        public BoundObject()
        {
            MyProperty = 42;
            MyReadOnlyProperty = "I'm immutable!";
            IgnoredProperty = "I am an Ignored Property";
            MyUnconvertibleProperty = GetType();
            SubObject = new SubBoundObject();
        }

        public void TestCallback(IJavascriptCallback javascriptCallback)
        {
            const int taskDelay = 1500;

            Task.Run(async () =>
            {
                await Task.Delay(taskDelay);

                await javascriptCallback.ExecuteAsync("This callback from C# was delayed " + taskDelay + "ms");
            });
        }

        public int EchoMyProperty()
        {
            return MyProperty;
        }

        public string Repeat(string str, int n)
        {
            string result = String.Empty;
            for (int i = 0; i < n; i++)
            {
                result += str;
            }
            return result;
        }

        public string EchoParamOrDefault(string param = "This is the default value")
        {
            return param;
        }

        public void EchoVoid()
        {
        }

        public Boolean EchoBoolean(Boolean arg0)
        {
            return arg0;
        }

        public Boolean? EchoNullableBoolean(Boolean? arg0)
        {
            return arg0;
        }

        public SByte EchoSByte(SByte arg0)
        {
            return arg0;
        }

        public SByte? EchoNullableSByte(SByte? arg0)
        {
            return arg0;
        }

        public Int16 EchoInt16(Int16 arg0)
        {
            return arg0;
        }

        public Int16? EchoNullableInt16(Int16? arg0)
        {
            return arg0;
        }

        public Int32 EchoInt32(Int32 arg0)
        {
            return arg0;
        }

        public Int32? EchoNullableInt32(Int32? arg0)
        {
            return arg0;
        }

        public Int64 EchoInt64(Int64 arg0)
        {
            return arg0;
        }

        public Int64? EchoNullableInt64(Int64? arg0)
        {
            return arg0;
        }

        public Byte EchoByte(Byte arg0)
        {
            return arg0;
        }

        public Byte? EchoNullableByte(Byte? arg0)
        {
            return arg0;
        }

        public UInt16 EchoUInt16(UInt16 arg0)
        {
            return arg0;
        }

        public UInt16? EchoNullableUInt16(UInt16? arg0)
        {
            return arg0;
        }

        public UInt32 EchoUInt32(UInt32 arg0)
        {
            return arg0;
        }

        public UInt32? EchoNullableUInt32(UInt32? arg0)
        {
            return arg0;
        }

        public UInt64 EchoUInt64(UInt64 arg0)
        {
            return arg0;
        }

        public UInt64? EchoNullableUInt64(UInt64? arg0)
        {
            return arg0;
        }

        public Single EchoSingle(Single arg0)
        {
            return arg0;
        }

        public Single? EchoNullableSingle(Single? arg0)
        {
            return arg0;
        }

        public Double EchoDouble(Double arg0)
        {
            return arg0;
        }

        public Double? EchoNullableDouble(Double? arg0)
        {
            return arg0;
        }

        public Char EchoChar(Char arg0)
        {
            return arg0;
        }

        public Char? EchoNullableChar(Char? arg0)
        {
            return arg0;
        }

        public DateTime EchoDateTime(DateTime arg0)
        {
            return arg0;
        }

        public DateTime? EchoNullableDateTime(DateTime? arg0)
        {
            return arg0;
        }

        public Decimal EchoDecimal(Decimal arg0)
        {
            return arg0;
        }

        public Decimal? EchoNullableDecimal(Decimal? arg0)
        {
            return arg0;
        }

        public String EchoString(String arg0)
        {
            return arg0;
        }

        // TODO: This will currently not work, as it causes a collision w/ the EchoString() method. We need to find a way around that I guess.
        //public String echoString(String arg)
        //{
        //    return "Lowercase echo: " + arg;
        //}

        public String lowercaseMethod()
        {
            return "lowercase";
        }

        public string ReturnJsonEmployeeList()
        {
            return "{\"employees\":[{\"firstName\":\"John\", \"lastName\":\"Doe\"},{\"firstName\":\"Anna\", \"lastName\":\"Smith\"},{\"firstName\":\"Peter\", \"lastName\":\"Jones\"}]}";
        }

        [JavascriptIgnore]
        public string IgnoredProperty { get; set; }

        [JavascriptIgnore]
        public string IgnoredMethod()
        {
            return "I am an Ignored Method";
        }

        public string ComplexParamObject(object param)
        {
            if (param == null)
            {
                return "param is null";
            }
            return "The param type is:" + param.GetType();
        }

        public SubBoundObject GetSubObject()
        {
            return SubObject;
        }
    }
}
