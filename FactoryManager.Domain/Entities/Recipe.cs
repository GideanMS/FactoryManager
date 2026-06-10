public class Recipe
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int ProductionTimeInSeconds { get; private set; }
}