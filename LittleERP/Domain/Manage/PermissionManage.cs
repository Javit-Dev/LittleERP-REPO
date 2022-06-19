using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class PermissionManage
    {
        public List<Permission> permissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionManage"/> class.
        /// </summary>
        public PermissionManage()
        {
            permissions = new List<Permission>();
        }

        /// <summary>
        /// Retrieves all permissions from the database.
        /// </summary>
        public void readAll() {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDPERMISSION, DESCRIPTION FROM PERMISSIONS","PERMISSIONS");
            DataTable table = data.Tables["PERMISSIONS"];

            Permission aux;

            foreach (DataRow row in table.Rows) {
                aux = new Permission();
                aux.idpermission=Convert.ToInt32(row["IDPERMISSION"]);
                aux.description = Convert.ToString(row["DESCRIPTION"]);
                permissions.Add(aux);
            }

        }

        /// <summary>
        /// Retrieves all permissions from the database given a role by parameter.
        /// </summary>
        /// <param name="role">The role.</param>
        public void readPermissions(Role role) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT P.IDPERMISSION, P.DESCRIPTION FROM PERMISSIONS P INNER JOIN ROLES_PERMISSIONS RP ON RP.IDPERMISSION = P.IDPERMISSION WHERE RP.IDROLE="+role.idrole, "PERMISSIONS");
            DataTable table = data.Tables["PERMISSIONS"];

            Permission aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Permission();
                aux.idpermission = Convert.ToInt32(row["IDPERMISSION"]);
                aux.description = Convert.ToString(row["DESCRIPTION"]);
                permissions.Add(aux);
            }
        }

    }
}
