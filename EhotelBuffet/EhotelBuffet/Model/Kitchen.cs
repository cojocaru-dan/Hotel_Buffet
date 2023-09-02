namespace EhotelBuffet.Model;

public class Kitchen
{
    public List<Ingredient> Stock = new List<Ingredient>();

    public Dictionary<MenuItem, List<Tuple<string, int, int, int>>> KitchenMenu = new Dictionary<MenuItem, List<Tuple<string, int, int, int>>>()
    {
        { MenuItem.Pizza, new List<Tuple<string, int, int, int>>()
            {
                new Tuple<string, int, int, int>("Mozarella", 3, 0, 80),
                new Tuple<string, int, int, int>("Ham", 3, 0, 65),
                new Tuple<string, int, int, int>("Mushrooms", 3, 0, 60)
            }
        },
        { MenuItem.PolentaWithCheeseAndCream, new List<Tuple<string, int, int, int>>()
            {
                new Tuple<string, int, int, int>("Corn", 3, 0, 25),
                new Tuple<string, int, int, int>("Cheese", 3, 0, 55),
                new Tuple<string, int, int, int>("Sour Cream", 3, 0, 60)
            } 
        },
        { MenuItem.Burger, new List<Tuple<string, int, int, int>>()
            {
                new Tuple<string, int, int, int>("Fries", 3, 0, 20),
                new Tuple<string, int, int, int>("Cheddar", 3, 0, 90),
                new Tuple<string, int, int, int>("Beef", 3, 0, 80)
            } 
        },
        { MenuItem.BakedBeans, new List<Tuple<string, int, int, int>>()
            { 
                new Tuple<string, int, int, int>("Beans", 3, 0, 40), 
                new Tuple<string, int, int, int>("Peppers", 3, 0, 35), 
            } 
        },
        { MenuItem.FishAndSalad, new List<Tuple<string, int, int, int>>()
            {
                new Tuple<string, int, int, int>("Fish", 3, 0, 75),
                new Tuple<string, int, int, int>("Cabbage", 3, 0, 25),
            } 
        },
        { MenuItem.ChickenAndRice, new List<Tuple<string, int, int, int>>()
            {
                new Tuple<string, int, int, int>("Chicken", 3, 0, 50),
                new Tuple<string, int, int, int>("Rice", 3, 0, 20),
            } 
        },
        { MenuItem.BeefWithPotatoes, new List<Tuple<string, int, int, int>>() 
            {
                new Tuple<string, int, int, int>("Beef", 3, 0, 80),
                new Tuple<string, int, int, int>("Potatoes", 3, 0, 20),
            } 
        },
        { MenuItem.PorkSteak, new List<Tuple<string, int, int, int>>() 
            {
                new Tuple<string, int, int, int>("Pork", 3, 0, 60),
                new Tuple<string, int, int, int>("Tomatoes", 3, 0, 25),
                new Tuple<string, int, int, int>("Garlic", 3, 0, 15)
            } 
        },
        { MenuItem.GreekSalad, new List<Tuple<string, int, int, int>>() 
            {
                new Tuple<string, int, int, int>("Onions", 3, 0, 15),
                new Tuple<string, int, int, int>("Olives", 3, 0, 70),
                new Tuple<string, int, int, int>("Cheese", 3, 0, 55)
            } 
        },
        { MenuItem.NoodlesWithVegetables, new List<Tuple<string, int, int, int>>() 
            {
                new Tuple<string, int, int, int>("Noodles", 3, 0, 25),
                new Tuple<string, int, int, int>("Carrot", 3, 0, 20),
                new Tuple<string, int, int, int>("Celery", 3, 0, 25),
                new Tuple<string, int, int, int>("Green Peas", 3, 0, 40)
            } 
        },
    };
    public void Add(List<Ingredient> ingredients)
    {
        Stock.AddRange(ingredients);        
    }

    public void Remove(List<Ingredient> servingIngredients)
    {
        foreach (var ingredient in servingIngredients)
        {
            Stock.Remove(ingredient);
        }
    }

    public List<Ingredient> GetIngredients(MenuItem menuItem)
    {
        List<Ingredient> menuItemIngredients = new List<Ingredient>();
        List<Tuple<string, int, int, int>> ingredientsData = KitchenMenu[menuItem];
        foreach (var ingredientData in ingredientsData)
        {
            string name = ingredientData.Item1;
            int freshness = ingredientData.Item2;
            int expirationTerm = ingredientData.Item3;
            int price = ingredientData.Item4;
            Ingredient newIngredient = new Ingredient(name, freshness, expirationTerm, price);
            menuItemIngredients.Add(newIngredient);
        }
        return menuItemIngredients;
    }
}