namespace EhotelBuffet.Service.Buffet;
using EhotelBuffet.Model;

public interface IBuffetService
{
    public void AddPortionsToBuffet(Buffet buffet, MealType mealType, int amount);
    public void BuffetRefill(Buffet buffet, Dictionary<MealType, int> portionsPerMealType);
    public bool ConsumeFreshest(Buffet buffet, MealType mealType);
    public void ConsumeBreakfast(Buffet breakfastBuffet, HashSet<Guest> currentGroup, ref int unhappyGuests);
    public int CollectWaste(Buffet buffet, MealType mealType);
}

public class BuffetService : IBuffetService
{
    public void AddPortionsToBuffet(Buffet buffet, MealType mealType, int amount)
    {
        Portion newPortion = new Portion(mealType, amount);
        buffet.MealPortions[mealType].Insert(0, newPortion);
    }
    public void BuffetRefill(Buffet buffet, Dictionary<MealType, int> portionsPerMealType)
    {
        foreach (var mealType  in portionsPerMealType.Keys)
        {
            if (portionsPerMealType[mealType] == 0) continue;
            else
            {
                Portion newPortion = new Portion(mealType, portionsPerMealType[mealType]);
                if (buffet.MealPortions.ContainsKey(mealType))
                {
                    buffet.MealPortions[mealType].Add(newPortion);
                }
                else
                {
                    buffet.MealPortions.Add(mealType, new List<Portion>(){ newPortion });
                }
            }
        }

    }
    public bool ConsumeFreshest(Buffet buffet, MealType mealType)
    {
        if (buffet.MealPortions[mealType].Count == 0) return false;
        buffet.MealPortions[mealType].Sort((portion1, portion2) => portion1.Cycle.CompareTo(portion2.Cycle));
        buffet.MealPortions[mealType].RemoveAt(0);
        return true;
    }
    public void ConsumeBreakfast(Buffet breakfastBuffet, HashSet<Guest> currentGroup, ref int unhappyGuests)
    {
        foreach (var guest in currentGroup)
        {
            List<MealType> guestPreferences = guest.Type.GetMealPreferences();
            List<MealType> commonMealTypes = new List<MealType>();
            foreach (var mealType in guestPreferences)
            {
                if (breakfastBuffet.MealPortions.ContainsKey(mealType) && breakfastBuffet.MealPortions[mealType].Count > 0)
                {
                    commonMealTypes.Add(mealType);
                }
            }
            if (commonMealTypes.Count == 0)
            {
                unhappyGuests += 1;
            }
            else
            {
                MealType randomCommonMealType = commonMealTypes[new Random().Next(commonMealTypes.Count)];
                List<Portion> portionsInTheBuffet = breakfastBuffet.MealPortions[randomCommonMealType];
                Portion randomChosenPortion = portionsInTheBuffet[new Random().Next(portionsInTheBuffet.Count)];
                if (randomChosenPortion.Amount > 0)
                {
                    randomChosenPortion.Amount -= 1;
                }
                else
                {
                    portionsInTheBuffet.Remove(randomChosenPortion);
                }
            }
        }
    }
    public int CollectWaste(Buffet buffet, MealType mealType)
    {
        int mealTypeCost = mealType.GetCost();
        MealDurability mealTypeDurability = mealType.GetDurability();
        int CycleDurability = mealTypeDurability == MealDurability.Short ? 3
                            : mealTypeDurability == MealDurability.Medium ? 5
                            : 8;
        int waste = 0;
        int idx = 0;
        List<Portion> buffetMealPortionsPerMealType = buffet.MealPortions[mealType];
        while (idx < buffetMealPortionsPerMealType.Count)
        {
            if (buffetMealPortionsPerMealType[idx].Cycle >= CycleDurability)
            {
                waste += mealTypeCost * buffetMealPortionsPerMealType[idx].Amount;
                buffetMealPortionsPerMealType.RemoveAt(idx);
            } else idx++;

        }
        if (buffetMealPortionsPerMealType.Count == 0) buffet.MealPortions.Remove(mealType); 
        return waste;
    }
}