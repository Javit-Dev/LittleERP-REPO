using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleERP.Domain.Manage
{
    public class SupplierManage
    {
        public List<Supplier> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierManage"/> class.
        /// </summary>
        public SupplierManage()
        {
            list = new List<Supplier>();
        }

        /// <summary>
        /// Retrieves all suppliers from a json.
        /// </summary>
        public void readAll()
        {
            String json = File.ReadAllText(@"Suppliers.json");
            list = JsonConvert.DeserializeObject<List<Supplier>>(json);
        }
    }
}
