using M17Library1.AVM.DBObject;
using M17Library1.AVM.Interfaces;
using System.Data.SqlClient;


namespace M17Library1.AVM
{
    public class TableEditAVM : TableAVM, IAVMEditSelect<EditAWM>
    {

        EditAWM insert;                    // модель вставки
        EditAWM update;                    // модель изменения
        EditAWM delete;                    // модель удаления


        public EditAWM Insert { get { return insert; } }
        public EditAWM Update { get { return update; } }
        public EditAWM Delete { get { return delete; } }

        public TableEditAVM (SqlConnectionStringBuilder strCon, string tableName) : 
            base(strCon, tableName)
        {
            insert = SelectEditAVM("INSERT");
            update = SelectEditAVM("UPDATE");
            delete = SelectEditAVM("DELETE");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public EditAWM SelectEditAVM(string query)
        {
            switch (this.TableName)
            {
                case "Buyers":
                    return new BuyersEditAVM(this, query);
                case "Products":
                    return new ProductsEditAWM(this, query);
            }
            return null;
        }


    }
}
