namespace EhotelBuffet.Model;

public record Buffet(Dictionary<MealType, List<Portion>> MealPortions);


public class Portion
{
    public MealType MealType { get; init; }
    public int Cycle = 0;
    public int Amount { get; set; }

    public Portion(MealType mealType, int amount)
    {
        MealType = mealType;
        Amount = amount;
    }
}