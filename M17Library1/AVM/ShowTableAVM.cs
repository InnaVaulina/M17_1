using M17Library1.AVM.Command;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace M17Library1.AVM
{


    
    public class ShowTableAVM : INotifyPropertyChanged
    {
        DataTable dt;
        TableAVM main;

       
        public TableAVM Main { get { return main; } }
        public DataTable Dt { get { return dt; } set { dt = value; OnPropertyChanged("Dt"); } }

       
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
            this.dt = new DataTable();

            openTable = new WeirdCommand(o => { NewTableView(); });
            goRight = new WeirdCommand(o => { main.AddMaxCondition(); });
            goLeft = new WeirdCommand(o => { main.AddMinCondition(); });
            goStart = new WeirdCommand(o => { main.StartQuery(); });
            goFinish = new WeirdCommand(o => { main.FinishQuery(); });
            
        }

        public ShowTableAVM(TableEditAVM main)
        {
            this.main = main;
            this.dt = new DataTable();

            openTable = new WeirdCommand(o => { NewTableView(); });
            goRight = new WeirdCommand(o => { main.AddMaxCondition(); });
            goLeft = new WeirdCommand(o => { main.AddMinCondition(); });
            goStart = new WeirdCommand(o => { main.StartQuery(); });
            goFinish = new WeirdCommand(o => { main.FinishQuery(); });
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
        public void NewTableView() 
        { 
            DataTable t = main.MakeTable();
            if (t != null) Dt = t;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
