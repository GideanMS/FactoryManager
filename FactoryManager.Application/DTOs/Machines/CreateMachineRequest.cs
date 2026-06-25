namespace FactoryManager.Application.DTOs.Machines;

public class CreateMachineRequest
{
    public string Name { get; set; } = string.Empty;
    public int ProductionPerMinute { get; set; }
}