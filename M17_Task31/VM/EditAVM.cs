using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using M17_Task31.VM.Obj.DBObject;
using M17_Task31.Command;


namespace M17_Task31.VM.Obj
{
    /// <summary>
    /// редактирование таблицы
    /// </summary>
    public class EditAWM : INotifyPropertyChanged
    {
        TableAVM main;
        ObservableCollection<Cell> cells;

        /// <summary>
        /// доступ к основной модели
        /// </summary>
        public TableAVM Main { get { return main; } }

        /// <summary>
        /// модель для представления единицы данных в бд
        /// </summary>
        public ObservableCollection<Cell> Columns { get { return cells; } }


        WeirdCommand brushRequestTable;   // очистить таблицу запроса
        public WeirdCommand BrushRequestTable { get { return brushRequestTable; } }


        protected WeirdCommand makeaRequest;  // выполнить запрос
        public WeirdCommand MakeaRequest { get { return makeaRequest; } }

        public EditAWM(TableAVM main)
        {
            this.main = main;
            this.cells = new ObservableCollection<Cell>();

            brushRequestTable = new WeirdCommand(o =>
            {
                ClearView();
            });

            makeaRequest = null;

        }

        public void ClearView()
        {
            foreach (Cell c in Columns) c.Value = "";
        }

        public virtual void InsertView() { }

        public virtual void InsertDataToDB() { }

        public virtual void UpdateView() { }

        public virtual void UpdateDataToDB() { }


        public virtual void DeleteView()
        {
            Columns.Add(new Cell("Id"));
        }

        public virtual void AddDataToDelete(Row row) { } 
       
        public virtual void DeleteDataFromDB()
        {
        }

 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
