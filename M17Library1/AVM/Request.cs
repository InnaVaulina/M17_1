using System;


namespace M17Library1.AVM
{
    public class SelectRequest
    {
        string select;
        string from;
        BoolCompare where;
        Cell whereExp;
        string order;


        public BoolCompare Where { get { return where; } }
        public Cell WhereExp { get { return whereExp; } set { whereExp = value; } }
        public string Order { get { return order; } set { order = value; } }
        public SelectRequest(string from)
        {
            this.select = "*";
            this.from = from;
            this.where = new BoolCompare();
            this.whereExp = null;
            this.order = "";
        }
        public string RequestString()
        {
            string s = $@"SELECT {select} FROM {from}";
            if (where.Count > 0 && whereExp == null) s = $@"{s} WHERE {Where.BoolCompareToString()}";
            if (whereExp != null && where.Count == 0) 
                s = $@"{s} WHERE {whereExp.DBColumnName} {whereExp.Compare} {whereExp.Value}";
            if (whereExp != null && where.Count > 0)
                s = $@"{s} WHERE ( {Where.BoolCompareToString()} ) AND {whereExp.DBColumnName} {whereExp.Compare} {whereExp.Value}";
            if (order != "") s = $@"{s} ORDER BY {order} DESC";
            return s;
        }
    }

 

}
