using System;

namespace KMUtils.Data
{
    [Serializable]
    public class cDataField
    {
        public DateTime date;
        public string category;
        public MoneyType type;
        public int value;
        public string info;

        public cDataField()
        {

        }

        public cDataField(DateTime date_, string category_, MoneyType type_, int value_, string info_)
        {
            date = date_;
            category = category_;
            type = type_;
            value = value_;
            info = info_;
        }
    }

    public enum MoneyType
    {
        None,
        In,
        Out,
    }
}