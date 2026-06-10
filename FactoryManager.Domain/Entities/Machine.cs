public class Machine
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int ProductionPerMinute { get; private set; }
    public bool IsActive { get; private set; }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}