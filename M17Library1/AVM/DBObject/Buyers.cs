using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using M17Library1.AVM.Command;

namespace M17Library1.AVM.DBObject
{
    /// <summary>
    /// моделирует стрку таблицы Buyers
    /// </summary>
    public class BuyersRow : Row
    {
        int id;
        string familyName;
        string firstName;
        string patronymic;
        string phone;
        string email;

        public int Id { get { return id; } set { id = value; } }
        public string FamilyName { get { return familyName; } set { familyName = value; } }
        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string Patronymic { get { return patronymic; } set { patronymic = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public string Email { get { return email; } set { email = value; } }

        public BuyersRow() 
        {
            this.id = 0;
            this.familyName = "";
            this.firstName = "";
            this.patronymic = "";
            this.phone = "";
            this.email = "";
        }
    }


    /// <summary>
    /// отображение таблицы Buyers
    /// </summary>
    public class BuyersShowTableAVM: ShowTableAVM 
    {
        BuyersRow row;  // выбранный элемент таблицы

        public event RowChoiceHendler RowChoiceNotify;  // перебросить выбранный элемент


        public BuyersRow Row { get { return row; }  }

        public BuyersShowTableAVM(TableAVM main): base(main)
        {
            row = new BuyersRow();
        }

        public BuyersShowTableAVM(TableEditAVM main) : base(main)
        {
            row = new BuyersRow();
        }

        /// <summary>
        /// запомнить выбранный элемент (строку) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void SelectRow(object sender, SelectedCellsChangedEventArgs e)
        {

            if (e.AddedCells.Count > 0)
            {
                DataGridCellInfo cellInfo;
                var cellInfos = e.AddedCells;
                cellInfo = cellInfos[0];
                if (cellInfo.IsValid)
                {
                    var cont = cellInfo.Column.GetCellContent(cellInfo.Item);
                    var r = (DataRowView)cont.DataContext;
                    object[] obj = r.Row.ItemArray;
                    row.Id = (int)obj[0];
                    row.FamilyName = obj[1].ToString();
                    row.FirstName = obj[2].ToString();
                    row.Patronymic = obj[3].ToString();
                    row.Phone = obj[4].ToString();
                    row.Email = obj[5].ToString();
                    RowChoiceNotify?.Invoke(row);   // передать выбранный элемент

                }
            }
        }
    }

    /// <summary>
    /// редактирование таблицы Buyers
    /// </summary>
    public class BuyersEditAVM: EditAWM
    {
        BuyersRow row;
 
        public BuyersEditAVM(TableAVM main, string query) :
            base(main)
        {
            row = new BuyersRow();
            switch (query) 
            {
                case "DELETE":
                    DeleteView();
                    makeaRequest = new WeirdCommand(o => { DeleteDataFromDB(); });
                    (main.Show as BuyersShowTableAVM).RowChoiceNotify += AddDataToDelete;
                    break;
                case "UPDATE":
                    UpdateView();
                    makeaRequest = new WeirdCommand(o => { UpdateDataToDB(); });
                    (main.Show as BuyersShowTableAVM).RowChoiceNotify += AddDataToUpdate;
                    break;
                case "INSERT":
                    InsertView();
                    makeaRequest = new WeirdCommand(o => { InsertDataToDB(); });
                    break;
            }
        }

        public override void InsertView() 
        {
            Columns.Add(new Cell("familyName", ""));
            Columns.Add(new Cell("firstName", ""));
            Columns.Add(new Cell("patronymic", ""));
            Columns.Add(new Cell("phone", ""));
            Columns.Add(new Cell("email", ""));
        }

        public override void InsertDataToDB() 
        {
            row.FamilyName = Columns[0].Value;
            row.FirstName = Columns[1].Value;
            row.Patronymic = Columns[2].Value;
            row.Phone = Columns[3].Value;
            row.Email = Columns[4].Value;

            string sql = $"INSERT INTO Buyers" +
                    " (familyName, firstName, patronymic, phone, email) VALUES" +
                    " (@familyName, @firstName, @patronymic, @phone, @email)";
            UpdIns(sql);

        }

        public override void UpdateView()
        {
            Columns.Add(new Cell("Id", ""));
            Columns.Add(new Cell("familyName", ""));
            Columns.Add(new Cell("firstName", ""));
            Columns.Add(new Cell("patronymic", ""));
            Columns.Add(new Cell("phone", ""));
            Columns.Add(new Cell("email", ""));
        }

        public void AddDataToUpdate(Row row)
        {
            BuyersRow bRow = row as BuyersRow;
            Columns[0].Value = bRow.Id.ToString();
            Columns[1].Value = bRow.FamilyName;
            Columns[2].Value = bRow.FirstName;
            Columns[3].Value = bRow.Patronymic;
            Columns[4].Value = bRow.Phone;
            Columns[5].Value = bRow.Email;
            
        }

        public override void AddDataToDelete(Row row) 
        {
            BuyersRow bRow = row as BuyersRow;
            Columns[0].Value = bRow.Id.ToString();
        }

        public override void UpdateDataToDB()
        {
            int id;
            if (int.TryParse(Columns[0].Value, out id))
            {

                row.FamilyName = Columns[1].Value;
                row.FirstName = Columns[2].Value;
                row.Patronymic = Columns[3].Value;
                row.Phone = Columns[4].Value;
                row.Email = Columns[5].Value;

                string sql = "UPDATE Buyers" +
                        " SET familyName = @familyName," +
                        " firstName = @firstName," +
                        " patronymic = @patronymic," +
                        " phone = @phone," +
                        " email = @email" +
                        $@" WHERE id = {id}";
                UpdIns(sql);
            }

        }




        public void UpdIns(string sql)
        {
            try
            {
                using (SqlConnection c = base.Main.Set())
                {
                                       
                    c.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, c)) 
                    {
                        SqlParameter parameter = new SqlParameter
                        {
                            ParameterName = "@familyName",
                            Value = this.row.FamilyName,
                            SqlDbType = SqlDbType.NChar,
                            Size = 10
                        };
                        cmd.Parameters.Add(parameter);

                        parameter = new SqlParameter
                        {
                            ParameterName = "@firstName",
                            Value = this.row.FirstName,
                            SqlDbType = SqlDbType.NChar,
                            Size = 10
                        };
                        cmd.Parameters.Add(parameter);

                        parameter = new SqlParameter
                        {
                            ParameterName = "@patronymic",
                            Value = this.row.Patronymic,
                            SqlDbType = SqlDbType.NChar,
                            Size = 10
                        };
                        cmd.Parameters.Add(parameter);

                        parameter = new SqlParameter
                        {
                            ParameterName = "@phone",
                            Value = this.row.Phone,
                            SqlDbType = SqlDbType.NChar,
                            Size = 10
                        };
                        cmd.Parameters.Add(parameter);

                        parameter = new SqlParameter
                        {
                            ParameterName = "@email",
                            Value = this.row.Email,
                            SqlDbType = SqlDbType.NChar,
                            Size = 10
                        };
                        cmd.Parameters.Add(parameter);
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


        
    }
}
