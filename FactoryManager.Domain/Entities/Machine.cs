namespace FactoryManager.Domain.Entities;

public class Machine
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int ProductionPerMinute { get; private set; }
    public bool IsActive { get; private set; }

// Objeto para o Entity Framework
    private Machine()
    {
    }

    public Machine(string name, int productionPerMinute)
    {
        Validate(name, productionPerMinute);
        Id = Guid.NewGuid();
        Name = name;
        ProductionPerMinute = productionPerMinute;
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateInformation(string name, int productionPerMinute)
    {
        Validate(name, productionPerMinute);
        Name = name;
        ProductionPerMinute = productionPerMinute;
    }

    private static void Validate(string name, int productionPerMinute)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Machine name cannot be empty.");
        }

        if (productionPerMinute < 0)
        {
            throw new ArgumentException("Production cannot be negative.");
        }
    }
}