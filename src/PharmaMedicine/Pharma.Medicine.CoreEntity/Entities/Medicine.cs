using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharma.Medicine.CoreEntity
{
    public class MedicineModel
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Expiry { get; set; }
        public string Notes { get; set; }
    }
}
