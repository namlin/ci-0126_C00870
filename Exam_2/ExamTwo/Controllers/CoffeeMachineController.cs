using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ExamTwo.Models;
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

    [HttpPost("buyCoffee")]
    public IActionResult BuyCoffee([FromBody] OrderRequestModel request)
    {
      try {
        string message = coffeeMachineService.ProcessOrder(request);
        return Ok(message);
      }

      catch (ArgumentException ex) {
        return BadRequest(ex.Message);
      }

      catch (InvalidOperationException ex)
      {
        return StatusCode(500, ex.Message);
      }
    }
  }
}
