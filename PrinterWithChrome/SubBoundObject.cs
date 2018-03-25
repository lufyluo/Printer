using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterWithChrome
{
    public class SubBoundObject
    {
        public string SimpleProperty { get; set; }

        public SubBoundObject()
        {
            SimpleProperty = "这是子对象属性";
        }

        public string GetMyType()
        {
            return "My Type is " + GetType();
        }

        public string EchoSimpleProperty()
        {
            return SimpleProperty;
        }
    }

}
