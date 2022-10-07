using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M17_Acc_Connection;
using M17_SQL_Connection;


namespace M17_union
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string path1 = $@"C:\Users\user\source\repos\M17_approach\M17_SQL_Connection\bin\Debug\db\MyFirstDB.mdf";


            SqlConnectionStringBuilder strCon1 = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = path1,
                IntegratedSecurity = false,
                Pooling = false,
                UserID = "TestLogin",
                Password = "0000*"
            };



            string con2 = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\M17_approach\M17_Acc_Connection\bin\x64\Debug\db\Database21.accdb; Jet OLEDB:Database Password = 123456";




            Task[] tasks = new Task[2];


            tasks[0] = M17_SQL_Connection.Program.TryConnectionSQL(strCon1);
            tasks[1] = M17_Acc_Connection.Program.TryConnectionAccess(con2);



            Task.WaitAll(tasks);

            Console.WriteLine("Задача выполнена.");
            Console.ReadLine();

        }
    }
}
