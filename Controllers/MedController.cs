using MedViaApi.Models;
using MedViaApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedViaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedController : ControllerBase
    {
        private readonly MedService _mediService;

        public MedController(MedService mediService)
        {
            _mediService=mediService;
        }


        [HttpPost("info")]
        public async Task<IActionResult> GetMedicineInfo([FromBody] MedicineRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Medicine name required.");

            var result = await _mediService.GetMedicineDetails(request.Name);

            if (result == null)
                return BadRequest("Failed to fetch medicine details. Try again.");

            return Ok(result);
        }
    }
}
