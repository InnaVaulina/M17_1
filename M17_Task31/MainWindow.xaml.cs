using M17_Task31.VM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
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
using System.Xml.Linq;



namespace M17_Task31
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        ManagerWindow managerWindow;
        //SellerWindow sellerWindow;

        User user;
        //SellerRoleAVM sellerAVM;
        ManagerRoleVM managerAVM;


        public MainWindow()
        {
    
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyFirstDBEntities context = new MyFirstDBEntities();

            
            user = new User();

            // тут нужно встретить пользователя
            //...
            //

            managerWindow = new ManagerWindow();
            //managerWindow.Title = $"{user.userName} - управляющий";
            managerAVM = new ManagerRoleVM(user, context, managerWindow);
            managerWindow.managerFunctions.DataContext = managerAVM;
            managerWindow.ElderWindow = this;
            managerWindow.Show();
            this.Hide();

        }
    }
}
