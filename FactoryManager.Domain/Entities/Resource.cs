namespace FactoryManager.Domain.Entities;

public class Resource
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    // Objeto para o Entity Framework
    private Resource()
    {
    }

    public Resource(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}