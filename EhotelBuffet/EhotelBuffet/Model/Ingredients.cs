namespace EhotelBuffet.Model;
public class Ingredient
{
    public string Name;
    public int Freshness;
    public int ExpirationTerm;
    public int Price;
    public Ingredient(string name, int freshness, int expirationTerm, int price)
    {
        Name = name;
        Freshness = freshness;
        ExpirationTerm = expirationTerm;
        Price = price;
    }
}