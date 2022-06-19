using LittleERP.Domain.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain
{
    public class Permission
    {
        public int idpermission { get; set; }
        public String description { get; set; }

        public PermissionManage manage { get; set; }


        public Permission() {
            manage = new PermissionManage();
        }

        public Permission(int idpermission, string description)
        {
            this.idpermission = idpermission;
            this.description = description;
            manage = new PermissionManage();
        }

        public void readAll() {
            manage.readAll();
        }

        public void readPermissions(Role role) {
            manage.readPermissions(role);
        }

        public override bool Equals(object obj)
        {
            return obj is Permission permission &&
                   idpermission == permission.idpermission;
        }
    }
}
