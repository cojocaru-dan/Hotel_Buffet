namespace EhotelBuffet.Model;

public enum MenuItem
{
    Pizza,
    PolentaWithCheeseAndCream,
    Burger,
    BakedBeans,
    FishAndSalad,
    ChickenAndRice,
    BeefWithPotatoes,
    PorkSteak,
    GreekSalad,
    NoodlesWithVegetables
}
public static class MenuExtensions
{
    public static Random rand = new Random();
    public static Dictionary<MenuItem, int> MenuPopularity = new Dictionary<MenuItem, int>()
    {
        { MenuItem.Pizza, 5 },
        { MenuItem.PolentaWithCheeseAndCream, 6 },
        { MenuItem.Burger, 4 },
        { MenuItem.BakedBeans, 3 },
        { MenuItem.FishAndSalad, 7 },
        { MenuItem.ChickenAndRice, 9 },
        { MenuItem.BeefWithPotatoes, 8 },
        { MenuItem.PorkSteak, 10 },
        { MenuItem.GreekSalad, 2 },
        { MenuItem.NoodlesWithVegetables, 1 },
    };

    public static int GetPopularity(this MenuItem menuItem)
    {
        return MenuPopularity[menuItem];
    }

    public static MenuItem GetMenuItemByProbability()
    {
        int randomNumber = rand.Next(1, 101);
        if (80 < randomNumber && randomNumber < 101)
        {
            return MenuItem.PorkSteak;
        }
        else if (65 < randomNumber && randomNumber < 81)
        {
            return MenuItem.ChickenAndRice;
        }
        else if (51 < randomNumber && randomNumber < 66)
        {
            return MenuItem.BeefWithPotatoes;
        }
        else if (37 < randomNumber && randomNumber < 52)
        {
            return MenuItem.FishAndSalad;
        }
        else if (27 < randomNumber && randomNumber < 38)
        {
            return MenuItem.PolentaWithCheeseAndCream;
        }
        else if (19 < randomNumber && randomNumber < 28)
        {
            return MenuItem.Pizza;
        }
        else if (12 < randomNumber && randomNumber < 20)
        {
            return MenuItem.Burger;
        }
        else if (6 < randomNumber && randomNumber < 13)
        {
            return MenuItem.BakedBeans;
        }
        else if (2 < randomNumber && randomNumber < 7)
        {
            return MenuItem.GreekSalad;
        }
        else
        {
            return MenuItem.NoodlesWithVegetables;
        }
    }
}