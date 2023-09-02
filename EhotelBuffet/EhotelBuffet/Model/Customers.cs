namespace EhotelBuffet.Model;

public class Customer
{
    public string Name { get; init; }
    public List<MenuItem> Preferences { get; init; }
    public DateTime CheckIn { get; init; }
    public DateTime CheckOut { get; init; }
    public Customer(string name, DateTime checkIn, DateTime checkOut)
    {
        Name = name;
        Preferences = GetRandomPreferences();
        CheckIn = checkIn;
        CheckOut = checkOut;
    }
    public List<MenuItem> GetRandomPreferences()
    {
        List<MenuItem> preferences = new List<MenuItem>();
        while (preferences.Count < 4)
        {
            MenuItem preferedMenuItem = MenuExtensions.GetMenuItemByProbability();
            if (!preferences.Contains(preferedMenuItem)) preferences.Add(preferedMenuItem);
        }
        return preferences;
    }
}