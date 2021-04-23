using Microsoft.EntityFrameworkCore;
using Pharma.Medicine.CoreEntity;
using Pharma.Medicine.InfraStructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharma.Medicine.Application.Repository
{
    public class MedicineRepository: IMedicineRepository
    {
        private readonly MedicineContext _medicineContext;

        public MedicineRepository(MedicineContext medicineContext)
        {
            _medicineContext = medicineContext;
        }

        public async Task Create(MedicineModel medicine)
        {
            _medicineContext.Medicines.Add(medicine);
            await _medicineContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(MedicineModel medicine)
        {
            _medicineContext.Medicines.Remove(medicine);
            await _medicineContext.SaveChangesAsync();

            return true;
        }

        public async Task<MedicineModel> GetMedcine(Guid id)
        {
            return await _medicineContext.Medicines.FindAsync(id);
        }

        public async Task<IEnumerable<MedicineModel>> GetMedicines()
        {
            return await _medicineContext.Medicines.ToListAsync();
        }

        public async Task<IEnumerable<MedicineModel>> GetMedicinesByBrand(string brand)
        {
            return await _medicineContext.Medicines.Where(
                (me) => me.Brand.Equals(brand)).ToListAsync();
        }

        public async Task<IEnumerable<MedicineModel>> GetMedicinesByName(string name)
        {
            return await _medicineContext.Medicines.Where(
                (me) => me.Name.Equals(name)).ToListAsync();
        }

        public async Task<bool> Update(MedicineModel medicine)
        {
            _medicineContext.Update(medicine);
            _medicineContext.Entry(medicine).State = EntityState.Modified;

            await _medicineContext.SaveChangesAsync();

            return true;
        }
    }
}
