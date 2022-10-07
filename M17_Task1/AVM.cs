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

namespace M17_Task1
{
    

    public class ConnectionAVM : INotifyPropertyChanged
    {
        protected string connectionName;
        protected string state;
        protected string connectionString;

        public string ConnectionName { get { return connectionName; } set { connectionName = value; } }
        public string State 
        { 
            get { return state; } 
            set { state = value; OnPropertyChanged("State"); } 
        }

        public string ConnectionString { get { return connectionString; } }

        public ConnectionAVM(string name)
        {
            this.ConnectionName = name;
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


    public class SQLConnectionAVM : ConnectionAVM
    {
        SqlConnection connection;

        public SqlConnection Connection { get { return connection; } }

        public SQLConnectionAVM(string name, SqlConnection connection) : base(name)
        {
            this.connection = connection;
            base.State = $"{connection.State}";
            base.connectionString = connection.ConnectionString;

            this.Connection.StateChange +=
               (s, e) =>
               {
                   base.State = $"{(s as SqlConnection).State}";
               };

        }
    }

        public class OleConnectionAVM : ConnectionAVM
        {
            OleDbConnection connection;

            public OleDbConnection Connection { get { return connection; } }
            public OleConnectionAVM(string name, OleDbConnection connection) : base(name)
            {
                this.connection = connection;
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
