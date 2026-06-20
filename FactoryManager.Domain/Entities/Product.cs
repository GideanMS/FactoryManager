namespace FactoryManager.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    // Objeto para o Entity Framework
    private Product()
    {
    }

    public Product(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
}