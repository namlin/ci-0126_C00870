using ExamTwo.Models;
using ExamTwo.Repositories;
using ExamTwo.Services;

namespace ExamTwo.Services
{
  public class CoffeeMachineService
  {
    private readonly CoffeeMachineRepository coffeeMachineRepository;

    public CoffeeMachineService()
    {
      this.coffeeMachineRepository = new CoffeeMachineRepository();
    }

    public Dictionary<string, int> GetCoffeeAmounts() {
      return this.coffeeMachineRepository.GetCoffeeAmounts();
    }

    public Dictionary<string, int> GetCoffeePrices() {
      return this.coffeeMachineRepository.GetCoffeePrices();
    }

   public Dictionary<int, int> GetCoinInventory() {
      return this.coffeeMachineRepository.GetCoinInventory();
    }
  }
}
