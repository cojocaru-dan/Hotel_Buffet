namespace EhotelBuffet.Service.Dinner;
using EhotelBuffet.Model;
using EhotelBuffet.Service.Logger;
public class DinnerManager
{
    private Kitchen Kitchen;
    private DinnerBuffet DinnerBuffet;
    private int ExpectedCustomers = 0;
    private static Random random = new Random();
    private ILogger _logger = new ConsoleLogger();

    public DinnerManager(Kitchen kitchen, DinnerBuffet dinnerBuffet)
    {
        Kitchen = kitchen;
        DinnerBuffet = dinnerBuffet;
    }

    public void Serve(HashSet<Customer> customersToday)
    {
        ExpectedCustomers = customersToday.Count;
        List<HashSet<Customer>> dinnerGroups = GenerateDinnerGroups(customersToday);
        int unhappyCustomers = 0;
        int customersHapiness = 0;
        int expiredIngredients = 0;
        int totalWaste = 0;

        for (int cycle = 0; cycle < 8; cycle++)
        {
            int expectedCustomersPerCycle = (int) Math.Ceiling((decimal)ExpectedCustomers / (8 - cycle));
            _logger.LogInfo($"We expect to see {expectedCustomersPerCycle} customers in this cycle.");
            List <MenuServing> optimalMenuServings = RefillKitchenAndGetOptimalMenuServings(expectedCustomersPerCycle);
            DinnerBuffetRefillFromKitchen(optimalMenuServings);
            HashSet<Customer> currentGroup = dinnerGroups[cycle];
            _logger.LogInfo($"In reality {currentGroup.Count} customers arrived in this cycle.");
            ConsumeDinner(currentGroup, ref unhappyCustomers, ref customersHapiness);
            DecreaseFreshness(ref expiredIngredients);
            CollectWaste(ref totalWaste);
            ExpectedCustomers -= currentGroup.Count;
        }
        _logger.LogInfo($"At the end we have {unhappyCustomers} unhappy customers.");
        _logger.LogInfo($"Customers Hapiness is {customersHapiness}.");
        _logger.LogInfo($"We have {expiredIngredients} expired ingredients.");
        _logger.LogInfo($"Total waste of food is {totalWaste} $.");
    }

    public List<HashSet<Customer>> GenerateDinnerGroups(HashSet<Customer> guestsToday)
    {
        int NumberOfGuests = guestsToday.Count;
        List<HashSet<Customer>> groups = new List<HashSet<Customer>>();

        for (int i = 0; i < 8; i++)
        {
            int randomGroupNumber = random.Next(0, guestsToday.Count / 4);
            randomGroupNumber = i == 7 ? NumberOfGuests : Math.Min(randomGroupNumber, NumberOfGuests);

            HashSet<Customer> group = new HashSet<Customer>();
            for (int j = 0; j < randomGroupNumber; j++)
            {
                Customer randomGuest = guestsToday.First();
                guestsToday.Remove(randomGuest);
                group.Add(randomGuest);

            }
            groups.Add(group);
            NumberOfGuests -= randomGroupNumber;
        }
        return groups;
    }

    private void DinnerBuffetRefillFromKitchen(List<MenuServing> optimalMenuServings)
    {
        _logger.LogInfo($"The buffet has already {DinnerBuffet.Servings.Count} servings.");
        foreach (var serving in optimalMenuServings)
        {
            List<Ingredient> servingIngredients = serving.Ingredients;
            Kitchen.Remove(servingIngredients);
            DinnerBuffet.Servings.Add(serving);
        }
        _logger.LogInfo($"The buffet was refilled with {optimalMenuServings.Count} servings from the kitchen.");
    }

    public List<MenuServing> RefillKitchenAndGetOptimalMenuServings(int expectedCustomersPerCycle)
    {
        int optimalDinnerServings = Math.Max(expectedCustomersPerCycle - DinnerBuffet.Servings.Count, 0);
        List<MenuServing> optimalMenuServings = new List<MenuServing>();

        for (int i = 0; i < optimalDinnerServings; i++)
        {
            MenuItem menuItem = MenuExtensions.GetMenuItemByProbability();
            List<Ingredient> menuItemIngredients = Kitchen.GetIngredients(menuItem);
            Kitchen.Add(menuItemIngredients);
            optimalMenuServings.Add(new MenuServing(menuItem, menuItemIngredients));
        }
        _logger.LogInfo($"The kitchen was refilled and we have {optimalMenuServings.Count} additional servings for the buffet.");
        return optimalMenuServings;
    }
    private void ConsumeDinner(HashSet<Customer> currentGroup, ref int unhappyCustomers, ref int customersHapiness)
    {
        int happyCustomers = 0;
        foreach (var customer in currentGroup)
        {
            bool buffetHasCustomerPreference = false;
            foreach (var preference in customer.Preferences)
            {
                MenuServing? servingInBuffet = DinnerBuffet.Servings.Find(serving => serving.Name == preference);
                if (servingInBuffet != null)
                {
                    buffetHasCustomerPreference = true;
                    happyCustomers += 1;
                    Ingredient? oldIngredient = servingInBuffet.Ingredients.Find(ingredient => ingredient.Freshness == 1);
                    if (oldIngredient != null) customersHapiness += 10;
                    else customersHapiness += 15;
                    DinnerBuffet.Servings.Remove(servingInBuffet);
                    break;
                }
            }
            if (!buffetHasCustomerPreference) unhappyCustomers += 1;
        }
        _logger.LogInfo($"We have {happyCustomers} happy customers and {currentGroup.Count - happyCustomers} unhappy customers in this cycle.");
    }
    public void DecreaseFreshness(ref int expiredIngredients)
    {
        foreach (var serving in DinnerBuffet.Servings)
        {
            foreach (var ingredient in serving.Ingredients)
            {
                ingredient.Freshness -= 1;
                if (ingredient.Freshness == ingredient.ExpirationTerm) expiredIngredients += 1;
            }
        }
    }

    public void CollectWaste(ref int totalWaste)
    {
        List<MenuServing> servingsToRemove = new List<MenuServing>();
        foreach (var serving in DinnerBuffet.Servings)
        {
            bool removeServing = false; 
            int wastePerServing = 0;
            foreach (var ingredient in serving.Ingredients)
            {
                wastePerServing += ingredient.Price;
                if (ingredient.Freshness == ingredient.ExpirationTerm) removeServing = true;
            }
            if (removeServing)
            {
                totalWaste += wastePerServing;
                servingsToRemove.Add(serving);
            }
        }
        foreach (var servingToRemove in servingsToRemove)
        {
            DinnerBuffet.Servings.Remove(servingToRemove);
        }
        _logger.LogInfo($"We removed {servingsToRemove.Count} servings in this cycle.\n\n");
    }
}