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
        Id = Guid.NewGuid();
        Name = name;
    }
    
}