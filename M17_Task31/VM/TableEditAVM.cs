using System.Data.SqlClient;
using M17_Task31.VM.Interfaces;
using M17_Task31.VM.Obj.DBObject;


namespace M17_Task31.VM.Obj
{
    /// <summary>
    /// редактирование таблицы
    /// </summary>
    public class TableEditAVM : TableAVM, IAVMEditSelect<EditAWM>
    {

        EditAWM insert;                    // модель вставки
        EditAWM update;                    // модель изменения
        EditAWM delete;                    // модель удаления


        public EditAWM Insert { get { return insert; } }
        public EditAWM Update { get { return update; } }
        public EditAWM Delete { get { return delete; } }

        public TableEditAVM (MyFirstDBEntities context, string tableName) : 
            base(context, tableName)
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
