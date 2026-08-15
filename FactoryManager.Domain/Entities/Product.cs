using FactoryManager.Domain.Exceptions;

namespace FactoryManager.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public ICollection<Recipe> Recipes { get; private set; } = new List<Recipe>();

    // Objeto para o Entity Framework
    private Product()
    {
    }

    public Product(string name)
    {
        Validate(name);
        Id = Guid.NewGuid();
        Name = name;
    }

    public void UpdateInformation(string name)
    {
        Validate(name);
        Name = name;
    }

    private static void Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
    }
}