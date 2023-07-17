namespace EhotelBuffet.Model;

public enum MealType
{
    ScrambledEggs,
    SunnySideUp,
    FriedSausage,
    FriedBacon,
    Pancake,
    Croissant,
    MashedPotato,
    Muffin,
    Bun,
    Cereal,
    Milk
}

public static class MealTypeExtensions
{
    private static readonly Dictionary<MealType, (int Cost, MealDurability Durability)> MealTypeMap =
        new Dictionary<MealType, (int Cost, MealDurability Durability)>
        {
            { MealType.ScrambledEggs, (70, MealDurability.Short) },
            { MealType.SunnySideUp, (70, MealDurability.Short) },
            { MealType.FriedSausage, (100, MealDurability.Short) },
            { MealType.FriedBacon, (70, MealDurability.Short) },
            { MealType.Pancake, (40, MealDurability.Short) },
            { MealType.Croissant, (40, MealDurability.Short) },
            { MealType.MashedPotato, (20, MealDurability.Medium) },
            { MealType.Muffin, (20, MealDurability.Medium) },
            { MealType.Bun, (10, MealDurability.Medium) },
            { MealType.Cereal, (30, MealDurability.Long) },
            { MealType.Milk, (10, MealDurability.Long) }
        };

    public static int GetCost(this MealType mealType)
    {
        return MealTypeMap[mealType].Cost;
    }

    public static MealDurability GetDurability(this MealType mealType)
    {
        return MealTypeMap[mealType].Durability;
    }
}