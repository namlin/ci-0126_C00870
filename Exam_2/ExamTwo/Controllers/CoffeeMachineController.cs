using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ExamTwo.Services;

namespace ExamTwo.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CoffeeMachineController : ControllerBase {
    private readonly CoffeeMachineService coffeeMachineService;

    public CoffeeMachineController() {
      this.coffeeMachineService = new CoffeeMachineService();
    }

    [HttpGet("getCoffeeAmounts")]
    public ActionResult<Dictionary<string, int>> GetCoffeeAmounts() {
      return Ok(this.coffeeMachineService.GetCoffeeAmounts());
    }

    [HttpGet("getCoffeePrices")]
    public ActionResult<Dictionary<string, int>> GetCoffeePrices() {
      return Ok(this.coffeeMachineService.GetCoffeePrices());
    }

    [HttpGet("getCoinInventory")]
    public ActionResult<Dictionary<int, int>> GetCoinInventory() {
      return Ok(this.coffeeMachineService.GetCoinInventory());
    }
  }
}
