using EhotelBuffet.Model;
using EhotelBuffet.Service.Guest;
using EhotelBuffet.Service.Logger;
using EhotelBuffet.Service.Buffet;
using EhotelBuffet.Service.Breakfast;

namespace EhotelBuffet;

public class Program
{
    public static Random random = new Random();
    public static void Main(string[] args)
    {
        // Initialize services
        ILogger consoleLogger = new ConsoleLogger();
        IGuestService guestService = new GuestService();
        Buffet breakfastBuffet = new Buffet(new Dictionary<MealType, List<Portion>>());
        IBuffetService breakfastBuffetService = new BuffetService();
        Optimizer breakfastOptimizer = new Optimizer(); 
        BreakfastManager breakfastManager = new BreakfastManager(breakfastBuffet, breakfastBuffetService, breakfastOptimizer);



        // MealType mealType = MealType.Muffin;
        // Dictionary<MealType, List<Portion>> mealPortions = new Dictionary<MealType, List<Portion>>
        // {
        //     { mealType, new List<Portion>() }
        // };
        
        // for (int i = 0; i < 5; i++)
        // {
        //     Thread.Sleep(200);
        //     Portion newPortion = new Portion(DateTime.Now, i);
        //     mealPortions[mealType].Add(newPortion);
        // }
        // Buffet newBuffet = new Buffet(mealPortions);
        // System.Console.WriteLine(buffetService.CollectWaste(newBuffet, mealType));
        // Generate guests for the season
        List<Guest> AllGuests = new List<Guest>();
        DateTime seasonStart = new DateTime(2023, 6, 1);
        DateTime seasonEnd = new DateTime(2023, 9, 15);
        int GuestsNr = random.Next(600, 1200);
        for (int i = 0; i < GuestsNr; i++)
        {
            Guest guest = guestService.GenerateRandomGuest(seasonStart, seasonEnd);
            AllGuests.Add(guest);
        }
        // Run breakfast buffet
        HashSet<Guest> guestsToday = guestService.GetGuestsForDay(AllGuests, new DateTime(2023, 8, 29));
        breakfastManager.Serve(guestsToday);
    }
}