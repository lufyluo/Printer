using System;

namespace Printer.Core.Printer.Model
{
    public class TicketFoot
    {
        private String decAmmBalance;        //点餐总额

        public String DecAmmBalance
        {
            get { return decAmmBalance; }
            set { decAmmBalance = value; }
        }
        private String decRebeatBalance;    //券内优惠

        public String DecRebeatBalance
        {
            get { return decRebeatBalance; }
            set { decRebeatBalance = value; }
        }
        private String decGroupBuyMoney;    //+

        public String DecGroupBuyMoney
        {
            get { return decGroupBuyMoney; }
            set { decGroupBuyMoney = value; }
        }
        private String decPaidBalance;        //折后支付

        public String DecPaidBalance
        {
            get { return decPaidBalance; }
            set { decPaidBalance = value; }
        }
        private String decMemberCoupon;        //会员优惠

        public String DecMemberCoupon
        {
            get { return decMemberCoupon; }
            set { decMemberCoupon = value; }
        }
        private String decWipeMoney;        //抹零优惠

        public String DecWipeMoney
        {
            get { return decWipeMoney; }
            set { decWipeMoney = value; }
        }
        private String wipe;                //

        public String Wipe
        {
            get { return wipe; }
            set { wipe = value; }
        }
        private String cashmoney;            //现金支付

        public String Cashmoney
        {
            get { return cashmoney; }
            set { cashmoney = value; }
        }
        private String bankmoney;            //银行卡支付

        public String Bankmoney
        {
            get { return bankmoney; }
            set { bankmoney = value; }
        }
        private String membermoney;            //会员支付

        public String Membermoney
        {
            get { return membermoney; }
            set { membermoney = value; }
        }
        private String counts;                //餐品数量

        public String Counts
        {
            get { return counts; }
            set { counts = value; }
        }

        private String givechange;            //找零

        public String Givechange
        {
            get { return givechange; }
            set { givechange = value; }
        }

        private string rechMoney;
        /// <summary>
        /// 充值金额
        /// </summary>
        public string RechMoney
        {
            get { return rechMoney; }
            set { rechMoney = value; }
        }
    }
}
