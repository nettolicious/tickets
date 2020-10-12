using Microsoft.AspNetCore.Mvc;
using System;

namespace Nettolicious.Tickets.API.Controllers {
  [ApiController]
  [Route("ping")]
  public class PingController : ControllerBase {

    [HttpGet]
    public IActionResult Get() {
      string message = string.Format("Alive at {0}", DateTimeOffset.Now);
      return Ok(message);
    }
  }
}
