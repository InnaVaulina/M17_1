using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Net.WebRequestMethods;

namespace M17_SQL_Connection
{
    public class Program
    {
        public static async Task TryConnectionSQL(SqlConnectionStringBuilder strCon)
        {
            Console.WriteLine($"Строка подключения: {strCon.ConnectionString}");
            //Console.WriteLine($"{strCon.UserID}_ Время: {DateTime.Now}");
            //Console.ReadKey();

            SqlConnection sqlConnection = new SqlConnection()
            { ConnectionString = strCon.ConnectionString };

            sqlConnection.StateChange +=
                (s, e) => {
                    Console.WriteLine($@"{strCon.UserID}_ {nameof(sqlConnection)} в состоянии:" +
                    $" {(s as SqlConnection).State}. Время: {DateTime.Now}");
                };


            //string sql = "SELECT CURRENT_USER";
            string sql = "SELECT USER";
            SqlCommand myCommand = new SqlCommand(sql, sqlConnection);




            try
            {
                await sqlConnection.OpenAsync(); // Открыть соединение с БД 
                Console.WriteLine($"{strCon.UserID}_ Местоположение базы данных: {sqlConnection.DataSource}");
                Console.WriteLine($"{strCon.UserID}_ Сервер: {sqlConnection.ServerVersion}");
                Console.WriteLine($"{strCon.UserID}_ Имя базы данных: {sqlConnection.Database}");
                Console.WriteLine($"{strCon.UserID}_ Клиент базы данных: {sqlConnection.WorkstationId}");
                SqlDataReader myDataReader = myCommand.ExecuteReader();
                while (await myDataReader.ReadAsync())
                    Console.WriteLine($"{strCon.UserID}_ Пользователь базы данных: {myDataReader[0]}");
                myDataReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sqlConnection.Close();// Закрыть соединение с БД Console.WriteLine(sqlConnection.State);

            }
        }

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

            SqlConnectionStringBuilder strCon2 = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = path1,
                IntegratedSecurity = false,
                Pooling = false,
                UserID = "Login2",
                Password = "j$k{rncUxrN.kp&diuz?nbzcmsFT7_&#$!~<vemguK~oYeba"
            };

            SqlConnectionStringBuilder strCon3 = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = path1,
                IntegratedSecurity = false,
                Pooling = false,
                UserID = "Login3",
                Password = "aptkkvbqimfg9chinJi{+xaemsFT7_&#$!~<kl~zemOcnMji"
            };


         


            Task[] tasks = new Task[3];

            tasks[0] = TryConnectionSQL(strCon1);
            tasks[1] = TryConnectionSQL(strCon2);
            tasks[2] = TryConnectionSQL(strCon3);

            Task.WaitAll(tasks);

            Console.WriteLine("Задача выполнена.");
            Console.ReadLine();
        }
    }
}
