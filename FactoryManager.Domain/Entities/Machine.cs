namespace FactoryManager.Domain.Entities;

public class Machine
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int ProductionPerMinute { get; set; }
    public bool IsActive { get; set; }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}