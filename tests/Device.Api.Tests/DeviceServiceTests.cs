using Device.Api.Controllers;
using Device.Api.DTOs;
using Device.Core.Services;
using M = Device.Core.Models;
using AutoFixture;

using AutoMapper;


using Microsoft.AspNetCore.Mvc;


using NSubstitute;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Device.Core.Repository;

namespace Device.Api.Tests
{
    public class DeviceServiceTests
    {

        private readonly DeviceService _sut;
        private readonly IDeviceRepository _repository = Substitute.For<IDeviceRepository>();
        private readonly IFixture _fixture = new Fixture();

        public DeviceServiceTests()
            => _sut = new(_repository);

        [Fact]
        public async Task GetAll_Returns_Existing_Devices()
        {
            // Arrange
            var expected = _fixture.CreateMany<M.Device>();
            _repository.GetAll().Returns(expected);

            // Act
            var actual = await _sut.GetAll();

            // Assert
            Assert.Equal(expected, actual);
            await _repository.Received(1).GetAll();
        }

        [Fact]
        public async Task Get_Returns_Existing_Device()
        {
            // Arrange
            var expected = _fixture.Create<M.Device>();
            _repository.Get(expected.Id).Returns(expected);

            // Act
            var actual = await _sut.Get(expected.Id);

            // Assert
            Assert.Equal(expected, actual);
            await _repository.Received(1).Get(expected.Id);
        }


        [Fact]
        public async Task Create_Returns_Created_WhenSuccess()
        {
            // Arrange
            var expected = _fixture.Create<M.Device>();

            _repository.Create(expected).Returns(expected);

            // Act
            var actual = await _sut.Create(expected);

            // Assert
            Assert.Equal(expected, actual);
            await _repository.Received(1).Create(expected);
        }

        [Fact]
        public async Task Update_Returns_Updated_WhenSuccess()
        {
            // Arrange
            var expected = _fixture.Create<M.Device>();

            _repository.Update(expected).Returns(expected);

            // Act
            var actual = await _sut.Update(expected);

            // Assert
            Assert.Equal(expected, actual);
            await _repository.Received(1).Update(expected);
        }

        [Fact]
        public async Task Delete_Returns_Success()
        {
            // Arrange
            var expected = _fixture.Create<M.Device>();
            _repository.Delete(expected.Id).Returns(true);

            // Act
            var actual = await _sut.Delete(expected.Id);

            // Assert
            Assert.True(actual);
            await _repository.Received(1).Delete(expected.Id);
        }

    }
}
