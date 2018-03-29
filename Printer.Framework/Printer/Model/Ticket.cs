using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer.Model
{
    public class Ticket
    {
        private String id;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }
        private String type;

        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        private String parentid;

        public String Parentid
        {
            get { return parentid; }
            set { parentid = value; }
        }
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private String num;

        public String Num
        {
            get { return num; }
            set { num = value; }
        }
        private String price;

        public String Price
        {
            get { return price; }
            set { price = value; }
        }
        private String money;

        public String Money
        {
            get { return money; }
            set { money = value; }
        }
        private String rebetmoney;

        public String Rebetmoney
        {
            get { return rebetmoney; }
            set { rebetmoney = value; }
        }
        private String rebetName;

        public String RebetName
        {
            get { return rebetName; }
            set { rebetName = value; }
        }
        private String rebetid;

        public String Rebetid
        {
            get { return rebetid; }
            set { rebetid = value; }
        }
        private String rebetType;

        public String RebetType
        {
            get { return rebetType; }
            set { rebetType = value; }
        }
        private String grouptype;

        public String Grouptype
        {
            get { return grouptype; }
            set { grouptype = value; }
        }
        private String groupid;

        public String Groupid
        {
            get { return groupid; }
            set { groupid = value; }
        }
        private String serialnumber;

        public String Serialnumber
        {
            get { return serialnumber; }
            set { serialnumber = value; }
        }

        private String addonname;

        public String AddOnName
        {
            get { return addonname; }
            set { addonname = value; }
        }

    }
}
