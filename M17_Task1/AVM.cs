using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace M17_Task1
{
    
    /// <summary>
    /// показывает подключение
    /// </summary>
    public class ConnectionAVM : INotifyPropertyChanged
    {
        protected string connectionName;
        protected string state;
        protected string connectionString;

        /// <summary>
        /// имя пользователя
        /// </summary>
        public string ConnectionName { get { return connectionName; } set { connectionName = value; } }
        
        /// <summary>
        /// состояние подключения
        /// </summary>
        public string State 
        { 
            get { return state; } 
            set { state = value; OnPropertyChanged("State"); } 
        }

        /// <summary>
        /// строка подключения
        /// </summary>
        public string ConnectionString { get { return connectionString; } }

        public ConnectionAVM()
        {
            this.ConnectionName = "";
            this.state = "";
            this.connectionString = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }

    /// <summary>
    /// показывает подключение к SQL Server
    /// </summary>
    public class SQLConnectionAVM : ConnectionAVM
    {
        SqlConnection connection;

        /// <summary>
        /// подключение
        /// </summary>
        public SqlConnection Connection { get { return connection; } }

        public SQLConnectionAVM(SqlConnection connection)
        {
            this.connection = connection;
            

            string username = "";

            try
            {
                using (SqlConnection c = new SqlConnection(connection.ConnectionString))
                {
                    string sql = $@"SELECT CURRENT_USER;";

                    c.Open();
                    SqlCommand command = new SqlCommand(sql, c);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        username = reader.GetValue(0).ToString();
                    }

                    reader.Close();

                    c.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                base.ConnectionName = username;
                base.State = $"{connection.State}";
                base.connectionString = connection.ConnectionString;
            }

            

            this.Connection.StateChange +=
               (s, e) =>
               {
                   base.State = $"{(s as SqlConnection).State}";
               };

        }
    }

    /// <summary>
    /// показывает подключение OleDb
    /// </summary>
    public class OleConnectionAVM : ConnectionAVM
        {
        OleDbConnection connection;


        /// <summary>
        /// подключение
        /// </summary>
        public OleDbConnection Connection { get { return connection; } }
        
        public OleConnectionAVM(string name, OleDbConnection connection)
            {
            this.connection = connection;
            base.ConnectionName=name;   
            base.State = $"{connection.State}";
            base.connectionString = connection.ConnectionString;

                this.Connection.StateChange +=
                   (s, e) =>
                   {
                       base.State = $"{(s as OleDbConnection).State}";                      
                   };

            }



        }
    
}
