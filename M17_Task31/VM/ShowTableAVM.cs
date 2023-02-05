using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Windows.Controls;
using M17_Task31.Command;
using System;
using System.Linq;
using System.Data.Entity;

namespace M17_Task31.VM.Obj
{


    
    public class ShowTableAVM : INotifyPropertyChanged
    {

        TableAVM main;

        ObservableCollection<CellS> conditionList;


        public TableAVM Main { get { return main; } }

        public ObservableCollection<CellS> ConditionList 
        { 
            get { return conditionList; }
            set { conditionList = value; }
        }


        WeirdCommand openTable;   // привязка таблицы и модели отображения
        public WeirdCommand OpenTable { get { return openTable; } }

        WeirdCommand goRight;   // выбрать следующие N строк таблицы справа
        public WeirdCommand GoRight { get { return goRight; } }

        WeirdCommand goLeft;   // выбрать следующие N строк таблицы слева
        public WeirdCommand GoLeft { get { return goLeft; } }

        WeirdCommand goStart;   // выбрать N строк таблицы в начале
        public WeirdCommand GoStart { get { return goStart; } }

        WeirdCommand goFinish;   // выбрать N строк таблицы в конце
        public WeirdCommand GoFinish { get { return goFinish; } }


        WeirdCommand deleteRow;   // удалить строку
        public WeirdCommand DeleteRow { get { return deleteRow; } }

        WeirdCommand updateRow;   // удалить строку
        public WeirdCommand UpdateRow { get { return updateRow; } }

        public ShowTableAVM(TableAVM main)
        {
            this.main = main;
            


            openTable = new WeirdCommand(o => { NewTableView(); });
            goRight = new WeirdCommand(o => { AddMaxCondition(); });
            goLeft = new WeirdCommand(o => { AddMinCondition(); });
            goStart = new WeirdCommand(o => { StartQuery(); });
            goFinish = new WeirdCommand(o => { FinishQuery(); });

        }

        public ShowTableAVM(TableEditAVM main)
        {
            this.main = main;
            

            openTable = new WeirdCommand(o => { NewTableView(); });
            goRight = new WeirdCommand(o => { AddMaxCondition(); });
            goLeft = new WeirdCommand(o => { AddMinCondition(); });
            goStart = new WeirdCommand(o => { StartQuery(); });
            goFinish = new WeirdCommand(o => { FinishQuery(); });
            deleteRow = new WeirdCommand(o =>
            {
                main.Delete.DeleteDataFromDB();
                NewTableView();
            });
            updateRow = new WeirdCommand(o => 
            {
                main.Update.UpdateDataToDB();
                NewTableView();
            });

        }



        public virtual void SelectRow(object sender, SelectedCellsChangedEventArgs e)
        { }



        /// <summary>
        /// // привязка таблицы и модели отображения (впервые)
        /// </summary>
        public virtual void NewTableView() { }


        /// <summary>
        /// 
        /// </summary>
        public virtual void AddMinCondition()
        {
         
           
        }


        public virtual void AddMaxCondition()
        {
          
           
        }


        public virtual void StartQuery()
        {
            
         
        }


        public virtual void FinishQuery()
        {
            
           
        }



        public void NewTableQuery(ObservableCollection<CellS> list)
        {
            NewTableView();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
