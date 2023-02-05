using System;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Data;
using M17_Task31.VM.Interfaces;
using M17_Task31.VM.Obj.DBObject;
using System.Data.Entity;

namespace M17_Task31.VM.Obj
{
    /// <summary>
    /// модель для интерфейса работы с таблицей бд
    /// IAVMShowSelect<ShowTableAVM> для работы с конкретной таблицей
    /// </summary>
    public class TableAVM: INotifyPropertyChanged, IAVMShowSelect<ShowTableAVM>
    {
        MyFirstDBEntities context;
        string tableName;                  // имя таблицы

        ShowTableAVM show;                 // модель таблицы
        RequestAWM select;                 // модель выборки

        /// <summary>
        /// подключение к бд
        /// </summary>
        public MyFirstDBEntities Context { get { return context; } }

        /// <summary>
        /// имя таблицы
        /// </summary>
        public string TableName { get { return tableName; } }

        /// <summary>
        /// отображение таблицы
        /// </summary>
        public ShowTableAVM Show { get { return show; } }

        /// <summary>
        /// отображение интерфейса для задания условий выборки данных
        /// </summary>
        public RequestAWM Select { get { return select; } }

        /// <summary>
        /// создать модель для таблицы
        /// </summary>
        /// <param name="strCon">строка состояния</param>
        /// <param name="tableName">имя таблицы</param>
        public TableAVM(MyFirstDBEntities context, string tableName)
        {
            this.context = context;
            this.tableName = tableName;
            show = SelectShowAVM();
            select = new RequestAWM(this);
            Select.QueryNotify += Show.NewTableQuery;
            show.ConditionList = Select.Columns;
            show.NewTableView();
        }

        /// <summary>
        /// выбор отображения 
        /// </summary>
        /// <returns></returns>
        public ShowTableAVM SelectShowAVM()
        {
            switch (TableName)
            {
                case "Buyers":
                    if (this.GetType() == typeof(TableAVM))
                        return new BuyersShowTableAVM(this as TableAVM);
                    if (this.GetType() == typeof(TableEditAVM))
                        return new BuyersShowTableAVM(this as TableEditAVM);
                    break;
                case "Products":
                    if (this.GetType() == typeof(TableAVM))
                        return new ProductsShowTableAVM(this as TableAVM);
                    if (this.GetType() == typeof(TableEditAVM))
                        return new ProductsShowTableAVM(this as TableEditAVM);
                    break;
            }
            return null;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


 




}
