using FactoryManager.Domain.Exceptions;

namespace FactoryManager.Domain.Entities;

public class Machine
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int ProductionPerMinute { get; private set; }
    public int MaxProductionPerMinute { get; private set; }
    public decimal EnergyConsumptionPerMinute { get; private set; }
    public DateTime? LastMaintenanceAt { get; private set; }
    public int MaintenanceIntervalInDays { get; private set; }
    public MachineStatus Status { get; private set; }

// Objeto para o Entity Framework
    private Machine()
    {
    }

    public Machine(string name, int productionPerMinute, int maxProductionPerMinute, decimal energyConsumptionPerMinute, int maintenanceIntervalInDays)
    {
        Validate(name, productionPerMinute, maxProductionPerMinute, energyConsumptionPerMinute, maintenanceIntervalInDays);
        Id = Guid.NewGuid();
        Name = name;
        ProductionPerMinute = productionPerMinute;
        MaxProductionPerMinute = maxProductionPerMinute;
        EnergyConsumptionPerMinute = energyConsumptionPerMinute;
        LastMaintenanceAt = DateTime.UtcNow;
        MaintenanceIntervalInDays = maintenanceIntervalInDays;
        Status = MachineStatus.Offline;
    }

    public void Activate()
    {
        if (Status == MachineStatus.Maintenance)
        {
            throw new DomainException("Cannot activate a machine under maintenance.");
        }

        if (LastMaintenanceAt?.AddDays(MaintenanceIntervalInDays) < DateTime.UtcNow)
        {
            throw new DomainException("Machine requires maintenance before activation.");
        }

        Status = MachineStatus.Running;
    }

    public void Deactivate()
    {
        if (Status == MachineStatus.Maintenance)
        {
            throw new DomainException("Cannot deactivate a machine under maintenance.");
        }

        Status = MachineStatus.Offline;
    }

    public void StartMaintenance()
    {
        Status = MachineStatus.Maintenance;
    }

    public void CompleteMaintenance()
    {
        LastMaintenanceAt = DateTime.UtcNow;
        Status = MachineStatus.Offline;
    }

    public void UpdateInformation(string name, int productionPerMinute, int maxProductionPerMinute, decimal energyConsumptionPerMinute, int maintenanceIntervalInDays)
    {
        Validate(name, productionPerMinute, maxProductionPerMinute, energyConsumptionPerMinute, maintenanceIntervalInDays);
        Name = name;
        ProductionPerMinute = productionPerMinute;
        MaxProductionPerMinute = maxProductionPerMinute;
        EnergyConsumptionPerMinute = energyConsumptionPerMinute;
        MaintenanceIntervalInDays = maintenanceIntervalInDays;
    }

    private static void Validate(string name, int productionPerMinute, int maxProductionPerMinute, decimal energyConsumptionPerMinute, int maintenanceIntervalInDays)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Machine name cannot be empty.");
        }

        if (productionPerMinute < 0)
        {
            throw new DomainException("Production per minute cannot be negative.");
        }

        if (productionPerMinute > maxProductionPerMinute)
        {
            throw new DomainException("Production per minute cannot exceed the maximum production limit.");
        }
        if (maxProductionPerMinute < 0)
        {
            throw new DomainException("Max production per minute cannot be negative.");
        }
        if (energyConsumptionPerMinute < 0)
        {
            throw new DomainException("Energy consumption per minute cannot be negative.");
        }
        if (maintenanceIntervalInDays < 0)
        {
            throw new DomainException("Maintenance interval in days cannot be negative.");
        }
    }
}