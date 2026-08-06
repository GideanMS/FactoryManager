namespace FactoryManager.Application.DTOs.Machines;

public class MachineResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductionPerMinute { get; set; }
    public string Status { get; set; } = string.Empty;
    public int MaxProductionPerMinute { get; set; }
    public decimal EnergyConsumptionPerMinute { get; set; }
    public DateTime? LastMaintenanceAt { get; set; }
    public int MaintenanceIntervalInDays { get; set; }
}