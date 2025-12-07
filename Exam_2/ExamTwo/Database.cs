namespace ExamTwo.Controllers
{
  public class Database
  {
    // Coffee amounts:
    public Dictionary<string, int> coffeeAmounts = new Dictionary<string, int>
    {
      { "Americano", 10 },
      { "Cappuccino", 8 },
      { "Lates", 10 },
      { "Mocaccino", 15 }
    };

    // Coffee prices:
    public Dictionary<string, int> coffeePrices = new Dictionary<string, int>
    {
      { "Americano", 950 },
      { "Cappuccino", 1200 },
      { "Lates", 1350 },
      { "Mocaccino", 1500 }
    };

    // Coffee machine coin inventory:
    public Dictionary<int, int> coinInventory = new Dictionary<int, int>
    {
      // Coin type, amount:
      { 500, 20 },
      { 100, 30 },
      { 50, 50 },
      { 25, 25 }
    };
  }
}
