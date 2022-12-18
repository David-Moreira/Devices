using Device.Core.Models;
using Device.Core.Services;

using Microsoft.AspNetCore.Mvc;

using Models = Device.Core.Models;

namespace Device.Api.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
            => this._deviceService = deviceService;


        /// <summary>
        /// Creates a new Device
        /// </summary>
        /// <response code="201">Returns the created Device</response>
        /// <response code="400">Validation Error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(Models.Device), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<ActionResult<Models.Device>> Post([FromBody] Models.Device device)
        {
            var result = await _deviceService.Create(device);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Returns an existing Device
        /// </summary>
        /// <response code="200">Returns an existing Device</response>
        /// <response code="404">Device was not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Models.Device), 200)]
        public async Task<ActionResult<Models.Device>> Get([FromRoute] int id)
        {
            var device = await _deviceService.Get(id);

            return device is null
                ? NotFound()
                : Ok(device);
        }

        /// <summary>
        /// Returns all devices
        /// </summary>
        /// <response code="200">Returns all devices</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<Models.Device>), 200)]
        public async Task<ActionResult<IEnumerable<Models.Device>>> GetAll()
            => Ok(await _deviceService.GetAll());


        /// <summary>
        /// Updates an existing Device
        /// </summary>
        /// <response code="200">Returns the updated Device</response>
        /// <response code="400">Validation Error</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Models.Device), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<ActionResult<Models.Device>> Put([FromRoute] int id, [FromBody] Models.Device device)
        {
            device.Id = id;
            var result = await _deviceService.Update(device);
            return result is null
                ? NotFound()
                : Ok(result);
        }

        /// <summary>
        /// Deletes an existing Device
        /// </summary>
        /// <response code="204">Device has been deleted successfully</response>
        /// <response code="404">Device was not found</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteResult = await _deviceService.Delete(id);
            return deleteResult
                ? NoContent()
                : NotFound();
        }

        /// <summary>
        /// Returns existing Devices by Brand
        /// </summary>
        /// <response code="200">Returns existing Devices by Brand</response>
        [HttpGet("ByBrand/{brand}")]
        [ProducesResponseType(typeof(IEnumerable<Models.Device>), 200)]
        public async Task<ActionResult<IEnumerable<Models.Device>>> GetByBrand([FromRoute] string brand)

            => Ok(await _deviceService.GetByBrand(brand));

    }
}
