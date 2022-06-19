using LittleERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Role
    {
        public int idrole { get; set; }
        public String name { get; set; }

        public RoleManage manage { get; set; }

        public Role() {
            manage = new RoleManage();
        }

        public Role(int idrole, String name)
        {
            manage = new RoleManage();
            this.idrole = idrole;
            this.name = name;
        }

        public void readAll()
        {
            manage.readAll();
        }

        public void readRoles(int idUser) {
            manage.readRoles(idUser);
        }

        public void changeRole(List<Permission> permissions) {
            manage.changeRole(this,permissions);
        }

        public override bool Equals(object obj)
        {
            return obj is Role role &&
                   idrole == role.idrole;
        }
    }
}
