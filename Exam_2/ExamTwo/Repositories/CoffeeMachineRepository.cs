using ExamTwo.Controllers;
using ExamTwo.Models;

namespace ExamTwo.Repositories
{
  public class CoffeeMachineRepository
  {
    private readonly Database database;

    public CoffeeMachineRepository() {
      this.database = new Database();
    }

    public Dictionary<string, int> GetCoffeeAmounts() {
      return this.database.coffeeAmounts;
    }

    public Dictionary<string, int> GetCoffeePrices() {
      return this.database.coffeePrices;
    }

    public Dictionary<int, int> GetCoinInventory() {
      return this.database.coinInventory;
    }
  }
}
