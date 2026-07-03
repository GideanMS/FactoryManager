namespace FactoryManager.Application.DTOs.Machines;

public class UpdateMachineRequest
{
    public string Name { get; set; } = string.Empty;
    public int ProductionPerMinute { get; set; }
}