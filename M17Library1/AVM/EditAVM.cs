using System;
using M17Library1.AVM.DBObject;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using M17Library1.AVM.Command;


namespace M17Library1.AVM
{
    public class EditAWM : INotifyPropertyChanged
    {
        TableAVM main;
        ObservableCollection<Cell> cells;

        public TableAVM Main { get { return main; } }
        public ObservableCollection<Cell> Columns { get { return cells; } }

        WeirdCommand brushRequestTable;   // очистить таблицу запроса
        public WeirdCommand BrushRequestTable { get { return brushRequestTable; } }

        protected WeirdCommand makeaRequest;
        public WeirdCommand MakeaRequest { get { return makeaRequest; } }

        public EditAWM(TableAVM main)
        {
            this.main = main;
            this.cells = new ObservableCollection<Cell>();

            brushRequestTable = new WeirdCommand(o =>
            {
                foreach (Cell cell in Columns) cell.Value = "";
            });

            makeaRequest = null;

        }

        

        public virtual void InsertView() { }

        public virtual void InsertDataToDB() { }

        public virtual void UpdateView() { }

        public virtual void UpdateDataToDB() { }


        public void DeleteView()
        {
            Columns.Add(new Cell(main.Key, ""));
        }

        public virtual void AddDataToDelete(Row row) { } 
       
        public void DeleteDataFromDB()
        {
            int id;
            if (int.TryParse(Columns[0].Value, out id))
                Delete(id);
        }

        public void Delete(int id)
        {
            try
            {
                using (SqlConnection c = main.Set())
                {
                    string sql = $@"DELETE {main.TableName} WHERE id = {id}";

                    c.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, c))
                    {

                        cmd.ExecuteNonQuery();
                    }

                    c.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { MessageBox.Show("Успех"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
