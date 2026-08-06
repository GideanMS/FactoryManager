using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Application.Interfaces;
using FactoryManager.Application.Services;
using FactoryManager.Domain.Entities;
using FactoryManager.Domain.Exceptions;
using FactoryManager.Tests.Builders;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FactoryManager.Tests.Services;

public class MachineServiceTests
{
    [Fact]
    public async Task GetMachineById_ShouldReturnMachine_WhenMachineExists()
    {
        // Arrange
        var machine = new MachineBuilder().Build();
        var repositoryMock = new Mock<IMachineRepository>();
        var createValidatorMock = new Mock<IValidator<CreateMachineRequest>>();
        var updateValidatorMock = new Mock<IValidator<UpdateMachineRequest>>();

        repositoryMock.Setup(r => r.GetByIdAsync(machine.Id))
            .ReturnsAsync(machine);     

        var service = new MachineService(repositoryMock.Object, createValidatorMock.Object, updateValidatorMock.Object);

        // Act
        var result = await service.GetByIdAsync(machine.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(machine.Id);
        result.Name.Should().Be(machine.Name);
        result.ProductionPerMinute.Should().Be(machine.ProductionPerMinute);
        result.Status.Should().Be(machine.Status.ToString());
    }

    [Fact]
    public async Task GetMachineById_ShouldThrowNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();
        var repositoryMock = new Mock<IMachineRepository>();
        var createValidatorMock = new Mock<IValidator<CreateMachineRequest>>();
        var updateValidatorMock = new Mock<IValidator<UpdateMachineRequest>>();

        repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Machine?)null);

        var service = new MachineService(repositoryMock.Object, createValidatorMock.Object, updateValidatorMock.Object);

        // Act
        Func<Task> act = async () => await service.GetByIdAsync(machineId);
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateMachine_ShouldCallRepositoryOnce()
    {
        // Arrange
        var request = new CreateMachineRequest
        {
            Name = "New Machine",
            ProductionPerMinute = 80,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        };

        var repositoryMock = new Mock<IMachineRepository>();
        var createValidatorMock = new Mock<IValidator<CreateMachineRequest>>();
        var updateValidatorMock = new Mock<IValidator<UpdateMachineRequest>>();

        createValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateMachineRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var service = new MachineService(repositoryMock.Object, createValidatorMock.Object, updateValidatorMock.Object);

        // Act
        await service.CreateAsync(request);

        // Assert
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Machine>()), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
