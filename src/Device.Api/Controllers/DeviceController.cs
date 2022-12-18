using AutoMapper;

using Device.Api.DTOs;
using Device.Core.Models;
using Device.Core.Services;

using Microsoft.AspNetCore.Mvc;

using Models = Device.Core.Models;

namespace Device.Api.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;

        public DeviceController(IDeviceService deviceService, IMapper mapper)
        {
            this._deviceService = deviceService;
            this._mapper = mapper;
        }


        /// <summary>
        /// Creates a new Device
        /// </summary>
        /// <response code="201">Returns the created Device</response>
        /// <response code="400">Validation Error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(DeviceDto), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<ActionResult<DeviceDto>> Post([FromBody] DeviceCreateUpdateDto device)
        {
            var deviceModel = _mapper.Map<Models.Device>(device);
            var result = await _deviceService.Create(deviceModel);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, _mapper.Map<DeviceDto>(result));
        }

        /// <summary>
        /// Returns an existing Device
        /// </summary>
        /// <response code="200">Returns an existing Device</response>
        /// <response code="404">Device was not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DeviceDto), 200)]
        public async Task<ActionResult<DeviceDto>> Get([FromRoute] int id)
        {
            var device = await _deviceService.Get(id);

            return device is null
                ? NotFound()
                : Ok(_mapper.Map<DeviceDto>(device));
        }

        /// <summary>
        /// Returns all devices
        /// </summary>
        /// <response code="200">Returns all devices</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<DeviceDto>), 200)]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<DeviceDto>>(await _deviceService.GetAll()));


        /// <summary>
        /// Updates an existing Device
        /// </summary>
        /// <response code="200">Returns the updated Device</response>
        /// <response code="400">Validation Error</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(DeviceDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<ActionResult<DeviceDto>> Put([FromRoute] int id, [FromBody] DeviceCreateUpdateDto device)
        {
            var deviceModel = _mapper.Map<Models.Device>(device);
            deviceModel.Id = id;
            var result = await _deviceService.Update(deviceModel);
            return result is null
                ? NotFound()
                : Ok(_mapper.Map<DeviceDto>(result));
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
        [ProducesResponseType(typeof(IEnumerable<DeviceDto>), 200)]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetByBrand([FromRoute] string brand)

            => Ok(_mapper.Map<IEnumerable<DeviceDto>>(await _deviceService.GetByBrand(brand)));

    }
}
