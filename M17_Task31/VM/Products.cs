using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Data.Entity;
using M17_Task31.Command;
using System.Collections.ObjectModel;
using M17_Task31.VM.Delegates;
using System.Linq;

namespace M17_Task31.VM.Obj.DBObject
{

    /// <summary>
    /// отображение таблицы Products
    /// </summary>
    public class ProductsShowTableAVM : ShowTableAVM
    {
        BindingList<Products> dt;   // таблица
        IQueryable<Products> queryGeneral;
        IQueryable<Products> queryPart;
        int keyMaxValue;                   // левый ключ для навигации по таблице
        int keyMinValue;                   // правый ключ для навигации по таблице

        Products row;

        public event RowChoiceHendler RowChoiceNotify;  // перебросить выбранный элемент

        public BindingList<Products> Dt
        {
            get { return dt; }
            set { dt = value; OnPropertyChanged("Dt"); }
        }

        public BindingList<Products> NewDt()
        {
            var d = new BindingList<Products>();
            foreach (var r in queryPart)
                d.Add(r);
            if (d.Count() > 0)
            {
                this.keyMaxValue = d.Max(e => e.Id);
                this.keyMinValue = d.Min(e => e.Id);
            }
            else
            {
                this.keyMaxValue = 0;
                this.keyMinValue = 0;
            }

            return d;
        }

        public Products Row { get { return row; } }

        public ProductsShowTableAVM(TableAVM main) : base(main)
        {
            row = new Products();

        }

        public ProductsShowTableAVM(TableEditAVM main) : base(main)
        {
            row = new Products();

        }


        public override void NewTableView()
        {

            queryGeneral = base.Main.Context.Products.MyQuery(this);
            queryPart = queryGeneral.OrderBy(e => e.Id).Take(5);
            queryPart.Load();
            Dt = NewDt();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AddMinCondition()
        {
            queryPart = queryGeneral.Where(e => e.Id < keyMinValue).OrderByDescending(e => e.Id).Take(5).OrderBy(e => e.Id);
            if (queryPart.Count() == 0)
                queryPart = queryGeneral.OrderBy(e => e.Id).Take(5);
            queryPart.Load();
            Dt = NewDt();
        }


        public override void AddMaxCondition()
        {
            queryPart = queryGeneral.Where(e => e.Id > keyMaxValue).Take(5);
            if (queryPart.Count() == 0)
                queryPart = queryGeneral.OrderByDescending(e => e.Id).Take(5).OrderBy(e => e.Id);
            queryPart.Load();
            Dt = NewDt();
        }


        public override void StartQuery()
        {
            queryPart = queryGeneral.OrderBy(e => e.Id).Take(5);
            queryPart.Load();
            Dt = NewDt();
        }


        public override void FinishQuery()
        {
            queryPart = queryGeneral.OrderByDescending(e => e.Id).Take(5).OrderBy(e => e.Id);
            queryPart.Load();
            Dt = NewDt();
        }


        public IQueryable<Products> Query(IQueryable<Products> before)
        {
            IQueryable<Products> result = before;

            // Id
            if (ConditionList[0].Compare != "" & ConditionList[0].Value != "")
            {
                string[] s = ConditionList[0].Value.Split(' ');
                int x = int.Parse(s[0]);
                switch (ConditionList[0].Compare)
                {
                    case "=":
                        result = result.Where(e => e.Id == x);
                        break;
                    case ">":
                        result = result.Where(e => e.Id > x);
                        break;
                    case "<":
                        result = result.Where(e => e.Id < x);
                        break;
                    case "между":
                        int x2 = int.Parse(s[1]);
                        result = result.Where(e => e.Id >= x & e.Id <= x2);
                        break;
                }
            }

            // goodName
            if (ConditionList[1].Compare != "" & ConditionList[1].Value != "")
            {
                string x = ConditionList[1].Value;
                switch (ConditionList[1].Compare)
                {
                    case "=":
                        result = result.Where(e => e.goodName == x);
                        break;
                    case "начинается с":
                        result = result.Where(e => e.goodName.StartsWith(x));
                        break;
                }
            }

            // weight
            if (ConditionList[2].Compare != "" & ConditionList[2].Value != "")
            {
                string[] s = ConditionList[2].Value.Split(' ');
                int x = int.Parse(s[0]);
                switch (ConditionList[2].Compare)
                {
                    case "=":
                        result = result.Where(e => e.weight == x);
                        break;
                    case ">":
                        result = result.Where(e => e.weight > x);
                        break;
                    case "<":
                        result = result.Where(e => e.weight < x);
                        break;
                    case "между":
                        int x2 = int.Parse(s[1]);
                        result = result.Where(e => e.weight >= x & e.weight <= x2);
                        break;
                }
            }

            // price
            if (ConditionList[3].Compare != "" & ConditionList[3].Value != "")
            {
                string[] s = ConditionList[3].Value.Split(' ');
                int x = int.Parse(s[0]);
                switch (ConditionList[3].Compare)
                {
                    case "=":
                        result = result.Where(e => e.price == x);
                        break;
                    case ">":
                        result = result.Where(e => e.price > x);
                        break;
                    case "<":
                        result = result.Where(e => e.price < x);
                        break;
                    case "между":
                        int x2 = int.Parse(s[1]);
                        result = result.Where(e => e.price >= x & e.price <= x2);
                        break;
                }
            }

            return result;
        }



        public override void SelectRow(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count > 0)
            {
                this.row = new Products(
                    (e.AddedCells[0].Item as Products).Id,
                    (e.AddedCells[0].Item as Products).goodName,
                    (e.AddedCells[0].Item as Products).weight,
                    (e.AddedCells[0].Item as Products).price
                    );
                RowChoiceNotify?.Invoke(row);
            }
        }

    }

    /// <summary>
    /// редактирование таблицы Products
    /// </summary>
    public class ProductsEditAWM: EditAWM
    {

        public ProductsEditAWM(TableAVM main, string query) :
            base(main)
        {
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
            Columns.Add(new Cell("Товар"));
            Columns.Add(new Cell("Вес"));
            Columns.Add(new Cell("Цена"));

        }

        public override void UpdateView()
        {
            Columns.Add(new Cell("Id"));
            Columns.Add(new Cell("Товар"));
            Columns.Add(new Cell("Вес"));
            Columns.Add(new Cell("Цена"));

        }

        public override void DeleteView()
        {
            Columns.Add(new Cell("Id"));
            Columns.Add(new Cell("Товар"));
            Columns.Add(new Cell("Вес"));
            Columns.Add(new Cell("Цена"));

        }


        public override void InsertDataToDB()
        {
            int w; 
            int p;
            if (int.TryParse(Columns[1].Value, out w))
                if (int.TryParse(Columns[2].Value, out p))
                {
                    Products pr = new Products
                    {
                        goodName = Columns[0].Value,
                        weight = w,
                        price = p
                    };

                    Main.Context.Products.Add(pr);
                    Main.Context.SaveChanges();
                }
        }

        
        public void AddDataToUpdate(Row row)
        {
            Products pRow = row as Products;
            Columns[0].Value = pRow.Id.ToString();
            Columns[1].Value = pRow.goodName;
            Columns[2].Value = pRow.weight.ToString();
            Columns[3].Value = pRow.price.ToString();

        }

        public override void AddDataToDelete(Row row)
        {
            Products pRow = row as Products;
            Columns[0].Value = pRow.Id.ToString();
            Columns[1].Value = pRow.goodName;
            Columns[2].Value = pRow.weight.ToString();
            Columns[3].Value = pRow.price.ToString();
        }

        public override void DeleteDataFromDB()
        {
            int id;
            if (int.TryParse(Columns[0].Value, out id))
            {
                Products pToDelete = Main.Context.Products.Find(id);
                if (pToDelete != null)
                {
                    Main.Context.Products.Remove(pToDelete);
                    Main.Context.SaveChanges();
                    ClearView();
                }
            }


        }

        public override void UpdateDataToDB()
        {
            int id; 
            int w;
            int p;
            if (int.TryParse(Columns[0].Value, out id))
                if (int.TryParse(Columns[1].Value, out w))
                    if (int.TryParse(Columns[2].Value, out p))
                    {

                        Products pToUpdate = Main.Context.Products.Find(id);
                        if (pToUpdate != null)
                        {
                            pToUpdate.goodName = Columns[1].Value;
                            pToUpdate.weight = w;
                            pToUpdate.price = p;

                            Main.Context.SaveChanges();
                        }
                    }
        }

    }
}
