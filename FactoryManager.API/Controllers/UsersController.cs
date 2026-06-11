using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("Users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get ()
    {
        return Ok("API funcionando");
    }
}