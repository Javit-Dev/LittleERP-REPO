using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class ProductManage
    {
        public List<Object> GenericList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManage"/> class.
        /// </summary>
        public ProductManage()
        {
            GenericList = new List<Object>();
        }

        /**
         * Metodo para leer todos los productos
         */

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        public void readAllProducts()
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            data = search.getData("SELECT IDPRODUCT, NAME, PRICE, STOCK, B.IDBRAND, B.BRAND, S.IDSIZE, S.DESCRIPTION FROM PRODUCTS P, BRANDS B, SIZES S "+
                                    "WHERE P.IDSIZE = S.IDSIZE AND P.IDBRAND = B.IDBRAND AND DELETED=0 ORDER BY IDPRODUCT","Products");

            DataTable table = data.Tables["Products"];


            Product aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Product();
                aux.name = Convert.ToString(row["NAME"]);
                aux.id = Convert.ToInt32(row["IDPRODUCT"]);
                aux.price = Convert.ToDouble(row["PRICE"]);
                aux.stock = Convert.ToInt32(row["STOCK"]);

                Brand brand = new Brand();
                brand.id = Convert.ToInt32(row["IDBRAND"]);
                brand.name = Convert.ToString(row["BRAND"]);

                SizeDrive size = new SizeDrive();
                size.id = Convert.ToInt32(row["IDSIZE"]);
                size.name = Convert.ToString(row["DESCRIPTION"]);


                aux.brand = brand;
                aux.size = size;
                
                GenericList.Add(aux);
            }

        }

        /**
         * Metodo para leer un producto especifico por nombre
         */

        /// <summary>
        /// Retrieves all products given a name by parameter.
        /// </summary>
        /// <param name="product">The product.</param>
        public void readProductName(Product product)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            data = search.getData("SELECT IDPRODUCT, NAME, PRICE, STOCK, B.IDBRAND, B.BRAND, S.IDSIZE, S.DESCRIPTION FROM PRODUCTS P, BRANDS B, SIZES S " +
                                    "WHERE P.IDSIZE = S.IDSIZE AND P.IDBRAND = B.IDBRAND AND DELETED=0 AND NAME LIKE('%"+product.name+"%')", "Products");

            DataTable table = data.Tables["Products"];


            Product aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Product();
                aux.name = Convert.ToString(row["NAME"]);
                aux.id = Convert.ToInt32(row["IDPRODUCT"]);
                aux.price = Convert.ToDouble(row["PRICE"]);
                aux.stock = Convert.ToInt32(row["STOCK"]);

                Brand brand = new Brand();
                brand.id = Convert.ToInt32(row["IDBRAND"]);
                brand.name = Convert.ToString(row["BRAND"]);

                SizeDrive size = new SizeDrive();
                size.id = Convert.ToInt32(row["IDSIZE"]);
                size.name = Convert.ToString(row["DESCRIPTION"]);


                aux.brand = brand;
                aux.size = size;

                GenericList.Add(aux);
            }
        }

        /// <summary>
        /// Retrieves a product from the database given a id by parameter.
        /// </summary>
        /// <param name="product">The product.</param>
        public void readProduct(Product product)
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();
            data = search.getData("SELECT IDPRODUCT, NAME, PRICE,STOCK, B.IDBRAND, B.BRAND, S.IDSIZE, S.DESCRIPTION FROM PRODUCTS P, BRANDS B, SIZES S " +
                                    "WHERE P.IDSIZE = S.IDSIZE AND P.IDBRAND = B.IDBRAND AND DELETED=0 AND IDPRODUCT="+product.id, "Products");

            DataTable table = data.Tables["Products"];


            Product aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Product();
                aux.name = Convert.ToString(row["NAME"]);
                aux.id = Convert.ToInt32(row["IDPRODUCT"]);
                aux.price = Convert.ToDouble(row["PRICE"]);
                aux.stock = Convert.ToInt32(row["STOCK"]);

                Brand brand = new Brand();
                brand.id = Convert.ToInt32(row["IDBRAND"]);
                brand.name = Convert.ToString(row["BRAND"]);

                SizeDrive size = new SizeDrive();
                size.id = Convert.ToInt32(row["IDSIZE"]);
                size.name = Convert.ToString(row["DESCRIPTION"]);


                aux.brand = brand;
                aux.size = size;

                GenericList.Add(aux);
            }
        }

        /**
         * Metodo para insertar un producto indicado por parametro
         */

        /// <summary>
        /// Inserts the product in the database.
        /// </summary>
        /// <param name="product">The product.</param>
        public void insertProduct(Product product)
        {
            ConnectOracle search = new ConnectOracle();

            int maximum = Convert.ToInt32("0" + search.DLookUp("max(idproduct)", "PRODUCTS", "")) + 1;
         
            product.id = maximum;

            search.setData("INSERT INTO PRODUCTS(IDPRODUCT,NAME,IDSIZE,IDBRAND,PRICE,STOCK,DELETED) VALUES("+product.id+",'"+product.name+"',"+product.size.id+","+product.brand.id+","+product.price+","+product.stock+","+0+")");

        }

        /**
         * Metodo para eliminar un producto indicado por parametro
         */

        /// <summary>
        /// Removes the product from the database.
        /// </summary>
        /// <param name="product">The product.</param>
        public void removeProduct(Product product)
        {
            ConnectOracle search = new ConnectOracle();
            search.setData("UPDATE PRODUCTS SET DELETED=-1 WHERE IDPRODUCT=" + product.id);
        }

        /**
         * Metodo para modificar un producto indicado por parametro
         */

        /// <summary>
        /// Modifies the product from the database.
        /// </summary>
        /// <param name="product">The product.</param>
        public void modifyProduct(Product product)
        {
            ConnectOracle search = new ConnectOracle();

            search.setData("UPDATE PRODUCTS SET NAME='" + product.name + "',IDSIZE=" +product.size.id+",IDBRAND="+product.brand.id+",PRICE="+product.price+",STOCK="+product.stock+" WHERE IDPRODUCT="+product.id);
        }

        /**
         * Metodo para leer todos los tamaños de producto de la tabla Sizes
         */

        /// <summary>
        /// Retrieves all product sizes from the database.
        /// </summary>
        public void chargeSizes()
        {

            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDSIZE, DESCRIPTION FROM SIZES", "Sizes");

            DataTable table = data.Tables["Sizes"];

            SizeDrive aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new SizeDrive();
                aux.name = Convert.ToString(row["DESCRIPTION"]);
                aux.id = Convert.ToInt32(row["IDSIZE"]);
                GenericList.Add(aux);
            }

        }

        /**
         * Metodo para leer todas las marcas de producto de la tabla Brands
         */

        /// <summary>
        /// Retrieves all product brands from the database
        /// </summary>
        public void chargeBrands() 
        {
            DataSet data = new DataSet();
            ConnectOracle search = new ConnectOracle();

            data = search.getData("SELECT IDBRAND, BRAND FROM BRANDS", "Brands");

            DataTable table = data.Tables["Brands"];

            Brand aux;

            foreach (DataRow row in table.Rows)
            {
                aux = new Brand();
                aux.name = Convert.ToString(row["BRAND"]);
                aux.id = Convert.ToInt32(row["IDBRAND"]);
                GenericList.Add(aux);
            }
        }

    }
}
