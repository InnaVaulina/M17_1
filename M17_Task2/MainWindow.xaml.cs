using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using M17Library1.AVM;


namespace M17_Task2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TableAVM workTable;


        public MainWindow()
        {
            InitializeComponent();

            SqlConnectionStringBuilder strCon1 = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = $@"C:\Users\user\source\repos\M17_approach\M17_SQL_Connection\bin\Debug\db\MyFirstDB.mdf",
                IntegratedSecurity = true,
                Pooling = true
            };

            workTable = new TableEditAVM(strCon1, "Buyers");
            //workTable = new TableEditAVM(strCon1, "Products");
            workTableShow.DataContext = workTable;
            dataRequest.SelectedCellsChanged += workTable.Select.AddNewCondition;


        }


    }
}
