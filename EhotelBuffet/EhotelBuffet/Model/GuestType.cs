namespace EhotelBuffet.Model;

using System.Collections.Generic;

public enum GuestType
{
    Business,
    Tourist,
    Kid
}

public static class GuestTypeExtensions
{
    private static readonly Dictionary<GuestType, List<MealType>> MealPreferencesMap =
        new Dictionary<GuestType, List<MealType>>
        {
            {
                GuestType.Business,
                new List<MealType>
                {
                    MealType.ScrambledEggs,
                    MealType.FriedBacon,
                    MealType.Croissant
                }
            },
            {
                GuestType.Tourist,
                new List<MealType>
                {
                    MealType.SunnySideUp,
                    MealType.FriedSausage,
                    MealType.MashedPotato,
                    MealType.Bun,
                    MealType.Muffin
                }
            },
            {
                GuestType.Kid,
                new List<MealType>
                {
                    MealType.Pancake,
                    MealType.Muffin,
                    MealType.Cereal,
                    MealType.Milk
                }
            }
        };

    public static List<MealType> GetMealPreferences(this GuestType guestType)
    {
        return MealPreferencesMap[guestType];
    }
}