using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace M17_task21
{
    public class Permission
    {
        string state;
        string perm;
        string bObject;
        public string State { get { return state; } }
        public string Perm { get { return perm; } }
        public string BObject { get { return bObject; } }

        public Permission(string state, string permission, string bObject)
        {
            this.state = state;
            this.perm = permission;
            this.bObject = bObject;
        }
    }

    public class Role
    {

        string roleName;
        ObservableCollection<Permission> permissions;

        public string RoleName { get { return roleName; } }

        public ObservableCollection<Permission>  Permissions
        { 
            get { return permissions; } 
        }

        public Role(string userName)
        {
            this.roleName = userName;
            permissions = new ObservableCollection<Permission>();
        }




    }
    public class User
    {
        string login;
        string userName;
        ObservableCollection<Role> roles;

        public string Login { get { return login; } }
        public string UserName{ get { return userName; } }
        public ObservableCollection<Role> Roles { get { return roles; } }

        public User(string login, string userName)
        {
            this.login = login;
            this.userName = userName;
            this.roles = new ObservableCollection<Role>();
        }

    }
}
