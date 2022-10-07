using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static M17_Task1.SQLConnectionAVM;

namespace M17_Task1
{
  


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLConnectionAVM firstAvm;
        SQLConnectionAVM secondAvm;
        SQLConnectionAVM thirdAvm;
        OleConnectionAVM fourthAvm;
        OleConnectionAVM fifthAvm;


        public MainWindow()
        {
            InitializeComponent();

            string con1 = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\M17_approach\M17_Acc_Connection\bin\x64\Debug\db\Database1.accdb";
            string con2 = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\source\repos\M17_approach\M17_Acc_Connection\bin\x64\Debug\db\Database21.accdb; Jet OLEDB:Database Password = 123456";

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

            firstAvm = new SQLConnectionAVM("User1", new SqlConnection() { ConnectionString = strCon1.ConnectionString });
            first.DataContext = firstAvm;
            secondAvm = new SQLConnectionAVM("User2", new SqlConnection() { ConnectionString = strCon2.ConnectionString });
            second.DataContext = secondAvm;
            thirdAvm = new SQLConnectionAVM("User3", new SqlConnection() { ConnectionString = strCon3.ConnectionString });
            third.DataContext = thirdAvm;

            fourthAvm = new OleConnectionAVM("Database1", new OleDbConnection() { ConnectionString = con1 });
            fourth.DataContext = fourthAvm;
            fifthAvm = new OleConnectionAVM("Database21", new OleDbConnection() { ConnectionString = con2 });
            fifth.DataContext = fifthAvm;
        }

        private async void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true) 
                await TryConnection(firstAvm.Connection);
            if (checkBox2.IsChecked == true) 
                await TryConnection(secondAvm.Connection);
            if (checkBox3.IsChecked == true) 
                await TryConnection(thirdAvm.Connection);
            if (checkBox4.IsChecked == true) 
                await TryConnection(fourthAvm.Connection);
            if (checkBox5.IsChecked == true) 
                await TryConnection(fifthAvm.Connection);

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true) firstAvm.Connection.Close();
            if (checkBox2.IsChecked == true) secondAvm.Connection.Close(); 
            if (checkBox3.IsChecked == true) thirdAvm.Connection.Close();
            if (checkBox4.IsChecked == true) fourthAvm.Connection.Close();
            if (checkBox5.IsChecked == true) fifthAvm.Connection.Close(); 

        }

        public async Task TryConnection(object connection) 
        {

            try 
            {
                if(connection.GetType() == typeof(SqlConnection))
                    await (connection as SqlConnection).OpenAsync();
                if (connection.GetType() == typeof(OleDbConnection))
                    await (connection as OleDbConnection).OpenAsync();
                

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message); 
            }
           

        }

        

    }
}
