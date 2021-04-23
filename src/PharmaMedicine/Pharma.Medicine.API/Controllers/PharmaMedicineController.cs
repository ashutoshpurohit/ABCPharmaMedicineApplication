using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pharma.Medicine.Application.Repository;
using Pharma.Medicine.CoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pharma.Medicine.API
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PharmaMedicineController: ControllerBase
    {
        private readonly IMedicineRepository _repository;
        private readonly ILogger<PharmaMedicineController> _logger;

        public PharmaMedicineController(IMedicineRepository repository, 
            ILogger<PharmaMedicineController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedicineModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<MedicineModel>>> GetMedicines()
        {
            var medicines = await _repository.GetMedicines();
            return Ok(medicines);
        }

        [Route("[action]/{id}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MedicineModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MedicineModel>> GetMedicineById(string id)
        {
            var medicine = await _repository.GetMedcine(new Guid(id));

            if (medicine == null)
            {
                _logger.LogError($"Medicine with id: {id}, not found.");
                return NotFound();
            }

            return Ok(medicine);
        }

        [Route("[action]/{name}", Name = "GetMedicineByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedicineModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<MedicineModel>>> GetMedicineByName(string name)
        {
            var medicines = await _repository.GetMedicinesByName(name);
            return Ok(medicines);
        }

        [Route("[action]/{brand}", Name = "GetMedicineByBrand")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedicineModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<MedicineModel>>> GetMedicineByBrand(string brand)
        {
            var medicines = await _repository.GetMedicinesByBrand(brand);
            return Ok(medicines);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MedicineModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MedicineModel>> CreateMedicine([FromBody] MedicineModel medicine)
        {
            var id = Guid.NewGuid();
            medicine.Id = id;
            await _repository.Create(medicine);

            var createdmedicine = await _repository.GetMedcine(id);

            return Ok(createdmedicine);

        }

        [HttpPut]
        [ProducesResponseType(typeof(MedicineModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMedicine([FromBody] MedicineModel medicine)
        {
            return Ok(await _repository.Update(medicine));
        }

        [HttpDelete(Name = "DeleteMedicine")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMedicineById(string id)
        {
            var medicine = await _repository.GetMedcine(new Guid(id));
            if (medicine == null)
            {
                _logger.LogError($"Medicine with id: {id}, not found.");
                return NotFound();
            }
            return Ok(await _repository.Delete(medicine));
        }
    }
}

