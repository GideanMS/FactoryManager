namespace FactoryManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Money { get; private set; }

    // Objeto para o Entity Framework
    private User()
    {
    }

    public User(string name, decimal money)
    {
        Id = Guid.NewGuid();
        Name = name;
        Money = money;
    }
    
    public void AddMoney(decimal value)
    {
        Money += value;
    }
}