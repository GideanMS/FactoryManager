using FactoryManager.Domain.Entities;

namespace FactoryManager.Tests.Builders;

public class MachineBuilder
{
    private string _name = "Steel Furnace";
    private int _productionPerMinute = 50;
    private int _maxProductionPerMinute = 100;
    private decimal _energyConsumptionPerMinute = 10;
    private int _maintenanceIntervalInDays = 30;
    private bool _isActive = true;

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

    public MachineBuilder WithMaxProductionPerMinute(int maxProductionPerMinute)
    {
        _maxProductionPerMinute = maxProductionPerMinute;
        return this;
    }

    public MachineBuilder WithEnergyConsumptionPerMinute(decimal energyConsumptionPerMinute)
    {
        _energyConsumptionPerMinute = energyConsumptionPerMinute;
        return this;
    }

    public MachineBuilder WithMaintenanceIntervalInDays(int maintenanceIntervalInDays)
    {
        _maintenanceIntervalInDays = maintenanceIntervalInDays;
        return this;
    }

    public MachineBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public Machine Build()
    {
        var machine = new Machine(
            _name,
            _productionPerMinute,
            _maxProductionPerMinute,
            _energyConsumptionPerMinute,
            _maintenanceIntervalInDays);

        if (_isActive)
            machine.Activate();

        return machine;
    }
}