using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LittleERP.Domain.Manage
{
    public class UserManage
    {
        public List<User> list { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManage"/> class.
        /// </summary>
        public UserManage()
        {
            list = new List<User>();

        }


        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        public void readAll() {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("Select iduser,name,password from Users where deleted=0","users");

            DataTable table = data.Tables["users"];

            User aux;

            foreach (DataRow row in table.Rows) {
                aux = new User(Convert.ToInt32(row["IDUSER"]));
                aux.name = Convert.ToString(row["NAME"]);
                aux.password = Convert.ToString(row["PASSWORD"]);
                list.Add(aux);
            }
        }

        /**
         * Metodo que lee todos los usuarios menos root y el propio usuario dado por parametro
         */
        /// <summary>
        /// Retrieves all users from the database except the user of the parameter.
        /// </summary>
        /// <param name="u">A user.</param>
        public void readAllMinus(User u) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("Select iduser,name,password from Users where deleted=0 and iduser NOT IN (0,"+u.iduser+")", "users");

            DataTable table = data.Tables["users"];

            User aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new User(Convert.ToInt32(row["IDUSER"]));
                aux.name = Convert.ToString(row["NAME"]);
                aux.password = Convert.ToString(row["PASSWORD"]);
                list.Add(aux);
            }
        }

        /// <summary>
        /// Retrieves a user from the database given a user with an id.
        /// </summary>
        /// <param name="user">The user.</param>
        public void readUser(User user)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("Select * from Users where idUser="+user.iduser, "users");

            DataTable table = data.Tables["users"];

            User aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new User(Convert.ToInt32(row["IDUSER"]));
                aux.name = Convert.ToString(row["NAME"]);
                aux.password = Convert.ToString(row["PASSWORD"]);
                list.Add(aux);
            }
        }

        /**
         * Metodo en el que se insertan usuarios a la tabla USERS Y USER_ROLES
         */
        /// <summary>
        /// Inserts a user and user roles in the database.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        public void insertUser(User user, List<Role> roles) {
            ConnectOracle search = new ConnectOracle();
            //Conseguir el id maximo+1 de la tabla USERS
            int maximum = Convert.ToInt32("0" + search.DLookUp("max(idUser)","USERS",""))+1;
            user.iduser = maximum;

            if (user.password != null)
            {
                user.password = resources.useful.GetSHA256(user.password);
            }

            //Se utiliza setData para sql que no sean consultas
            search.setData("INSERT INTO USERS VALUES("+maximum+",'"+user.name+"','"+ user.password+"',0)");

            foreach (Role role in roles)
            {
                //Conseguir el id maximo+1 de la tabla USER_ROLES
                int maximum2 = Convert.ToInt32("0" + search.DLookUp("max(id_user_role)", "USER_ROLES", "")) + 1;

                //Se utiliza el setData para insertar el role y el usuario en USER_ROLES
                search.setData("INSERT INTO USER_ROLES VALUES(" + role.idrole + "," + user.iduser + "," + maximum2 + ")");
            }
        }

        /**
         * Metodo que comprueba que existe un usuario dado por parametro
         * en una tabla, comprueba si hay un usuario con el mismo nombre y contraseña
         */
        /// <summary>
        /// Checks the user exists
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Boolean checkUser(User user) {
            Boolean exist = false;

            ConnectOracle search = new ConnectOracle();
            int count = Convert.ToInt32(search.DLookUp("count(idUser)", "USERS", "name='" + user.name + "' and password='" + user.password + "'" + "and deleted=0"));
            if (count == 1)
                exist = true;

            return exist;
        }

        /// <summary>
        /// Reads the user id given a user with a name and a password.
        /// </summary>
        /// <param name="user">The user.</param>
        public void readUserId(User user) {
            ConnectOracle search = new ConnectOracle();
            user.iduser = Convert.ToInt32(search.DLookUp("IDUSER", "USERS", "name='" + user.name + "' and password='" + user.password + "'" + "and deleted=0"));
        }

        /**
         * Metodo que comprueba que existe un usuario con cierto nombre
         */
        /// <summary>
        /// Checks the name of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Boolean checkUserName(User user) {
            Boolean exist = false;

            ConnectOracle search = new ConnectOracle();
            int count = Convert.ToInt32(search.DLookUp("count(idUser)", "USERS", "name='" + user.name + "'"));
            if (count == 1)
                exist = true;


            return exist;
        }

        /**
         * ¡BORRADO LOGICO! Deja el campo deleted de ese usuario a 1
         * 
         */
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void deleteUser(User user) {
            ConnectOracle search = new ConnectOracle();
            search.setData("UPDATE USERS SET DELETED=-1 WHERE IDUSER=" + user.iduser);
        }

        /**
         * Cambia la contraseña de un usuario
         */
        /// <summary>
        /// Changes the password of a user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void changePassword(User user) {
            ConnectOracle search = new ConnectOracle();
            user.password = resources.useful.GetSHA256(user.password);

            search.setData("UPDATE USERS SET PASSWORD='"+user.password+"' WHERE IDUSER=" + user.iduser);
        }

        /**
         * Cambia la contraseña y roles de un usuario
         * elimina los roles actuales del usuario e inserta los nuevos
         */

        /// <summary>
        /// Changes the user password and roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        public void changeUser(User user, List<Role> roles)
        {
            changePassword(user);

            ConnectOracle search = new ConnectOracle();

            //Elimina los roles de un usuario
            search.setData("DELETE FROM USER_ROLES WHERE IDUSER=" + user.iduser);

            //Inserta los roles nuevos al usuario
            foreach (Role role in roles) {
                //Conseguir el id maximo+1 de la tabla USER_ROLES
                int maximum2 = Convert.ToInt32("0" + search.DLookUp("max(id_user_role)", "USER_ROLES", "")) + 1;

                //Se utiliza el setData para insertar el role y el usuario en USER_ROLES
                search.setData("INSERT INTO USER_ROLES VALUES(" + role.idrole + "," + user.iduser + "," + maximum2 + ")");
            }
            

        }

        /**
         * Carga los privilegios de un usuario en base a los roles que tenga asignados
         */

        /// <summary>
        /// Charges the privileges of a user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void chargePrivileges(User user) {

            if (user.iduser != 0)
            {
                DataSet data = new DataSet();
                ConnectOracle search = new ConnectOracle();

                data = search.getData("SELECT P.IDPERMISSION, P.DESCRIPTION FROM PERMISSIONS P, USER_ROLES UR, ROLES_PERMISSIONS RP " +
                    "WHERE UR.IDROLE = RP.IDROLE AND P.IDPERMISSION = RP.IDPERMISSION AND UR.IDUSER = "+user.iduser+" "+
                    "GROUP BY P.IDPERMISSION, P.DESCRIPTION ORDER BY P.IDPERMISSION", "PERMISSIONS");

                DataTable table = data.Tables["PERMISSIONS"];

                Permission aux;

                foreach (DataRow row in table.Rows)
                {
                    aux = new Permission();
                    aux.idpermission = Convert.ToInt32(row["IDPERMISSION"]);
                    aux.description = Convert.ToString(row["DESCRIPTION"]);

                    user.Permissions.Add(aux);
                }


            }
            else {
                Permission aux = new Permission();
                aux.readAll();
                foreach(Permission perm in aux.manage.permissions)
                {
                    user.Permissions.Add(perm);
                }
            }


            }

        /**
         * Metodo para saber si es la primera sesion del usuario
         */

        /// <summary>
        /// Determines whether [is first session] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <c>true</c> if [is first session] [the specified user]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean isFirstSession(User user) {
            Boolean isFirstS = false;

            ConnectOracle search = new ConnectOracle();

            int count=Convert.ToInt32(search.DLookUp("COUNT(FIRSTSESSION)","USERS","FIRSTSESSION=-1 AND IDUSER="+user.iduser));

            if (count == 1) {
                isFirstS = true;
            }

            return isFirstS;
        }

        /**
         * Metodo para marcar la primera sesión de un usuario
         */

        /// <summary>
        /// Marks the first session of a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="first">if set to <c>true</c> [first].</param>
        public void markFirstSession(User user, Boolean first) {
            ConnectOracle search = new ConnectOracle();

            int fs = 0;

            if (first)
                fs = -1;

            search.setData("UPDATE USERS SET FIRSTSESSION="+fs+" WHERE IDUSER=" + user.iduser);

        }

        /**
         * Lee todos los usuarios en base a busqueda de nombre
         */

        /// <summary>
        /// Retrieves from the database the selected users.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <param name="name">The name.</param>
        public void readSelectUsers(User u, String name) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDUSER,NAME,PASSWORD FROM USERS WHERE NAME LIKE('%" + name + "%') AND IDUSER NOT IN(0,"+u.iduser+")","USERS");

            DataTable table = data.Tables["USERS"];
            User aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new User();
                aux.iduser = Convert.ToInt32(row["IDUSER"]);
                aux.name= Convert.ToString(row["NAME"]);
                aux.password = Convert.ToString(row["PASSWORD"]);

                list.Add(aux);
            }
        }

    }
}
