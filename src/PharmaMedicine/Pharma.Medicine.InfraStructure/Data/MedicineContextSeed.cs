using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pharma.Medicine.CoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharma.Medicine.InfraStructure.Data
{
    public class MedicineContextSeed
    {
        public static async Task SeedAsync(MedicineContext medicineContext, 
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailabilty = retry.HasValue ? retry.Value : 0;

            try
            {
                medicineContext.Database.Migrate();
                if (!medicineContext.Medicines.Any())
                {
                    medicineContext.Medicines.AddRange(GetPreConfiguredMedicines());
                }
                await medicineContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                if (retryForAvailabilty < 3)
                {
                    retryForAvailabilty++;
                    var log = loggerFactory.CreateLogger<MedicineContextSeed>();
                    log.LogError(exception.Message);
                    await SeedAsync(medicineContext, loggerFactory, retryForAvailabilty);
                }
            }

        }

        private static IEnumerable<MedicineModel> GetPreConfiguredMedicines()
        {
            return new List<MedicineModel>
            {
                new MedicineModel()
                {
                   Id = Guid.NewGuid(),
                   Name = "Dummy Cetzine",
                   Brand = "Dummy Cipla",
                   Expiry = DateTime.Now.AddDays(365),
                   Quantity = 100,
                   Price = 100.98f,
                   Notes = "This is dummy medicine for test"
                   
                }
            };
        }
    }
}
