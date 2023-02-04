using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace M17_Task31.VM
{

    public class User 
    {
        public string userName { get; set; }

        public List<string> roles { get; set; }

        

        public User() 
        {
            userName = "";
            roles = new List<string>();
            
        }

    }
    public class UserVM : INotifyPropertyChanged
    {

        protected User user;                             // пользователь 
        
        protected MyFirstDBEntities context;             // контекст для бд

        ObservableCollection<string> tables;             // список таблиц

        /// <summary>
        /// название таблиц для переключения
        /// </summary>
        public ObservableCollection<string> BObjects { get { return tables; } }

        public UserVM(User user, MyFirstDBEntities context)
        {
            this.user = user;
            this.context = context;
            tables = new ObservableCollection<string>() { "Buyers", "Products" };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        

    }
}
