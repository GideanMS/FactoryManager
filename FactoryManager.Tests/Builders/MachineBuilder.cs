using FactoryManager.Domain.Entities;

namespace FactoryManager.Tests.Builders;

public class MachineBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "Steel Furnace";
    private int _productionPerMinute = 50;
    private bool _isActive = true;

    public MachineBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public MachineBuilder WithName(string name)
    {
        _name = name;
        return this;    
    }

    public MachineBuilder WithProductionPerMinute(int productionPerMinute)
    {
        _productionPerMinute = productionPerMinute;
        return this;
    }

    public MachineBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public Machine Build()
    {
        var machine = new Machine(_name, _productionPerMinute);

        if (_isActive)
            machine.Activate();

        return machine;
    }
}