using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using M17Library1.AVM;
using M17Library1.AVM.Command;
using M17Library1.AVM.DBObject;
using M17Library1.AVM.Interfaces;


namespace M17_task21.AVM
{
    public class UserRoleAVM: INotifyPropertyChanged
    {
        protected User user;                             // пользователь в роли продавец
        protected SqlConnectionStringBuilder strCon;     // строка подключения

        ObservableCollection<string> tables;             // список таблиц

        /// <summary>
        /// название таблиц для переключения
        /// </summary>
        public ObservableCollection<string> BObjects { get { return tables; } }

        public UserRoleAVM(User user, SqlConnectionStringBuilder strCon) 
        {
            this.user = user;
            this.strCon = strCon;
            tables = new ObservableCollection<string>();
            

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }




}
