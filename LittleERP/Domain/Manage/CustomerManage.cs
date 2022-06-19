using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class CustomerManage
    {
        //Lista generica: para hacer un casting a una lista de objetos concreta es con: reg.manage.GenericList.Cast<Region>().ToList()
        public List<Object> GenericList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManage"/> class.
        /// </summary>
        public CustomerManage() { 
            GenericList = new List<Object>();
        }



        /**
         * Metodo que lee todo de cada cliente existente en la tabla de Customers y se añaden a la lista generica
         */
        /// <summary>
        /// Retrieves all the customers from the database.
        /// </summary>
        public void readAll()
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDCUSTOMER,NIF, NAME,SURNAME,ADDRESS,PHONE,EMAIL, IDREGION, REGION, IDSTATE, STATE, IDCITY, CITY, IDZIPCODE, ZIPCODE FROM CUSTOMERS C, ZIPCODESCITIES ZC, STATES S, REGIONS R, ZIPCODES Z, CITIES C  " +
                "WHERE C.REFZIPCODESCITIES = ZC.IDZIPCODESCITIES " +
                "AND ZC.REFSTATE = S.IDSTATE AND S.REFREGION = R.IDREGION " +
                "AND ZC.REFCITY = C.IDCITY AND ZC.REFZIPCODE = Z.IDZIPCODE "+
                "AND DELETED=0", "Customers");

            DataTable table = data.Tables["Customers"];

            Customer aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Customer();
                aux.id = Convert.ToInt32(row["IDCUSTOMER"]);
                aux.name = Convert.ToString(row["NAME"]);
                aux.NIF = Convert.ToString(row["NIF"]);
                aux.email = Convert.ToString(row["EMAIL"]);
                aux.surname = Convert.ToString(row["SURNAME"]);
                aux.address = Convert.ToString(row["ADDRESS"]);
                aux.phone = Convert.ToInt32(row["PHONE"]);

                Region region = new Region();
                region.id = Convert.ToInt32(row["IDREGION"]);
                region.name= Convert.ToString(row["REGION"]);

                State state = new State();
                state.id = Convert.ToInt32(row["IDSTATE"]);
                state.name = Convert.ToString(row["STATE"]);

                City city = new City();
                city.id = Convert.ToInt32(row["IDCITY"]);
                city.name=Convert.ToString(row["CITY"]);

                Zipcode zipcode = new Zipcode();
                zipcode.id = Convert.ToInt32(row["IDZIPCODE"]);
                zipcode.name= Convert.ToString(row["ZIPCODE"]);

                aux.region = region;
                aux.state = state;
                aux.city = city;
                aux.zipcode = zipcode;

                GenericList.Add(aux);
            }
        }

        /**
         * Metodo igual que el de readAll, pero se le pasa un customer por parametro y busca un customer por un nombre o nif
         */
        /// <summary>
        /// Retrieves a customer from the database given a customer with a name or NIF.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void readCustomer(Customer customer) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDCUSTOMER,NIF, NAME,SURNAME,ADDRESS,PHONE,EMAIL, IDREGION, REGION, IDSTATE, STATE, IDCITY, CITY, IDZIPCODE, ZIPCODE FROM CUSTOMERS C, ZIPCODESCITIES ZC, STATES S, REGIONS R, ZIPCODES Z, CITIES C  " +
                "WHERE C.REFZIPCODESCITIES = ZC.IDZIPCODESCITIES " +
                "AND ZC.REFSTATE = S.IDSTATE AND S.REFREGION = R.IDREGION " +
                "AND ZC.REFCITY = C.IDCITY AND ZC.REFZIPCODE = Z.IDZIPCODE " +
                "AND DELETED=0 AND NAME LIKE('" + customer.name + "%') AND NIF LIKE('"+customer.NIF+"%')", "Customers"); 

            DataTable table = data.Tables["Customers"];

            Customer aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Customer();
                aux.id = Convert.ToInt32(row["IDCUSTOMER"]);
                aux.name = Convert.ToString(row["NAME"]);
                aux.NIF = Convert.ToString(row["NIF"]);
                aux.email = Convert.ToString(row["EMAIL"]);
                aux.surname = Convert.ToString(row["SURNAME"]);
                aux.address = Convert.ToString(row["ADDRESS"]);
                aux.phone = Convert.ToInt32(row["PHONE"]);

                Region region = new Region();
                region.id = Convert.ToInt32(row["IDREGION"]);
                region.name = Convert.ToString(row["REGION"]);

                State state = new State();
                state.id = Convert.ToInt32(row["IDSTATE"]);
                state.name = Convert.ToString(row["STATE"]);

                City city = new City();
                city.id = Convert.ToInt32(row["IDCITY"]);
                city.name = Convert.ToString(row["CITY"]);

                Zipcode zipcode = new Zipcode();
                zipcode.id = Convert.ToInt32(row["IDZIPCODE"]);
                zipcode.name = Convert.ToString(row["ZIPCODE"]);

                aux.region = region;
                aux.state = state;
                aux.city = city;
                aux.zipcode = zipcode;
                GenericList.Add(aux);
            }

        }

        /**
         * Insertar un customer en la tabla de Customers, se necesita buscar el maximo id+1 y el idzipcodescities
         */
        /// <summary>
        /// Inserts the customer in the database.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void insertCustomer(Customer customer) {
            ConnectOracle search = new ConnectOracle();

            int maximum = Convert.ToInt32("0" + search.DLookUp("max(idcustomer)", "CUSTOMERS", "")) + 1;
            //Se busca el idzipcodecities en base al zipcode guardado del customer
            int idzipcodescities = Convert.ToInt32(search.DLookUp("IDZIPCODESCITIES", "ZIPCODESCITIES ZC ", "ZC.REFZIPCODE=" + customer.zipcode.id + " AND ZC.REFCITY="+customer.city.id + " AND REFSTATE="+customer.state.id));
            customer.id = maximum;

            search.setData("INSERT INTO CUSTOMERS(IDCUSTOMER,NAME,NIF,SURNAME,ADDRESS,PHONE,EMAIL,REFZIPCODESCITIES,DELETED) VALUES(" + maximum +  ", '"+customer.name+"', '"+customer.NIF+"', '"+customer.surname+"','"+customer.address+"',"+customer.phone+",'"+customer.email+"',"+ idzipcodescities + ",0"+  ")");

        }

        /**
         * Metodo para eliminar un customer de la tabla Customers, se busca por su id
         */

        /// <summary>
        /// Removes the customer from the database.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void removeCustomer(Customer customer) {
            ConnectOracle search = new ConnectOracle();
            search.setData("UPDATE CUSTOMERS SET DELETED=-1 WHERE IDCUSTOMER=" + customer.id);
        }

        /**
         * Metodo para modificar un customer de la tabla Customers, se modifica todo practicamente menos el NIF
         */
        /// <summary>
        /// Modifies the customer from the database.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public void modifyCustomer(Customer customer) {
            ConnectOracle search = new ConnectOracle();

            int idzipcodescities = Convert.ToInt32(search.DLookUp("IDZIPCODESCITIES", "ZIPCODESCITIES ZC ", "ZC.REFZIPCODE=" + customer.zipcode.id + " AND ZC.REFCITY=" + customer.city.id + " AND REFSTATE=" + customer.state.id));


            search.setData("UPDATE CUSTOMERS SET NAME='" + customer.name + "',SURNAME='" + customer.surname + "',ADDRESS='" + customer.address + "',PHONE=" + customer.phone + ",EMAIL='" + customer.email + "',REFZIPCODESCITIES=" + idzipcodescities + "WHERE IDCUSTOMER =" + customer.id);
        }

        /**
         * Metodo para cargar todas las regiones y añadirlas a la lista generica
         */
        /// <summary>
        /// Retrieves all regions from the database.
        /// </summary>
        public void chargeRegions()
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDREGION,REGION FROM REGIONS R "+
                                    "GROUP BY IDREGION,REGION " +
                                    "ORDER BY IDREGION ", "Regions");

            DataTable table = data.Tables["Regions"];

            Region aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Region();
                aux.name = Convert.ToString(row["REGION"]);
                aux.id = Convert.ToInt32(row["IDREGION"]);
                GenericList.Add(aux);
            }

        }

        /**
         * Metodo para cargar regiones en base a un zipcode dado por parametro
         */
        /// <summary>
        /// Retrieves all regions given a zipcode by parameter.
        /// </summary>
        /// <param name="zip">The zip.</param>
        public void chargeRegions(Zipcode zip)
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT DISTINCT IDREGION, REGION FROM REGIONS R, STATES S, ZIPCODESCITIES ZC "+
                                    "WHERE R.IDREGION = S.REFREGION AND S.IDSTATE = ZC.REFSTATE AND "+
                                    "ZC.REFZIPCODE = "+zip.id, "Regions");

            DataTable table = data.Tables["Regions"];

            Region aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Region();
                aux.name = Convert.ToString(row["REGION"]);
                aux.id = Convert.ToInt32(row["IDREGION"]);
                GenericList.Add(aux);
            }

        }

        /**
         * Metodo para cargar las ciudades en base a un state dado por parametro
         */

        /// <summary>
        /// Retrieves all cities given a state by parameter.
        /// </summary>
        /// <param name="state">The state.</param>
        public void chargeCities(State state)
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDCITY, CITY FROM CITIES C, ZIPCODESCITIES Z " +
                                    "WHERE Z.REFCITY = C.IDCITY " +
                                    "AND Z.REFSTATE =  " + state.id + " "+
                                    "GROUP BY IDCITY, CITY " +
                                    "ORDER BY IDCITY", "Cities");

            DataTable table = data.Tables["Cities"];

            City aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new City();
                aux.name = Convert.ToString(row["CITY"]);
                aux.id = Convert.ToInt32(row["IDCITY"]);
                GenericList.Add(aux);
            }





        }

        /**
         * Metodo para cargar las ciudades en base a un zipcode dado por parametro
         */

        /// <summary>
        /// Retrieves all cities given a zipcode by parameter.
        /// </summary>
        /// <param name="zip">The zip.</param>
        public void chargeCities(Zipcode zip) {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT DISTINCT IDCITY, CITY FROM CITIES C, ZIPCODESCITIES ZC " +
                                    "WHERE C.IDCITY = ZC.REFCITY AND " +
                                    "ZC.REFZIPCODE = "+zip.id, "Cities");

            DataTable table = data.Tables["Cities"];

            City aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new City();
                aux.name = Convert.ToString(row["CITY"]);
                aux.id = Convert.ToInt32(row["IDCITY"]);
                GenericList.Add(aux);
            }

        }


        /**
         * Metodo para cargar los zipcodes en base a una ciudad y state dados por parametro
         */

        /// <summary>
        /// Retrieves all zipcodes given a city and a state by parameter.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        public void chargeZipcodes(City city, State state)
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            //La consulta se hace así ya que solo con el id de la ciudad no vale, hay varios zipcode asignados a una misma ciudad pero a distintos state
            data = search.getData("SELECT IDZIPCODE, ZIPCODE FROM ZIPCODES Z, ZIPCODESCITIES ZC "+
                                    "WHERE Z.IDZIPCODE = ZC.REFZIPCODE "+
                                    "AND ZC.REFCITY = "+city.id + " "+
                                    "AND ZC.REFSTATE = "+state.id + " "+
                                    "GROUP BY IDZIPCODE, ZIPCODE " +
                                    "ORDER BY IDZIPCODE", "Zipcodes");

            DataTable table = data.Tables["Zipcodes"];

            Zipcode aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Zipcode();
                aux.name = Convert.ToString(row["ZIPCODE"]);
                aux.id = Convert.ToInt32(row["IDZIPCODE"]);
                GenericList.Add(aux);
            }

        }

        /**
         * Metodo para buscar un zipcode en base a un zipcode dado por parametro, se busca por su "nombre"
         */

        /// <summary>
        /// Searches the zipcode gizen a zipcode by parameter.
        /// </summary>
        /// <param name="zip">The zip.</param>
        public void searchZipcode(String zip) {
           DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDZIPCODE, ZIPCODE FROM ZIPCODES Z " +
                                    "WHERE Z.ZIPCODE = '" +zip+ "' "+
                                    "GROUP BY IDZIPCODE, ZIPCODE " +
                                    "ORDER BY IDZIPCODE", "Zipcodes");

            DataTable table = data.Tables["Zipcodes"];

            Zipcode aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Zipcode();
                aux.name = Convert.ToString(row["ZIPCODE"]);
                aux.id = Convert.ToInt32(row["IDZIPCODE"]);
                GenericList.Add(aux);
            }
        }
        /**
        * Metodo para cargar los States en base a una region dada por parametro
        */

        /// <summary>
        /// Retrieves all states given a region by parameter.
        /// </summary>
        /// <param name="region">The region.</param>
        public void chargeStates(Region region)
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDSTATE, STATE FROM STATES S " +
                                    "WHERE REFREGION = " + region.id + " "+
                                    "GROUP BY IDSTATE, STATE " +
                                    "ORDER BY IDSTATE", "States");

            DataTable table = data.Tables["States"];

            State aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new State();
                aux.name = Convert.ToString(row["STATE"]);
                aux.id = Convert.ToInt32(row["IDSTATE"]);
                GenericList.Add(aux);
            }

        }
        /**
        * Metodo para cargar los States en base a un zipcode dado por parametro
        */

        /// <summary>
        /// Retrieves all states given a zipcode by parameter.
        /// </summary>
        /// <param name="zip">The zip.</param>
        public void chargeStates(Zipcode zip)
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT DISTINCT IDSTATE, STATE FROM STATES S, ZIPCODESCITIES ZC " +
                                    "WHERE ZC.REFZIPCODE = "+zip.id+" AND "+
                                    "ZC.REFSTATE = S.IDSTATE", "States");

            DataTable table = data.Tables["States"];

            State aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new State();
                aux.name = Convert.ToString(row["STATE"]);
                aux.id = Convert.ToInt32(row["IDSTATE"]);
                GenericList.Add(aux);
            }

        }


    }
}
