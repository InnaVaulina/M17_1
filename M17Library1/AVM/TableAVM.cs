using System;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Data;
using M17Library1.AVM.DBObject;
using M17Library1.AVM.Interfaces;

namespace M17Library1.AVM
{
    /// <summary>
    /// модель для интерфейса работы с таблицей бд
    /// IAVMShowSelect<ShowTableAVM> для работы с конкретной таблицей
    /// </summary>
    public class TableAVM: INotifyPropertyChanged, IAVMShowSelect<ShowTableAVM>
    {
        SqlConnectionStringBuilder strCon; // строка подключения
        string tableName;                  // имя таблицы
        string key;                        // PK таблицы
        int keyMaxValue;                   // левый ключ для навигации по таблице
        int keyMinValue;                   // правый ключ для навигации по таблице
        SelectRequest selectQuery;         // запрос селект
        string query;                      // запрос селект для отображения в окне
        ShowTableAVM show;                 // модель таблицы
        RequestAWM select;                 // модель выборки
       


        /// <summary>
        /// содержит структуру, которая собирается в запрос для выборки данных
        /// </summary>
        public SelectRequest SelectQuery { get { return selectQuery; } set { selectQuery = value; } }
        
        /// <summary>
        /// строка подключения
        /// </summary>
        public SqlConnectionStringBuilder StrCon { get { return strCon; } }
        
        /// <summary>
        /// имя таблицы
        /// </summary>
        public string TableName { get { return tableName; } }


        /// <summary>
        /// имя столбца - ключа
        /// </summary>
        public string Key { get { return key; } set { key = value; } }
        
        /// <summary>
        /// ограничивает выборку сверху
        /// </summary>
        public int KeyMaxValue 
        { 
            get { return keyMaxValue; } 
            set { keyMaxValue = value; OnPropertyChanged("KeyMaxValue"); } 
        }
        
        /// <summary>
        /// ограничивает выборку снизу
        /// </summary>
        public int KeyMinValue 
        { 
            get { return keyMinValue; } 
            set { keyMinValue = value; OnPropertyChanged("KeyMinValue"); } 
        }

        /// <summary>
        /// отображение запроса селект
        /// </summary>
        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;
                this.Show.NewTableView();
                OnPropertyChanged("Query"); 
            }
        }


        /// <summary>
        /// отображение таблицы
        /// </summary>
        public ShowTableAVM Show { get { return show; } }
        
        /// <summary>
        /// отображение интерфейса для задания условий выборки данных
        /// </summary>
        public RequestAWM Select { get { return select; } }

        /// <summary>
        /// создать модель для таблицы
        /// </summary>
        /// <param name="strCon">строка состояния</param>
        /// <param name="tableName">имя таблицы</param>
        public TableAVM(SqlConnectionStringBuilder strCon, string tableName)
        {
            this.strCon = strCon;
            this.tableName = tableName;
            this.GetKey();
            selectQuery = new SelectRequest(TableName);
            query = selectQuery.RequestString(); // должно быть объявлено до Select без обновления окна
            show = SelectShowAVM();           
            select = new RequestAWM(this);
            Show.NewTableView();
        }


        /// <summary>
        /// выбор отображения 
        /// </summary>
        /// <returns></returns>
        public ShowTableAVM SelectShowAVM()
        {
            switch (TableName)
            {
                case "Buyers":
                    if(this.GetType() == typeof(TableAVM)) 
                        return new BuyersShowTableAVM(this as TableAVM);
                    if (this.GetType() == typeof(TableEditAVM))
                        return new BuyersShowTableAVM(this as TableEditAVM);
                    break;
                case "Products":
                    if (this.GetType() == typeof(TableAVM))
                        return new ProductsShowTableAVM(this as TableAVM);
                    if (this.GetType() == typeof(TableEditAVM))
                        return new ProductsShowTableAVM(this as TableEditAVM);
                    break;
            }
            return null;
        }


        /// <summary>
        /// определение ключа
        /// </summary>
        public void GetKey()
        {

            try
            {
                using (SqlConnection c = Set())
                {

                    c.Open();
                    string keyQuery = $@"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE" +
                                      $@" WHERE TABLE_NAME = '{TableName}'";

                    SqlCommand command = new SqlCommand(keyQuery, c);
                   
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        this.Key = reader.GetValue(0).ToString();
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


        /// <summary>
        /// выборка N строк таблицы TableName (N = 5)
        /// </summary>
        /// <returns></returns>
        public DataTable MakeTable()
        {
            DataTable dt = null;
            try
            {
                using (SqlConnection c = Set())
                {

                    c.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(Query, c);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, 0, 5, TableName);
                    dt = ds.Tables[0];
                    DataRow row;                    
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        int min = (int)row[Key];
                        int max = (int)row[Key];

                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            row = dt.Rows[i];
                            if (min > (int)row[Key]) min = (int)row[Key];
                            if (max < (int)row[Key]) max = (int)row[Key];
                        }
                        KeyMinValue = min;
                        KeyMaxValue = max;
                    }
                    else dt = null;
                    
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dt = null;
            }
            return dt;
        }


        /// <summary>
        /// установка соединения с бд
        /// </summary>
        /// <returns></returns>
        public SqlConnection Set()
        {
            return new SqlConnection(StrCon.ConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddMinCondition()
        {
            if (KeyMinValue > 0)
            {
                SelectQuery.WhereExp = new Cell(Key, "<", KeyMinValue.ToString());
                SelectQuery.Order = Key;
            }
            else 
            { 
                SelectQuery.WhereExp = null;
                SelectQuery.Order = "";
            }
                     
            Query = SelectQuery.RequestString();
        }

        public void AddMaxCondition()
        {           
            SelectQuery.WhereExp = new Cell(Key, ">", KeyMaxValue.ToString());
            SelectQuery.Order = "";
            Query = SelectQuery.RequestString();
        }

        public void StartQuery() 
        {
            SelectQuery.WhereExp = null;
            selectQuery.Order = "";
            Query = SelectQuery.RequestString();
        }
        public void FinishQuery() 
        {
            SelectQuery.WhereExp = null;
            selectQuery.Order = Key;
            Query = SelectQuery.RequestString();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


 




}
