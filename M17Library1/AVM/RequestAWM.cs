using System;
using M17Library1.AVM.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;

namespace M17Library1.AVM
{
    public class RequestAWM : INotifyPropertyChanged
    {
        TableAVM main;                        // модель - источник
        ObservableCollection<Cell> cells;     // поля     
        string selectQueryString;             // запрос
        Cell selectedCell;                    // условие, для добавления в запрос

        public TableAVM WorkTable { get { return main; } }
        public ObservableCollection<Cell> Columns { get { return cells; } }

        public string SelectQueryString
        {
            get { return selectQueryString; }
            set { selectQueryString = value; OnPropertyChanged("SelectQueryString"); }
        }




        WeirdCommand brushRequestTable;   // очистить таблицу запроса
        public WeirdCommand BrushRequestTable { get { return brushRequestTable; } }

        WeirdCommand addCondition;   // добавить условие
        public WeirdCommand AddCondition { get { return addCondition; } }

        WeirdCommand deleteCondition;   // удалить условие
        public WeirdCommand DeleteCondition { get { return deleteCondition; } }

        WeirdCommand selectData;   // добавить условие
        public WeirdCommand SelectData { get { return selectData; } }



        public RequestAWM(TableAVM main)
        {
            this.main = main;
            this.cells = new ObservableCollection<Cell>();
            ShowSelectTable();
            SelectQueryString = main.SelectQuery.RequestString();


            brushRequestTable = new WeirdCommand(o =>
            {
                foreach (Cell cell in Columns)
                {
                    cell.Compare = "";
                    cell.Value = "";
                }

                main.SelectQuery.Where.DeleteAll();
                main.SelectQuery.WhereExp = null;
                SelectQueryString = main.SelectQuery.RequestString();
            });

            addCondition = new WeirdCommand(o => { ConditionAdd((string)o); });

            deleteCondition = new WeirdCommand(o => { DeleteSelCondition(); });

            selectData = new WeirdCommand(o => { main.Query = SelectQueryString; });

        }



        /// <summary>
        /// обрабатывает событие выбора строки таблицы запроса Select
        /// dataRequest.SelectedCellsChanged += workTable.Select.AddNewCondition;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddNewCondition(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedCell = new Cell((e.AddedCells[0].Item as Cell).DBColumnName,
                (e.AddedCells[0].Item as Cell).Compare,
                (e.AddedCells[0].Item as Cell).Value);

        }

        public void DeleteSelCondition()
        {
            main.SelectQuery.Where.Delete(selectedCell);
            SelectQueryString = main.SelectQuery.RequestString();
        }


        public void ConditionAdd(string c)
        {

            if (c == null) main.SelectQuery.Where.Add(selectedCell);
            if (c == "OR") main.SelectQuery.Where.Add(selectedCell, $"{c}");
            if (c == "AND") main.SelectQuery.Where.Add(selectedCell, $"{c}");
            
            SelectQueryString = main.SelectQuery.RequestString();
        }



        void ShowSelectTable()
        {
            try
            {
                using (SqlConnection c = main.Set())
                {

                    c.Open();
                    var sql = $@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{main.TableName}'";

                    SqlCommand command = new SqlCommand(sql, c);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {

                            Columns.Add(new Cell(reader.GetValue(3).ToString(), reader.GetValue(7).ToString()));
                        }
                    reader.Close();
                    c.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
