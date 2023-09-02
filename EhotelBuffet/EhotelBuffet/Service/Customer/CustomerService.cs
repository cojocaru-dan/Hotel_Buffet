namespace EhotelBuffet.Service.Customer;

using EhotelBuffet.Model;

public class CustomerService
{
    private static Random rand = new Random();
    public Customer GenerateRandomCustomer(DateTime seasonStart, DateTime seasonEnd)
    {

        string randomName = GetRandomName();
        (DateTime CheckIn, DateTime CheckOut) = GetRandomDate(seasonStart, seasonEnd);

        return new Customer(randomName, CheckIn, CheckOut);

    }
    public HashSet<Customer> GetCustomersForDay(List<Customer> Customers, DateTime date)
    {
        HashSet<Customer> CustomersSet = new HashSet<Customer>();
        foreach (var Customer in Customers)
        {
            if (DateTime.Compare(Customer.CheckIn, date) <= 0 && DateTime.Compare(Customer.CheckOut, date) >= 0)
            {
                CustomersSet.Add(Customer);
            }
        }
        return CustomersSet;
    }

    public static string GetRandomName()
    {
        string[] CustomersNames = new string[]
        {
            "Emily Johnson", "Benjamin Smith", "Olivia Williams", "William Brown", "Ava Davis",
            "James Anderson", "Sophia Martinez", "Ethan Taylor", "Mia Thompson", "Alexander Wilson",
            "Isabella Clark","Michael Hall", "Emma Murphy", "Daniel Turner", "Charlotte Parker",
            "David Garcia", "Amelia White", "Matthew Adams", "Harper Scott", "Liam Robinson"
        };
        return CustomersNames[rand.Next(CustomersNames.Length)];
    }

    public static (DateTime, DateTime) GetRandomDate(DateTime seasonStart, DateTime seasonEnd)
    {
        TimeSpan timeSpan = seasonEnd.AddDays(-7) - seasonStart;
        TimeSpan randomTimeSpan = new TimeSpan((long)(rand.NextDouble() * timeSpan.Ticks));
        DateTime CheckIn = seasonStart + randomTimeSpan;
        DateTime CheckOut = CheckIn.AddDays(rand.Next(1, 8));
        return (CheckIn, CheckOut);
    }
}