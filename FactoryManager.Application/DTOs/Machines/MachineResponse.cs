namespace FactoryManager.Application.DTOs.Machines;

public class MachineResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductionPerMinute { get; set; }
    public bool IsActive { get; set; }
}