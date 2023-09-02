namespace EhotelBuffet.Service.Guest;

using EhotelBuffet.Model;

public interface IGuestService
{
    public Guest GenerateRandomGuest(DateTime seasonStart, DateTime seasonEnd);
    public HashSet<Guest> GetGuestsForDay(List<Guest> guests, DateTime date);
}

public class GuestService : IGuestService
{
    private static Random rand = new Random();
    public Guest GenerateRandomGuest(DateTime seasonStart, DateTime seasonEnd)
    {
        
        string randomName = GetRandomName();
        GuestType randomGuestType = GetRandomGuestType();
        (DateTime CheckIn, DateTime CheckOut) = GetRandomDate(seasonStart, seasonEnd);
        
        return new Guest(randomName, randomGuestType, CheckIn, CheckOut);

    }
    public HashSet<Guest> GetGuestsForDay(List<Guest> guests, DateTime date)
    {
        HashSet<Guest> guestsSet = new HashSet<Guest>();
        foreach (var guest in guests)
        {
            if (DateTime.Compare(guest.CheckIn, date) <= 0 && DateTime.Compare(guest.CheckOut, date) >= 0)
            {
                guestsSet.Add(guest);
            }
        }
        return guestsSet;
    }

    public string GetRandomName()
    {
        string[] guestsNames = new string[]
        {
            "Emily Johnson", "Benjamin Smith", "Olivia Williams", "William Brown", "Ava Davis",
            "James Anderson", "Sophia Martinez", "Ethan Taylor", "Mia Thompson", "Alexander Wilson",
            "Isabella Clark","Michael Hall", "Emma Murphy", "Daniel Turner", "Charlotte Parker",
            "David Garcia", "Amelia White", "Matthew Adams", "Harper Scott", "Liam Robinson"
        };
        return guestsNames[rand.Next(guestsNames.Length)];
    }

    public GuestType GetRandomGuestType()
    {
        var guestTypesArray = Enum.GetValues(typeof(GuestType));
        GuestType randomGuestType = (GuestType)guestTypesArray.GetValue(rand.Next(guestTypesArray.Length));
        return randomGuestType;
    }

    public (DateTime, DateTime) GetRandomDate(DateTime seasonStart, DateTime seasonEnd)
    {
        TimeSpan timeSpan = seasonEnd.AddDays(-7) - seasonStart;
        TimeSpan randomTimeSpan = new TimeSpan((long)(rand.NextDouble() * timeSpan.Ticks));
        DateTime CheckIn = seasonStart + randomTimeSpan;
        DateTime CheckOut = CheckIn.AddDays(rand.Next(1,8));
        return (CheckIn, CheckOut);
    }
} 