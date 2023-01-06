using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using System.IO;
using M17Library1.AVM;
using M17Library1.AVM.DBObject;
using M17_task21.AVM;


namespace M17_task21
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string path1 = $@"C:\Users\user\source\repos\M17_approach\M17_SQL_Connection\bin\Debug\db\MyFirstDB.mdf";

        SqlConnectionStringBuilder strCon1 = new SqlConnectionStringBuilder()
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            AttachDBFilename = $@"C:\Users\user\source\repos\M17_approach\M17_SQL_Connection\bin\Debug\db\MyFirstDB.mdf",
            IntegratedSecurity = false,
            Pooling = false,
            //UserID = "TestLogin",
            //Password = "0000*"

        };

        

        ManagerWindow managerWindow;
        SellerWindow sellerWindow;

        User user;
        SellerRoleAVM sellerAVM;
        ManagerRoleAVM managerAVM;

        public MainWindow()
        {
            InitializeComponent();
   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            strCon1.UserID = login.Text;
            strCon1.Password = password.Password;

            //strCon1.UserID = "TestLogin";
            //strCon1.Password = "0000*";

            //strCon1.UserID = "Robert2";
            //strCon1.Password = "0000@2";

            //strCon1.UserID = "Robert3";
            //strCon1.Password = "0000@3";

            //strCon1.UserID = "Maria1";
            //strCon1.Password = "000@1";

            //strCon1.UserID = "Maria2";
            //strCon1.Password = "0000@1";

            if (login.Text == "") MessageBox.Show($"Имя пользователя не может быть пустым");
            else 
            {
                try
                {
                    using (SqlConnection c = new SqlConnection(strCon1.ConnectionString))
                    {
                        string sql = $@"SELECT CURRENT_USER;";
                        string sql3 = $@"SELECT perms.state_desc AS State, " +
                                        "permission_name AS[Permission], " +
                                        "obj.name AS[on Object], " +
                                        "dp.name AS[to Role Name] " +
                                        "FROM sys.database_permissions AS perms " +
                                        "JOIN sys.database_principals AS dp " +
                                        "ON perms.grantee_principal_id = dp.principal_id " +
                                        "JOIN sys.objects AS obj " +
                                        "ON perms.major_id = obj.object_id;";


                        string sql2 = $@"SELECT DISTINCT " +
                                        "dp.name AS[to Role Name] " +
                                        "FROM sys.database_permissions AS perms " +
                                        "JOIN sys.database_principals AS dp " +
                                        "ON perms.grantee_principal_id = dp.principal_id " +
                                        "JOIN sys.objects AS obj " +
                                        "ON perms.major_id = obj.object_id;";






                        c.Open();
                        SqlCommand command1 = new SqlCommand(sql, c);
                        SqlCommand command2 = new SqlCommand(sql2, c);
                        SqlCommand command3 = new SqlCommand(sql3, c);


                        SqlDataReader reader = command1.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            user = new User(strCon1.UserID, reader.GetValue(0).ToString());
                        }
                        reader.Close();


                        reader = command2.ExecuteReader();
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                user.Roles.Add(new Role(reader.GetValue(0).ToString()));
                            }
                        reader.Close();


                        reader = command3.ExecuteReader();
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                foreach (Role r in user.Roles)
                                    if (reader.GetValue(3).ToString() == r.RoleName)
                                        r.Permissions.Add(new Permission(reader.GetValue(0).ToString(),
                                            reader.GetValue(1).ToString(),
                                            reader.GetValue(2).ToString()));
                            }
                        reader.Close();

                        c.Close();

                        //__________________________

                        if (user.Roles.Count == 1)
                        {
                            if (user.Roles[0].RoleName == "seller")
                            {
                                sellerWindow = new SellerWindow();
                                sellerWindow.Title = $"{user.UserName} - продавец";
                                sellerAVM = new SellerRoleAVM(user, strCon1, sellerWindow);
                                sellerWindow.sellerFunctions.DataContext = sellerAVM;
                                sellerWindow.ElderWindow = this;
                                sellerWindow.Show();
                                this.Hide();
                            }
                        }
                        if (user.Roles.Count > 1)
                        {
                            foreach (Role r in user.Roles)
                            {
                                if (r.RoleName == "manager")
                                {
                                    managerWindow = new ManagerWindow();
                                    managerWindow.Title = $"{user.UserName} - управляющий";
                                    managerAVM = new ManagerRoleAVM(user, strCon1, managerWindow);
                                    managerWindow.managerFunctions.DataContext = managerAVM;
                                    managerWindow.ElderWindow = this;
                                    managerWindow.Show();
                                    this.Hide();
                                    break;
                                }
                            }

                        }



                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

    }
}


