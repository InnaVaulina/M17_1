using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using M17_Task31.VM.Obj.DBObject;

namespace M17_Task31.VM.Obj
{
    public static class Query
    {
        static public IQueryable<Buyers> MyQuery(this IQueryable<Buyers> before, BuyersShowTableAVM y) 
        {
            return y.Query(before);
        }

        static public IQueryable<Products> MyQuery(this IQueryable<Products> before, ProductsShowTableAVM y)
        {
            return y.Query(before);
        }
    }
}
