using AutoFixture;

using AutoMapper;

using Device.Api.Controllers;
using Device.Api.DTOs;
using Device.Core.Services;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using M = Device.Core.Models;

namespace Device.Api.Tests
{
    public class DeviceControllerTests
    {
        private readonly DeviceController _sut;
        private readonly IDeviceService _service = Substitute.For<IDeviceService>();
        private readonly IMapper _mapper;
        private readonly IFixture _fixture = new Fixture();

        public DeviceControllerTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Device.Core.Models.Device, DeviceDto>().ReverseMap();
                cfg.CreateMap<DeviceCreateUpdateDto, Device.Core.Models.Device>();
            }));

            _sut = new(_service, _mapper);
        }

        [Fact]
        public async Task GetAll_Returns_Existing_Devices()
        {
            // Arrange
            var expected = _fixture.CreateMany<M.Device>();
            IEnumerable<DeviceDto>? expectedMapped = _mapper.Map<IEnumerable<DeviceDto>>(expected);
            _service.GetAll().Returns(expected);

            // Act
            var actual = await _sut.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expectedMapped, okObjectResult.Value);
            await _service.Received(1).GetAll();
        }

        [Fact]
        public async Task Get_Returns_Existing_Device()
        {
            // Arrange
            var requestId = 1;
            var expected = _fixture.Create<M.Device>();
            var expectedMapped = _mapper.Map<DeviceDto>(expected);
            _service.Get(requestId).Returns(expected);

            // Act
            var actual = await _sut.Get(requestId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expectedMapped, okObjectResult.Value);
            await _service.Received(1).Get(requestId);
        }

        [Fact]
        public async Task Get_Returns_NotFound()
        {
            // Arrange
            var requestId = 1;
            _service.Get(requestId).Returns((M.Device)null);

            // Act
            var actual = await _sut.Get(requestId);

            // Assert
            Assert.IsType<NotFoundResult>(actual.Result);
            await _service.Received(1).Get(requestId);
        }

        [Fact]
        public async Task Post_Returns_Created_Device()
        {
            // Arrange
            var request = _fixture.Create<DeviceCreateUpdateDto>();
            var expected = _mapper.Map<M.Device>(request);
            var expectedMapped = _mapper.Map<DeviceDto>(expected);

            _service.Create(expected).Returns(expected);

            // Act
            var actual = await _sut.Post(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actual.Result);
            Assert.Equal(expectedMapped, createdAtActionResult.Value);
            await _service.Received(1).Create(expected);
        }

        [Fact]
        public async Task Put_Returns_Updated_Device()
        {
            // Arrange
            var request = _fixture.Create<DeviceCreateUpdateDto>();
            var expected = _mapper.Map<M.Device>(request);
            var expectedMapped = _mapper.Map<DeviceDto>(expected);

            _service.Update(expected).Returns(expected);

            // Act
            var actual = await _sut.Put(expected.Id, request);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expectedMapped, okObjectResult.Value);
            await _service.Received(1).Update(expected);
        }

        [Fact]
        public async Task Delete_Returns_OkNoContent_WhenSuccess()
        {
            // Arrange
            var requestId = 1;

            _service.Delete(requestId).Returns(true);

            // Act
            var actual = await _sut.Delete(requestId);

            // Assert
            Assert.IsType<NoContentResult>(actual);
            await _service.Received(1).Delete(requestId);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_WhenFailure()
        {
            // Arrange
            var requestId = 1;

            _service.Delete(requestId).Returns(false);

            // Act
            var actual = await _sut.Delete(requestId);

            // Assert
            Assert.IsType<NotFoundResult>(actual);
            await _service.Received(1).Delete(requestId);
        }
    }
}