namespace FactoryManager.Domain.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }

    public Product Product { get; private set; }
    public int ProductionTimeInSeconds { get; private set; }

    // Objeto para o Entity Framework
    private Recipe()
    {
    }

    public Recipe(Guid productId, int productionTimeInSeconds)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductionTimeInSeconds = productionTimeInSeconds;
    }
}