using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using M17_Task31.Command;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.ComponentModel;
using M17_Task31.VM.Delegates;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace M17_Task31.VM.Obj.DBObject
{



    /// <summary>
    /// отображение таблицы Buyers
    /// </summary>
    public class BuyersShowTableAVM: ShowTableAVM 
    {
        BindingList<Buyers> dt;   // таблица

        IQueryable<Buyers> queryGeneral;
        IQueryable<Buyers> queryPart;
        int keyMaxValue;                   // левый ключ для навигации по таблице
        int keyMinValue;                   // правый ключ для навигации по таблице

        Buyers row;  // выбранный элемент таблицы

        public event RowChoiceHendler RowChoiceNotify;  // перебросить выбранный элемент

        

        public BindingList<Buyers> Dt
        {
            get { return dt; }
            set { dt = value; OnPropertyChanged("Dt"); }
        }

        public Buyers Row { get { return row; } }

        /// <summary>
        /// преобразование запроса в таблицу
        /// </summary>
        /// <returns></returns>
        public BindingList<Buyers> NewDt() 
        {
            var d = new BindingList<Buyers>();
            foreach (var r in queryPart.ToList())
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



        public BuyersShowTableAVM(TableAVM main): base(main)
        {
            row = new Buyers();
            Dt = null;
            
        }

        public BuyersShowTableAVM(TableEditAVM main) : base(main)
        {
            row = new Buyers();
            Dt = null;
        }

        /// <summary>
        /// выполняет запрос, возвращает часть данных из начала таблицы 
        /// </summary>
        public override void NewTableView() 
        {
            
            queryGeneral = base.Main.Context.Buyers.MyQuery(this);
            queryPart = queryGeneral.OrderBy(e => e.Id).Take(5);
            queryPart.Load();
            Dt = NewDt();

        }

        /// <summary>
        /// добавляет в основной запрос условие для выборки данных с Id меньше, чем текущие
        /// </summary>
        public override void AddMinCondition()
        {
            queryPart = queryGeneral.Where(e => e.Id < keyMinValue).OrderByDescending(e => e.Id).Take(5).OrderBy(e => e.Id);
            if (queryPart.Count() == 0)
                queryPart = queryGeneral.OrderBy(e => e.Id).Take(5);
            queryPart.Load();
            Dt = NewDt();
        }

        /// <summary>
        /// добавляет в основной запрос условие для выборки данных с Id больше, чем текущие
        /// </summary>
        public override void AddMaxCondition()
        {
            queryPart = queryGeneral.Where(e => e.Id > keyMaxValue).Take(5);
            if (queryPart.Count() == 0)
                queryPart = queryGeneral.OrderByDescending(e => e.Id).Take(5).OrderBy(e => e.Id);
            queryPart.Load();
            Dt = NewDt();
        }

        /// <summary>
        /// смещение в начало таблицы
        /// </summary>
        public override void StartQuery()
        {
            queryPart = queryGeneral.OrderBy(e => e.Id).Take(5);
            queryPart.Load();
            Dt = NewDt();
        }

        /// <summary>
        /// смещение в конец таблицы
        /// </summary>
        public override void FinishQuery()
        {
            queryPart = queryGeneral.OrderByDescending(e => e.Id).Take(5).OrderBy(e => e.Id);
            queryPart.Load();
            Dt = NewDt();
        }

        

        /// <summary>
        /// обрабатывает пользовательские условия
        /// </summary>
        /// <param name="before"></param>
        /// <returns></returns>
        public IQueryable<Buyers> Query(IQueryable<Buyers> before) 
        {
            IQueryable<Buyers> result = before;

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

            // familyName
            if (ConditionList[1].Compare != "" & ConditionList[1].Value != "")
            {
                string x = ConditionList[1].Value;
                switch (ConditionList[1].Compare)
                {
                    case "=":
                        result = result.Where(e => e.familyName == x);
                        break;
                    case "начинается с":
                        result = result.Where(e => e.familyName.StartsWith(x));
                        break;

                }
            }

            // firstName
            if (ConditionList[2].Compare != "" & ConditionList[2].Value != "")
            {
                string x = ConditionList[2].Value;
                switch (ConditionList[2].Compare)
                {
                    case "=":
                        result = result.Where(e => e.firstName == x);
                        break;
                    case "начинается с":
                        result = result.Where(e => e.firstName.StartsWith(x));
                        break;
                }
            }
                

            // patronymic
            if (ConditionList[3].Compare != "" & ConditionList[3].Value != "")
            {
                string x = ConditionList[3].Value;
                switch (ConditionList[3].Compare)
                {
                    case "=":
                        result = result.Where(e => e.patronymic == x);
                        break;
                    case "начинается с":
                        result = result.Where(e => e.patronymic.StartsWith(x));
                        break;
                }
            }

            // phone
            if (ConditionList[4].Compare != "" & ConditionList[4].Value != "")
            {
                string x = ConditionList[4].Value;
                switch (ConditionList[4].Compare)
                {
                    case "=":
                        result = result.Where(e => e.phone == x);
                        break;
                    case "начинается с":
                        result = result.Where(e => e.phone.StartsWith(x));
                        break;
                }
            }

            // email
            if (ConditionList[5].Compare != "" & ConditionList[5].Value != "")
            {
                string x = ConditionList[5].Value;
                string y = ConditionList[5].Compare;
                switch (ConditionList[5].Compare)
                {
                    case "=":
                        result = result.Where(e => e.email == x);
                        break;
                    case "начинается с":
                        result = result.Where(e => e.email.StartsWith(x));
                        break;
                }
            }

            return result;
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
                this.row = new Buyers(
                    (e.AddedCells[0].Item as Buyers).Id,
                    (e.AddedCells[0].Item as Buyers).familyName,
                    (e.AddedCells[0].Item as Buyers).firstName,
                    (e.AddedCells[0].Item as Buyers).patronymic,
                    (e.AddedCells[0].Item as Buyers).phone,
                    (e.AddedCells[0].Item as Buyers).email
                    );
                RowChoiceNotify?.Invoke(row);   // передать выбранный элемент

            }
        }
    }

    /// <summary>
    /// редактирование таблицы Buyers
    /// </summary>
    public class BuyersEditAVM: EditAWM
    {

        public BuyersEditAVM(TableAVM main, string query) :
            base(main)
        {
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
            Columns.Add(new Cell("Фамилия"));
            Columns.Add(new Cell("Имя"));
            Columns.Add(new Cell("Отчетство"));
            Columns.Add(new Cell("Телефон"));
            Columns.Add(new Cell("Email"));
        }

        public override void UpdateView()
        {
            Columns.Add(new Cell("Id"));
            Columns.Add(new Cell("Фамилия"));
            Columns.Add(new Cell("Имя"));
            Columns.Add(new Cell("Отчетство"));
            Columns.Add(new Cell("Телефон"));
            Columns.Add(new Cell("Email"));
        }

        public override void DeleteView()
        {
            Columns.Add(new Cell("Id"));
            Columns.Add(new Cell("Фамилия"));
            Columns.Add(new Cell("Имя"));
            Columns.Add(new Cell("Отчетство"));
            Columns.Add(new Cell("Телефон"));
            Columns.Add(new Cell("Email"));
        }




        public override void InsertDataToDB()
        {
            Buyers buyer = new Buyers {
                familyName = Columns[0].Value,
                firstName = Columns[1].Value,
                patronymic = Columns[2].Value,
                phone = Columns[3].Value,
                email = Columns[4].Value
            };

            Main.Context.Buyers.Add(buyer);
            Main.Context.SaveChanges();
                
        }


        public void AddDataToUpdate(Row row)
        {
            Buyers bRow = row as Buyers;
            Columns[0].Value = bRow.Id.ToString();
            Columns[1].Value = bRow.familyName;
            Columns[2].Value = bRow.firstName;
            Columns[3].Value = bRow.patronymic;
            Columns[4].Value = bRow.phone;
            Columns[5].Value = bRow.email;
            
        }

        public override void AddDataToDelete(Row row) 
        {
            Buyers bRow = row as Buyers;
            Columns[0].Value = bRow.Id.ToString();
            Columns[1].Value = bRow.familyName;
            Columns[2].Value = bRow.firstName;
            Columns[3].Value = bRow.patronymic;
            Columns[4].Value = bRow.phone;
            Columns[5].Value = bRow.email;
        }


        public override void DeleteDataFromDB()
        {
            int id;
            if (int.TryParse(Columns[0].Value, out id))
            {
                Buyers bToDelete = Main.Context.Buyers.Find(id);
                if (bToDelete != null)
                {
                    Main.Context.Buyers.Remove(bToDelete);
                    Main.Context.SaveChanges();
                    ClearView();
                }
            }
                
            
        }

        public override void UpdateDataToDB()
        {
            int id;
            if (int.TryParse(Columns[0].Value, out id)) 
            {
                Buyers bToUpdate = Main.Context.Buyers.Find(id);
                if (bToUpdate != null) 
                {
                    bToUpdate.familyName = Columns[1].Value;
                    bToUpdate.firstName = Columns[2].Value;
                    bToUpdate.patronymic = Columns[3].Value;
                    bToUpdate.phone = Columns[4].Value;
                    bToUpdate.email = Columns[5].Value;
                    Main.Context.SaveChanges();
                }                 
            }
        }
  
    }
}
