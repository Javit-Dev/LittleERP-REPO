using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class User
    {

        public int iduser { get; set; }
        public String name { get; set; }
        public String password { get; set; }

        public Manage.UserManage manage { get; set; }

        public List<Permission> Permissions { get; set; }

        public User() {
            manage = new Manage.UserManage();
            Permissions = new List<Permission>();
        }

        public User(int idUser) {
            manage = new Manage.UserManage();
            this.iduser = idUser;
            Permissions = new List<Permission>();
        }

        public void insert(List<Role> roles)
        {
            manage.insertUser(this,roles);
        }

        public Boolean check() {
            return manage.checkUser(this);
        }

        public Boolean checkName() {
            return manage.checkUserName(this);
        }

        public void readAll()
        {
            manage.readAll();
        }

        public void readAllMinus() {
            manage.readAllMinus(this);
        }
        public void delete()
        {
            manage.deleteUser(this);
        }
        public void readUser()
        {
            manage.readUser(this);
        }

        public void readUserId() {
            manage.readUserId(this);
        }


        public void changePassword() {
            manage.changePassword(this);
        }

        public void changeUser(List<Role> roles) {
            manage.changeUser(this, roles);
        }

        public void chargePrivileges() {
            manage.chargePrivileges(this);
        }

        public Boolean isFirstSession() {
            return manage.isFirstSession(this);
        }

        public void markFirstSession(Boolean first) {
            manage.markFirstSession(this,first);
        }

        public void readSelectUsers(String name) {
            manage.readSelectUsers(this, name);
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   iduser == user.iduser;
        }
    }
}
