using System;


namespace M17Library1.AVM.DBObject
{
    /// <summary>
    /// строка таблицы базы данных
    /// класс родитель для BuyersRow и ProductsRow
    /// для использования ITableSelect<in \T>
    /// </summary>
    public class Row
    {
        public Row() { }

    }

    // для события RowChoiceNotify в BuyersRow и ProductsRow
    public delegate void RowChoiceHendler(Row row);

}
