using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using M17Library1.AVM.Command;

namespace M17Library1.AVM.DBObject
{

    /// <summary>
    /// моделирует стрку таблицы Products
    /// </summary>
    public class ProductsRow : Row
    {
        int id;
        string goodName;
        int weight;
        int price;

        public int Id { get { return id; } set { id = value; } }
        public string GoodName { get { return goodName; } set { goodName = value; } }
        public int Weight { get { return weight; } set { weight = value; } }
        public int Price { get { return price; } set { price = value; } }


        public ProductsRow()
        {
            this.id = 0;
            this.goodName = "";
            this.weight = 0;
            this.price = 0;
        }
    }


    /// <summary>
    /// отображение таблицы Products
    /// </summary>
    public class ProductsShowTableAVM : ShowTableAVM
    {
        ProductsRow row;

        public event RowChoiceHendler RowChoiceNotify;  // перебросить выбранный элемент

        public ProductsRow Row { get { return row; } }

        public ProductsShowTableAVM(TableAVM main) : base(main)
        {
            row = new ProductsRow();
        }

        public ProductsShowTableAVM(TableEditAVM main) : base(main)
        {
            row = new ProductsRow();
        }

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
                    row.GoodName = obj[1].ToString();
                    row.Weight = (int)obj[2];
                    row.Price = (int)obj[3];
                    RowChoiceNotify?.Invoke(row);
                }

            }
        }

    }

    /// <summary>
    /// редактирование таблицы Products
    /// </summary>
    public class ProductsEditAWM: EditAWM
    {
        ProductsRow row;


        public ProductsEditAWM(TableAVM main, string query) :
            base(main)
        {
            row = new ProductsRow();


            switch (query)
            {
                case "DELETE":
                    DeleteView();
                    makeaRequest = new WeirdCommand(o => { DeleteDataFromDB(); });
                    (main.Show as ProductsShowTableAVM).RowChoiceNotify += AddDataToDelete;
                    break;
                case "UPDATE":
                    UpdateView();
                    makeaRequest = new WeirdCommand(o => { UpdateDataToDB(); });
                    (main.Show as ProductsShowTableAVM).RowChoiceNotify += AddDataToUpdate;
                    break;
                case "INSERT":
                    InsertView();
                    makeaRequest = new WeirdCommand(o => { InsertDataToDB(); });
                    break;

            }

        }

        public override void InsertView()
        {
            Columns.Add(new Cell("goodName", ""));
            Columns.Add(new Cell("weight", ""));
            Columns.Add(new Cell("price", ""));

        }

        public override void InsertDataToDB()
        {
            int w; int p;
            row.GoodName = Columns[0].Value;
            if (!int.TryParse(Columns[1].Value, out w)) row.Weight = 0;
            else row.Weight = w;
            if (!int.TryParse(Columns[2].Value, out p)) row.Price = 0;
            else row.Price = p;
            

            string sql = $"INSERT INTO Products" +
                    " (goodName, weight, price) VALUES" +
                    " (@goodName, @weight, @price)";
            UpdIns(sql);

        }

        public override void UpdateView()
        {
            Columns.Add(new Cell("Id", ""));
            Columns.Add(new Cell("goodName", ""));
            Columns.Add(new Cell("weight", ""));
            Columns.Add(new Cell("price", ""));

        }

        public void AddDataToUpdate(Row row)
        {
            ProductsRow bRow = row as ProductsRow;
            Columns[0].Value = bRow.Id.ToString();
            Columns[1].Value = bRow.GoodName;
            Columns[2].Value = bRow.Weight.ToString();
            Columns[3].Value = bRow.Price.ToString();

        }

        public override void AddDataToDelete(Row row)
        {
            ProductsRow bRow = row as ProductsRow;
            Columns[0].Value = bRow.Id.ToString();
        }



        public override void UpdateDataToDB()
        {
            int id;
            if (int.TryParse(Columns[0].Value, out id))
            {
                int w; int p;
                row.GoodName = Columns[1].Value;
                if (!int.TryParse(Columns[2].Value, out w)) row.Weight = 0;
                else row.Weight = w;
                if (!int.TryParse(Columns[3].Value, out p)) row.Price = 0;
                else row.Price = p;

                string sql = "UPDATE Products" +
                        " SET goodName = @goodName," +
                        " weight = @weight," +
                        " price = @price" +
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
                            ParameterName = "@goodName",
                            Value = row.GoodName,
                            SqlDbType = SqlDbType.NChar,
                            Size = 10
                        };
                        cmd.Parameters.Add(parameter);

                        parameter = new SqlParameter
                        {
                            ParameterName = "@weight",
                            Value = row.Weight,
                            SqlDbType = SqlDbType.Int,
                        };
                        cmd.Parameters.Add(parameter);

                        parameter = new SqlParameter
                        {
                            ParameterName = "@price",
                            Value = row.Price,
                            SqlDbType = SqlDbType.Int,
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
