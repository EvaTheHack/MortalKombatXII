using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MortalKombatXII.Core.Repositories;

namespace MortalKombatXII.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayersRepository _players;

        public PlayersController(PlayersRepository players)
        {
            _players = players;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_players.CreatePlayer());
        }
    }
}
