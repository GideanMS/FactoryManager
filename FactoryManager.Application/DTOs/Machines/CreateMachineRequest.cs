namespace FactoryManager.Application.DTOs.Machines;

public class CreateMachineRequest
{
    public string Name { get; set; } = string.Empty;
    public int ProductionPerMinute { get; set; }
    public int MaxProductionPerMinute { get; set; }
    public int EnergyConsumptionPerMinute { get; set; }
    public int MaintenanceIntervalInDays { get; set; }
}