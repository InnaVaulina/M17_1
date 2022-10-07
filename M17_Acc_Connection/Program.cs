using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Reflection.Emit;
using System.Data.Common;

namespace M17_Acc_Connection
{
    public class Program
    {

        public static async Task TryConnectionAccess(string connection)
        {
            //DataTable table = new OleDbEnumerator().GetElements();
            //string inf = "";
            //foreach (DataRow row in table.Rows)
            //    inf += row["SOURCES_NAME"] + "\n";
            //Console.WriteLine(inf);

            OleDbConnection accessConnection = new OleDbConnection();                                                                                                  // создание нового подключения
            accessConnection.ConnectionString = connection;

            Console.WriteLine($"Строка подключения: {accessConnection.ConnectionString}");

            accessConnection.StateChange +=
               (s, e) => {
                   Console.WriteLine($@"accDB {accessConnection.Database}_ {nameof(accessConnection)} в состоянии:" +
                   $" {(s as OleDbConnection).State}. Время: {DateTime.Now}");
               };


            try
            {
                await accessConnection.OpenAsync();
                Console.WriteLine($"accDB {accessConnection.Database}_ Имя базы данных: {accessConnection.Database}");
                Console.WriteLine($"accDB {accessConnection.Database}_ Местоположение базы данных: {accessConnection.DataSource}");
                Console.WriteLine($"accDB {accessConnection.Database}_ Сервер: {accessConnection.ServerVersion}");
                Console.WriteLine($"accDB {accessConnection.Database}_ Поставщик OLE DB: {accessConnection.Provider}");


                OleDbCommand commP = accessConnection.CreateCommand();
                commP.CommandText = "SELECT MSysObjects.Name FROM MSysObjects WHERE (((MSysObjects.Type)=1));";
                OleDbDataReader reader = commP.ExecuteReader();

                while (await reader.ReadAsync())
                    Console.WriteLine($"accDB {accessConnection.Database}_ Пользователь базы данных: {reader[0]}");
                
                reader.Close();
                return;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                accessConnection.Close();

            }

        }

        static void Main(string[] args)
        {
            string con1 = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\M17_approach\M17_Acc_Connection\bin\x64\Debug\db\Database1.accdb";
            string con2 = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\M17_approach\M17_Acc_Connection\bin\x64\Debug\db\Database21.accdb; Jet OLEDB:Database Password = 123456";

 
            Task[] tasks = new Task[2];


            tasks[0] = TryConnectionAccess(con1);
            tasks[1] = TryConnectionAccess(con2);



            Task.WaitAll(tasks);

            Console.WriteLine("Задача выполнена.");
            Console.ReadLine();
        }
    }
}
