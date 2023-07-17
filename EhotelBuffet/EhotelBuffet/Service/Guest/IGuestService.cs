namespace EhotelBuffet.Service.Guest;

using EhotelBuffet.Model;

public interface IGuestService
{
    public Guest GenerateRandomGuest(DateTime seasonStart, DateTime seasonEnd);
    public HashSet<Guest> GetGuestsForDay(List<Guest> guests, DateTime date);
}