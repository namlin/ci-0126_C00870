using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ExamTwo.Models;
using ExamTwo.Repositories;

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

    public string ProcessOrder(OrderRequestModel request)
    {
      if (request.Order == null || request.Order.Count == 0)
        throw new ArgumentException("La orden está vacía.");

      if (request.Payment.TotalAmount <= 0)
        throw new ArgumentException("Debe ingresar dinero para comprar café.");

      Dictionary<string, int> coffeeAmounts = coffeeMachineRepository.GetCoffeeAmounts();
      Dictionary<string, int> coffeePrices = coffeeMachineRepository.GetCoffeePrices();
      Dictionary<int, int> coinInventory = coffeeMachineRepository.GetCoinInventory();

      // Verificar el stock:
      foreach (var item in request.Order) {
        if (!coffeeAmounts.ContainsKey(item.Key))
          throw new ArgumentException($"El café '{item.Key}' no existe.");

        if (item.Value > coffeeAmounts[item.Key])
          throw new ArgumentException($"No hay suficientes {item.Key} en la máquina.");
      }

      int totalCost = this.CalculateTotalCost(request, coffeePrices);

      int change = request.Payment.TotalAmount - totalCost;

      // Calcular vuelto:
      var breakdown = this.CalculateChange(change, coinInventory);

      if (breakdown == null)
        throw new InvalidOperationException("Fallo al realizar la compra: no hay suficiente cambio.");

      // Generar el response:
      string message = $"Su vuelto es de {change} colones.\nDesglose:\n";

      foreach (var entry in breakdown)
        message += $"{entry.Value} moneda(s) de {entry.Key}\n";

      return message;
    }

    public Dictionary<int, int>? CalculateChange(int amount, Dictionary<int, int> coinInventory) {
      var result = new Dictionary<int, int>();
      int remaining = amount;

      foreach (int coin in coinInventory.Keys.OrderByDescending(c => c)) {
        int use = Math.Min(remaining / coin, coinInventory[coin]);

        if (use > 0) {
          result[coin] = use;
          remaining -= coin * use;
        }
      }

      if (remaining > 0)
        return null;

      return result;
    }

    public int CalculateTotalCost(OrderRequestModel request, Dictionary<string, int> coffeePrices) {
      int totalCost = request.Order.Sum(item =>
        coffeePrices[item.Key] * item.Value
      );

      if (request.Payment.TotalAmount < totalCost)
        throw new ArgumentException("Dinero insuficiente para completar la compra.");

      return totalCost;
    }
  }
}
