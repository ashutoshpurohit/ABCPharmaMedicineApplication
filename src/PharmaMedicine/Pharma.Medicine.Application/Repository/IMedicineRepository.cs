using Pharma.Medicine.CoreEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pharma.Medicine.Application.Repository
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<MedicineModel>> GetMedicines();
        Task<MedicineModel> GetMedcine(Guid id);
        Task<IEnumerable<MedicineModel>> GetMedicinesByName(string name);
        Task<IEnumerable<MedicineModel>> GetMedicinesByBrand(string brand);
        Task Create(MedicineModel medicine);
        Task<bool> Update(MedicineModel medicine);
        Task<bool> Delete(MedicineModel medicine);
    }
}
