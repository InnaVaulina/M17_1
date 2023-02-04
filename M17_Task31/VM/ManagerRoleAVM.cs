using M17_Task31;
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
using M17_Task31.Command;
using M17_Task31.VM.Obj;

namespace M17_Task31.VM
{

    public class ManagerRoleVM : UserVM
    {
        ManagerWindow window;                  // окно
                                               
        //SellerWindow sellerWindow;
        //SellerRoleAVM sellerAVM;


        TableEditAVM workTable;                // таблица в работе
        TableEditAVM bTable;
        TableEditAVM pTable;


        //string newLogin;
        //string newPassword;

        public User Manager { get { return base.user; } }

        /// <summary>
        /// отображаемая таблица
        /// </summary>
        public TableEditAVM WorkTable
        {
            get { return workTable; }
            set { workTable = value; OnPropertyChanged("WorkTable"); }
        }



        WeirdCommand exit;   // выход
        public WeirdCommand Exit { get { return exit; } }


        public ManagerRoleVM(User user, MyFirstDBEntities context, ManagerWindow window) :
            base(user, context)
        {

            this.window = window;           
            
            // модели для уплавлеия таблицами
            bTable = new TableEditAVM(context,"Buyers");  
            pTable = new TableEditAVM(context,"Products");  
            // подключить модель таблицы для отображения
            
            workTable = bTable;
            // смена отображаемой таблицы
            this.window.objList.SelectionChanged += WorkTableChange;
            // подключить обработчик выбора условия для выборки данных
            //this.window.dataRequest.SelectedCellsChanged += WorkTable.Select.AddNewCondition;
            // подключить обработчик выбара строки в таблице
            this.window.SelectedNotify += WorkTable.Show.SelectRow;
            

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
            //this.window.dataRequest.SelectedCellsChanged -= WorkTable.Select.AddNewCondition;
            this.window.SelectedNotify -= WorkTable.Show.SelectRow;

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
            //this.window.dataRequest.SelectedCellsChanged += WorkTable.Select.AddNewCondition;
            this.window.SelectedNotify += WorkTable.Show.SelectRow;
        }
    }
}
