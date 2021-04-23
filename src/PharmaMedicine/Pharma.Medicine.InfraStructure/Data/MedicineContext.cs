using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharma.Medicine.CoreEntity;

namespace Pharma.Medicine.InfraStructure.Data
{
    public class MedicineContext : DbContext
    {
        public MedicineContext(DbContextOptions<MedicineContext> options)
            : base(options)
        {

        }

        public DbSet<MedicineModel> Medicines { get; set; }
    }
}