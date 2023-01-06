using M17Library1.AVM;
using M17Library1.AVM.DBObject;
using M17Library1.AVM.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace M17_task21.AVM
{

    public class ManagerRoleAVM : UserRoleAVM
    {
        ManagerWindow window;                  // окно
        NewUserCreateWindow createUserWindow;
        
        
        SellerWindow sellerWindow;
        SellerRoleAVM sellerAVM;


        TableEditAVM workTable;                // таблица в работе
        TableEditAVM bTable;
        TableEditAVM pTable;


        string newLogin;
        string newPassword;

        public User Manager { get { return base.user; } }

        public TableEditAVM WorkTable 
        { 
            get { return workTable; } 
            set { workTable = value; OnPropertyChanged("WorkTable"); } 
        }


        /// <summary>
        /// имя (логин) нового пользователя
        /// </summary>
        public string NewLogin
        {
            get { return newLogin; }
            set { newLogin = value; OnPropertyChanged("NewLogin"); }
        }

        /// <summary>
        /// пароль для нового пользователя
        /// </summary>
        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged("NewPassword"); }
        }


        WeirdCommand exit;   // выход
        public WeirdCommand Exit { get { return exit; } }

        WeirdCommand showSelerRole;   // изменить роль на продавца
        public WeirdCommand ShowSelerRole { get { return showSelerRole; } }

        WeirdCommand makeUserWindowShow;   // добавление нового пользователя
        public WeirdCommand MakeUserWindowShow { get { return makeUserWindowShow; } }

        WeirdCommand makeNewSeller;   // добавить продавца
        public WeirdCommand MakeNewSeller { get { return makeNewSeller; } }

        WeirdCommand makeNewManager;   // добавить продавца
        public WeirdCommand MakeNewManager { get { return makeNewManager; } }


        public ManagerRoleAVM(User user, SqlConnectionStringBuilder strCon, ManagerWindow window) :
            base(user, strCon)
        {
            this.createUserWindow = null;
            this.newLogin = "";
            this.newPassword = "";

            this.window = window;           
            // типа выбрать список таблиц из user.Roles
            List<string> tables1 = new List<string>();
            foreach (Role r in user.Roles)
                if (r.RoleName == "manager")
                {
                    foreach (Permission p in r.Permissions) tables1.Add(p.BObject);
                    List<string> tables2 = tables1.Distinct().ToList();
                    foreach (string s in tables2) BObjects.Add(s);
                    break;
                }

            // модели для уплавлеия таблицами
            bTable = new TableEditAVM(strCon, BObjects[0]);  // tables[0] == Buyers
            pTable = new TableEditAVM(strCon, BObjects[1]);  // tables[1] == Products
            // подключить модель таблицы для отображения
            
            workTable = bTable;
            // смена отображаемой таблицы
            this.window.objList.SelectionChanged += WorkTableChange;
            // подключить обработчик выбора условия для выборки данных
            this.window.dataRequest.SelectedCellsChanged += WorkTable.Select.AddNewCondition;
            // подключить обработчик выбара строки в таблице
            this.window.dataShow.SelectedCellsChanged += WorkTable.Show.SelectRow;

            makeUserWindowShow = new WeirdCommand(o => 
            {
                this.createUserWindow = new NewUserCreateWindow();
                createUserWindow.DataContext = this;
                createUserWindow.Show();
            });

            makeNewSeller = new WeirdCommand(o => { AddSeller(NewLogin, NewPassword); });

            makeNewManager = new WeirdCommand(o => { AddManager(NewLogin, NewPassword); });

            showSelerRole = new WeirdCommand(o => 
            {
                sellerWindow = new SellerWindow();
                sellerWindow.Title = $"{user.UserName} - продавец";
                sellerAVM = new SellerRoleAVM(user, strCon, sellerWindow);
                sellerWindow.sellerFunctions.DataContext = sellerAVM;
                sellerWindow.ElderWindow = this.window;
                sellerWindow.Show();
                this.window.Hide();
            });

            exit = new WeirdCommand(o => 
            {
                this.window.ElderWindow.Show();
                this.window.Close();
            });

        }

        /// <summary>
        /// смена таблиц
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void WorkTableChange(object sender, SelectionChangedEventArgs args)
        {

            // отключить обработчики событий для сменяемой теблицы
            this.window.dataRequest.SelectedCellsChanged -= WorkTable.Select.AddNewCondition;
            this.window.dataShow.SelectedCellsChanged -= WorkTable.Show.SelectRow;

            // подключить другую таблицу
            switch (args.AddedItems[0].ToString())
            {
                case "Buyers":
                    WorkTable = bTable;
                    break;
                case "Products":
                    WorkTable = pTable;
                    break;

            }
            // подключить обработчики событий для текущей теблицы
            this.window.dataRequest.SelectedCellsChanged += WorkTable.Select.AddNewCondition;
            this.window.dataShow.SelectedCellsChanged += WorkTable.Show.SelectRow;

        }


        /// <summary>
        /// добавить нового пользователя с ролью продавец
        /// </summary>
        /// <param name="newLogin"></param>
        /// <param name="newPassword"></param>
        public void AddSeller(string newLogin, string newPassword)
        {
            string connectiontoMasterString = "Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

            string sql1 = $@"EXECUTE AS LOGIN = '{this.Manager.Login}';";
            string sql2 = $@"CREATE LOGIN [{newLogin}] WITH PASSWORD = '{newPassword}';";

            // войти в базу master, создать логин с парелем
            try
            {
                using (SqlConnection c = new SqlConnection(connectiontoMasterString))
                {

                    c.Open();
                    SqlCommand cmd1 = new SqlCommand(sql1, c);
                    SqlCommand cmd2 = new SqlCommand(sql2, c);
                    
                    
                    SqlTransaction tx = null;
                    try
                    {
                        tx = c.BeginTransaction();
                        cmd1.Transaction = tx;
                        cmd2.Transaction = tx;
                      
                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();

                        tx.Commit();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        tx?.Rollback();

                    }

                    c.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            string sql3 = $@"CREATE USER [{newLogin}] FOR LOGIN [{newLogin}];";
            string sql4 = $@"ALTER ROLE seller ADD member [{newLogin}];";

            // войти в базу myfirstdb, создать пользователя для логина
            try
            {
                using (SqlConnection c = this.workTable.Set())
                {

                    c.Open();
                    SqlCommand cmd3 = new SqlCommand(sql3, c);
                    SqlCommand cmd4 = new SqlCommand(sql4, c);


                    SqlTransaction tx = null;
                    try
                    {
                        tx = c.BeginTransaction();
                        cmd3.Transaction = tx;
                        cmd4.Transaction = tx;
                        cmd3.ExecuteNonQuery();
                        cmd4.ExecuteNonQuery();

                        tx.Commit();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        tx?.Rollback();

                    }


                    c.Close();
                    MessageBox.Show($"Создан пользователь {newLogin}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public void AddManager(string newLogin, string newPassword)
        {
            string connectiontoMasterString = "Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

            string sql1 = $@"EXECUTE AS LOGIN = '{this.Manager.Login}';";
            string sql2 = $@"CREATE LOGIN [{newLogin}] WITH PASSWORD = '{newPassword}';";
            string sql3 = $@"ALTER SERVER ROLE manager_myfirstdb ADD MEMBER [{newLogin}]";

            // войти в базу master, создать логин с парелем
            try
            {
                using (SqlConnection c = new SqlConnection(connectiontoMasterString))
                {

                    c.Open();
                    SqlCommand cmd1 = new SqlCommand(sql1, c);
                    SqlCommand cmd2 = new SqlCommand(sql2, c);
                    SqlCommand cmd3 = new SqlCommand(sql3, c);

                    SqlTransaction tx = null;
                    try
                    {
                        tx = c.BeginTransaction();

                        cmd1.Transaction = tx;
                        cmd2.Transaction = tx;
                        cmd3.Transaction = tx;

                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        cmd3.ExecuteNonQuery();

                        tx.Commit();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        tx?.Rollback();

                    }

                    c.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            string sql4 = $@"CREATE USER [{newLogin}] FOR LOGIN [{newLogin}];";
            string sql5 = $@"ALTER ROLE manager ADD MEMBER [{newLogin}];";
            string sql6 = $@"GRANT ALTER ON ROLE::manager TO [{newLogin}] WITH GRANT OPTION;";
            //string sql7 = $@"ALTER ROLE db_accessadmin ADD MEMBER [{newLogin}];";

            // войти в базу myfirstdb, создать пользователя для логина
            try
            {
                using (SqlConnection c = this.workTable.Set())
                {

                    c.Open();
                    SqlCommand cmd4 = new SqlCommand(sql4, c);
                    SqlCommand cmd5 = new SqlCommand(sql5, c);
                    SqlCommand cmd6 = new SqlCommand(sql6, c);
                    //SqlCommand cmd7 = new SqlCommand(sql7, c);

                    SqlTransaction tx = null;
                    try
                    {
                        tx = c.BeginTransaction();

                        cmd4.Transaction = tx;
                        cmd5.Transaction = tx;
                        cmd6.Transaction = tx;
                        //cmd7.Transaction = tx;

                        cmd4.ExecuteNonQuery();
                        cmd5.ExecuteNonQuery();
                        cmd6.ExecuteNonQuery();
                        //cmd7.ExecuteNonQuery();

                        tx.Commit();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        tx?.Rollback();

                    }


                    c.Close();
                    MessageBox.Show($"Создан пользователь {newLogin}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
