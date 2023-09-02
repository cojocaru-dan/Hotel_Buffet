using EhotelBuffet.Model;
using EhotelBuffet.Service.Guest;
using EhotelBuffet.Service.Logger;
using EhotelBuffet.Service.Buffet;
using EhotelBuffet.Service.Breakfast;
using EhotelBuffet.Service.Customer;
using EhotelBuffet.Service.Dinner;

namespace EhotelBuffet;

public class Program
{
    public static Random random = new Random();
    public static void Main(string[] args)
    {
        // Initialize services
        ILogger consoleLogger = new ConsoleLogger();
        /* IGuestService guestService = new GuestService();
        Buffet breakfastBuffet = new Buffet(new Dictionary<MealType, List<Portion>>());
        IBuffetService breakfastBuffetService = new BuffetService();
        Optimizer breakfastOptimizer = new Optimizer(); 
        BreakfastManager breakfastManager = new BreakfastManager(breakfastBuffet, breakfastBuffetService, breakfastOptimizer); */
        CustomerService customerService = new CustomerService();
        Kitchen kitchen = new Kitchen();
        DinnerBuffet dinnerBuffet = new DinnerBuffet(new List<MenuServing>());
        DinnerManager dinnerManager = new DinnerManager(kitchen, dinnerBuffet);


        /* // Generate guests for the season
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
        breakfastManager.Serve(guestsToday); */

        // Generate customers for the season
        List<Customer> AllCustomers = new List<Customer>();
        DateTime seasonStart = new DateTime(2023, 6, 1);
        DateTime seasonEnd = new DateTime(2023, 9, 15);
        int CustomersNr = random.Next(600, 1200);
        for (int i = 0; i < CustomersNr; i++)
        {
            Customer customer = customerService.GenerateRandomCustomer(seasonStart, seasonEnd);
            AllCustomers.Add(customer);
        }

        // Run dinner buffet
        HashSet<Customer> customersToday = customerService.GetCustomersForDay(AllCustomers, new DateTime(2023, 9, 1));
        dinnerManager.Serve(customersToday);
    }
}