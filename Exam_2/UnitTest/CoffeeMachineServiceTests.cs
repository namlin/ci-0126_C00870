using ExamTwo.Models;
using ExamTwo.Services;

namespace UnitTest
{
  public class CoffeeMachineServiceTests
  {
    private CoffeeMachineService coffeeMachineService;

    [SetUp]
    public void Setup() {
      this.coffeeMachineService = new CoffeeMachineService();
    }

    [Test]
    public void ProcessOrder_InvalidOrder() {
      var request = new OrderRequestModel {
        Order = new Dictionary<string, int>(),
        Payment = new PaymentModel { TotalAmount = 1000 }
      };

      Assert.Throws<ArgumentException>(() => this.coffeeMachineService.ProcessOrder(request));
    }

    [Test]
    public void ProcessOrder_NotEnoughInventory() {
      var request = new OrderRequestModel {
        Order = new Dictionary<string, int> { { "Expresso", 9999 } },
        Payment = new PaymentModel { TotalAmount = 99 }
      };

      Assert.Throws<ArgumentException>(() => this.coffeeMachineService.ProcessOrder(request));
    }

    [Test]
    public void NotEnoughMoney_ThrowsException() {
      var request = new OrderRequestModel {
        Order = new Dictionary<string, int> { { "Americano", 1 } },
        Payment = new PaymentModel { TotalAmount = 0 }
      };

      Assert.Throws<ArgumentException>(() => this.coffeeMachineService.ProcessOrder(request));
    }

    [Test]
    public void CalculateChange_NotPossible_ReturnsNull() {
      var coinInventory = new Dictionary<int, int>
      {
        { 100, 0 },
        { 50, 0 }
      };

      var result = this.coffeeMachineService.CalculateChange(150, coinInventory);

      Assert.IsNull(result);
    }
  }
}
