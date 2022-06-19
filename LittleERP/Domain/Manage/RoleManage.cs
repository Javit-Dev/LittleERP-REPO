using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class RoleManage
    {
        public List<Role> roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManage"/> class.
        /// </summary>
        public RoleManage() {
            roles = new List<Role>();
        
        }

        /// <summary>
        /// Retrieves all roles from the database.
        /// </summary>
        public void readAll() {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            //Lo guarda como en una vista llamada roles
            data = search.getData("Select idrole,name from Roles", "Roles");

            DataTable table = data.Tables["Roles"];

            Role aux;

            foreach (DataRow row in table.Rows) {
                aux=new Role(Convert.ToInt32(row["IDROLE"]), Convert.ToString(row["NAME"]));
                roles.Add(aux);
            }
        }

        /**
         * Lee los roles de un usuario
         */
        /// <summary>
        /// Retrieves all the roles of a user from the database given a id by parameter.
        /// </summary>
        /// <param name="idUser">The identifier user.</param>
        public void readRoles(int idUser) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("Select R.IDROLE, R.NAME FROM ROLES R INNER JOIN USER_ROLES UR ON UR.IDROLE = R.IDROLE WHERE IDUSER = " + idUser, "Roles");

            DataTable table = data.Tables["Roles"];

            Role aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Role(Convert.ToInt32(row["IDROLE"]), Convert.ToString(row["NAME"]));
                roles.Add(aux);
            }
        }

        /**
         * Cambia los permisos de un rol en base a una lista de permisos
         */

        /// <summary>
        /// Modify the permissions of a role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="permissions">The permissions.</param>
        public void changeRole(Role role, List<Permission> permissions) {
            ConnectOracle search = new ConnectOracle();

            //Elimino los permisos de ese rol
            search.setData("DELETE FROM ROLES_PERMISSIONS WHERE IDROLE="+role.idrole);

            foreach (Permission perm in permissions) {
                int maximum = Convert.ToInt32("0" + search.DLookUp("max(ID_ROLE_PERMISSION)", "ROLES_PERMISSIONS", "")) + 1;
                search.setData("INSERT INTO ROLES_PERMISSIONS(IDPERMISSION,IDROLE,ID_ROLE_PERMISSION) VALUES(" + perm.idpermission + " ," + role.idrole + " ,"+maximum+")");
            }
        }
    }
}
