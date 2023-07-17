namespace EhotelBuffet.Model;

public record Guest(string Name, GuestType Type, DateTime CheckIn, DateTime CheckOut);