using System;
using M17_Task31.Command;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;
using M17_Task31.VM.Delegates;
using System.Reflection;
using System.Linq;

namespace M17_Task31.VM.Obj
{
    public class RequestAWM : INotifyPropertyChanged
    {
        TableAVM main;                        // модель - источник
        ObservableCollection<CellS> cells;     // поля     
        Cell selectedCell;                    // условие, для добавления в запрос
       

        public event QueryListHendler QueryNotify;  // перебросить список условий


        public TableAVM WorkTable { get { return main; } }
        public ObservableCollection<CellS> Columns { get { return cells; } }






        WeirdCommand brushRequestTable;   // очистить таблицу запроса
        public WeirdCommand BrushRequestTable { get { return brushRequestTable; } }

        WeirdCommand deleteCondition;   // удалить условие
        public WeirdCommand DeleteCondition { get { return deleteCondition; } }

        WeirdCommand selectData;   // проверка условий и выполнение запроса
        public WeirdCommand SelectData { get { return selectData; } }



        public RequestAWM(TableAVM main)
        {
            this.main = main;
            this.cells = new ObservableCollection<CellS>();
            selectedCell = null;

            Type tp = null;
            PropertyInfo[] fields = null;
            switch (main.TableName)
            {
                case "Buyers":
                    tp = Type.GetType("M17_Task31.Buyers");
                    fields = tp.GetProperties();
                    foreach (var f in fields)
                        Columns.Add(new CellS(f.Name, f.PropertyType.ToString()));
                    Columns[1].DBColumnName = "Фамилия";
                    Columns[2].DBColumnName = "Имя";
                    Columns[3].DBColumnName = "Отчество";
                    Columns[4].DBColumnName = "Телефон";
                    break;
                case "Products":
                    tp = Type.GetType("M17_Task31.Products");
                    fields = tp.GetProperties();
                    foreach (var f in fields)
                        Columns.Add(new CellS(f.Name, f.PropertyType.ToString()));
                    Columns[1].DBColumnName = "Товар";
                    Columns[2].DBColumnName = "Вес";
                    Columns[3].DBColumnName = "Цена";
                    break;
            }
            
           

            brushRequestTable = new WeirdCommand(o =>
            {
                foreach (Cell cell in Columns)
                {
                    cell.Compare = "";
                    cell.Value = "";
                }

            });

            deleteCondition = new WeirdCommand(o => 
            {
                if (selectedCell != null)
                    for (int i = 0; i < Columns.Count; i++)
                        if (selectedCell.DBColumnName == Columns[i].DBColumnName)
                        { Columns[i].Compare = ""; Columns[i].Value = ""; }
                selectedCell = null;
            });

            selectData = new WeirdCommand(o => 
            {

                    QueryNotify?.Invoke(Columns);               
            });

            
        }


 



        ///// <summary>
        ///// обрабатывает событие выбора строки таблицы 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public void AddNewCondition(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    selectedCell = new Cell((e.AddedCells[0].Item as Cell).DBColumnName,
        //        (e.AddedCells[0].Item as Cell).Compare,
        //        (e.AddedCells[0].Item as Cell).Value);

        //}





        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
