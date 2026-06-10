public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Money { get; private set; }

    public void AddMoney(decimal value)
    {
        Money += value;
    }
}