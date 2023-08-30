namespace EhotelBuffet.Service.Breakfast;
using EhotelBuffet.Model;
using EhotelBuffet.Service.Buffet;

public class BreakfastManager
{
    public static Random random = new Random();
    private Buffet BreakfastBuffet { get; }
    private IBuffetService BreakfastBuffetService { get; }
    private Optimizer BreakfastOptimizer { get; }

    public BreakfastManager(Buffet buffet, IBuffetService buffetService, Optimizer breakfastOptimizer)
    {
        this.BreakfastBuffet = buffet;
        this.BreakfastBuffetService = buffetService;
        this.BreakfastOptimizer = breakfastOptimizer;
    }

    public void Serve(HashSet<Guest> guestsToday)
    {
        BuildOptimizer(guestsToday);
        int costOfUnhappyGuest = 20;
        int unhappyGuests = 0;
        int totalWaste = 0;
        List<HashSet<Guest>> breakfastGroups = PrepareBreakfast(guestsToday);

        for (int cycle = 0; cycle < 8; cycle++)
        {
            Dictionary<MealType, int> optimalPortions = GetOptimalPortions(BreakfastBuffet, BreakfastOptimizer, 8 - cycle, costOfUnhappyGuest);
            BreakfastBuffetService.BuffetRefill(BreakfastBuffet, optimalPortions);
            HashSet<Guest> currentGroup = breakfastGroups[cycle];
            BreakfastBuffetService.ConsumeBreakfast(BreakfastBuffet, currentGroup, ref unhappyGuests);
            DecreaseFreshness();
            CollectTotalWaste(ref totalWaste);
            UpdateOptimizer(currentGroup);
        }
        Console.WriteLine($"Unhappy Guests: {unhappyGuests}");
        Console.WriteLine($"Total Waste of Food: {totalWaste}");
    }

    private void BuildOptimizer(HashSet<Guest> guestsToday)
    {
        foreach (var guest in guestsToday)
        {
            if (guest.Type == GuestType.Business) BreakfastOptimizer.Business += 1;
            else if (guest.Type == GuestType.Tourist) BreakfastOptimizer.Tourist += 1;
            else if (guest.Type == GuestType.Kid) BreakfastOptimizer.Kid += 1;
        }
    }
    public List<HashSet<Guest>> PrepareBreakfast(HashSet<Guest> guestsToday)
    {
        int NumberOfGuests = guestsToday.Count;
        List<HashSet<Guest>> groups = new List<HashSet<Guest>>();
        
        for (int i = 0; i < 8; i++)
        {
            int randomGroupNumber = random.Next(0, guestsToday.Count / 3);
            randomGroupNumber = i == 7 ? NumberOfGuests : Math.Min(randomGroupNumber, NumberOfGuests);

            HashSet<Guest> group = new HashSet<Guest>();
            for (int j = 0; j < randomGroupNumber; j++)
            {
                Guest randomGuest = guestsToday.First();
                guestsToday.Remove(randomGuest);
                group.Add(randomGuest);
                
            }
            groups.Add(group);
            NumberOfGuests -= randomGroupNumber;
        }
        return groups;
    }
    public Dictionary<MealType, int> GetOptimalPortions(Buffet buffet, Optimizer optimizer, int leftCycles, int costOfUnhappyGuest)
    {
        Dictionary<MealType, int> optimalPortions = new Dictionary<MealType, int>()
        {
            { MealType.ScrambledEggs, 0},
            { MealType.SunnySideUp, 0},
            { MealType.FriedSausage, 0},
            { MealType.FriedBacon, 0},
            { MealType.Pancake, 0},
            { MealType.Croissant, 0},
            { MealType.MashedPotato, 0},
            { MealType.Muffin, 0},
            { MealType.Bun, 0},
            { MealType.Cereal, 0},
            { MealType.Milk, 0}
        };

        (int portionsAlreadyInTheBuffet, int businessPortionsInTheBuffet, int touristPortionsInTheBuffet, int kidPortionsInTheBuffet) = CountPortionsInTheBuffet();
        
        int averagePortionsToExpect = (optimizer.Business + optimizer.Tourist + optimizer.Kid) / leftCycles;
        int portionsToMake = Math.Max(averagePortionsToExpect - portionsAlreadyInTheBuffet, 0);
        System.Console.WriteLine("portionsAlreadyInTheBuffet {0}", portionsAlreadyInTheBuffet);
        System.Console.WriteLine("averagePortionsToExpect {0}", averagePortionsToExpect);
        System.Console.WriteLine("portionsToMake {0}", portionsToMake);

        CountOptimalPortionsToMake(optimalPortions, portionsToMake, businessPortionsInTheBuffet, touristPortionsInTheBuffet, kidPortionsInTheBuffet, optimizer, costOfUnhappyGuest);
        return optimalPortions;
    }

    private (int, int, int, int) CountPortionsInTheBuffet()
    {
        int portionsAlreadyInTheBuffet = 0;
        int businessPortionsInTheBuffet = 0;
        int touristPortionsInTheBuffet = 0;
        int kidPortionsInTheBuffet = 0;

        foreach (MealType mealType in BreakfastBuffet.MealPortions.Keys)
        {
            int numberOfPortionsPerMealType = BreakfastBuffet.MealPortions[mealType].Count;
            portionsAlreadyInTheBuffet += numberOfPortionsPerMealType;
            if (GuestType.Business.GetMealPreferences().Contains(mealType)) businessPortionsInTheBuffet += numberOfPortionsPerMealType;
            else if (GuestType.Tourist.GetMealPreferences().Contains(mealType)) touristPortionsInTheBuffet += numberOfPortionsPerMealType;
            else if (GuestType.Kid.GetMealPreferences().Contains(mealType)) kidPortionsInTheBuffet += numberOfPortionsPerMealType;
        }
        return (portionsAlreadyInTheBuffet, businessPortionsInTheBuffet, touristPortionsInTheBuffet, kidPortionsInTheBuffet);
    }
    private void CountOptimalPortionsToMake(Dictionary<MealType, int> optimalPortions, int portionsToMake, int businessPortionsInTheBuffet, int touristPortionsInTheBuffet, int kidPortionsInTheBuffet, Optimizer optimizer, int costOfUnhappyGuest)
    {
        List<MealType> businessPreferences = GuestType.Business.GetMealPreferences();
        List<MealType> touristPreferences = GuestType.Tourist.GetMealPreferences();
        List<MealType> kidPreferences = GuestType.Kid.GetMealPreferences();
        int portionNumber = 0;

        while (portionNumber < portionsToMake)
        {
            if (businessPortionsInTheBuffet < optimizer.Business)
            {
                MealType newBusinessMealType = businessPreferences[random.Next(businessPreferences.Count)];
                optimalPortions[newBusinessMealType] += 1;
                businessPortionsInTheBuffet += 1;
                portionNumber += 1;
            }
            if (touristPortionsInTheBuffet < optimizer.Tourist)
            {
                MealType newTouristMealType = touristPreferences[random.Next(touristPreferences.Count)];
                optimalPortions[newTouristMealType] += 1;
                touristPortionsInTheBuffet += 1;
                portionNumber += 1;
            }
            if (kidPortionsInTheBuffet < optimizer.Kid)
            {
                MealType newKidMealType = kidPreferences[random.Next(kidPreferences.Count)];
                optimalPortions[newKidMealType] += 1;
                kidPortionsInTheBuffet += 1;
                portionNumber += 1;
            }
        }
        CountExtraPortionsToMake(optimalPortions, costOfUnhappyGuest, portionNumber, portionsToMake);
        
    }
    public void CountExtraPortionsToMake(Dictionary<MealType, int> optimalPortions, int costOfUnhappyGuest, int portionNumber, int portionsToMake)
    {
        List<MealType> touristPreferences = GuestType.Tourist.GetMealPreferences();
        int averageWasteCost = 0;
        foreach (var mealType in optimalPortions.Keys)
        {
            averageWasteCost += mealType.GetCost();
        }
        averageWasteCost /= optimalPortions.Count;

        double unhappyGuestAverageWasteRaport = (double) costOfUnhappyGuest / averageWasteCost;
        int extraPortionsToMake = unhappyGuestAverageWasteRaport > 0.6 ? 1 : 0;
        extraPortionsToMake = Math.Max(extraPortionsToMake - (portionNumber - portionsToMake), 0);

        for (int i = 0; i < extraPortionsToMake; i++)
        {
            MealType newTouristMealType = touristPreferences[random.Next(touristPreferences.Count)];
            optimalPortions[newTouristMealType] += 1;
        }
    }

    public void DecreaseFreshness()
    {
        foreach (var mealType in BreakfastBuffet.MealPortions.Keys)
        {
            foreach (var portion in BreakfastBuffet.MealPortions[mealType])
            {
                portion.Cycle += 1;
            }
        }
    }
    public void CollectTotalWaste(ref int totalWaste)
    {
        foreach (var mealType in BreakfastBuffet.MealPortions.Keys)
        {
            int wastePerMeaLType = BreakfastBuffetService.CollectWaste(BreakfastBuffet, mealType);
            totalWaste += wastePerMeaLType;
        }
    }

    private void UpdateOptimizer(HashSet<Guest> currentGroup)
    {
        foreach (var guest in currentGroup)
        {
            if (guest.Type == GuestType.Business)
            {
                BreakfastOptimizer.Business -= 1;
            }
            else if (guest.Type == GuestType.Tourist)
            {
                BreakfastOptimizer.Tourist -= 1;
            }
            else if (guest.Type == GuestType.Kid)
            {
                BreakfastOptimizer.Kid -= 1;
            }
        }
    }
}

public class Optimizer
{
    public int Business { get; set; } = 0;
    public int Tourist { get; set; } = 0;
    public int Kid { get; set; } = 0;
}