using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MortalKombatXII.Core.Models;
using MortalKombatXII.Core.Repositories;

namespace MortalKombatXII.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly PlayersRepository _players;
        private readonly RoomsRepository _rooms;
        private readonly IMapper _mapper;

        public RoomsController(PlayersRepository player, RoomsRepository rooms, IMapper mapper)
        {
            _players = player;
            _rooms = rooms;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_rooms.GetRooms());
        }

        [HttpGet("open")]
        public IActionResult GetOpen()
        {
            return Ok(_rooms.GetRooms().Where(x => x.Status == RoomStatus.Pending));
        }

        [HttpPost("create/{playerId}")]
        public IActionResult Create(Guid playerId)
        {
            var player = _players.Get(playerId);

            return Created("", _rooms.CteateRoom(player));
        }

        [HttpPost("connect/{roomId}/{playerId}")]
        public IActionResult Connect(Guid roomId, Guid playerId)
        {
            var player = _players.Get(playerId);
            
            var room = _rooms.ConnectToRoom(roomId, player);

            return Ok(room);
        }

        [HttpGet("{roomId}/status")]
        public IActionResult GetState(Guid roomId)
        {
            var room = _rooms.GetRoom(roomId);
            switch (room.Status)
            {
                case RoomStatus.Pending:
                    return Ok(_mapper.Map<PendingRoomStatus>(room));
                case RoomStatus.Battle:
                    return Ok(_mapper.Map<BattleRoomStatus>(room));
                case RoomStatus.Finished:
                    return Ok(_mapper.Map<FinishedRoomStatus>(room));
                default:
                    return BadRequest("Unknown room status");
            }
        }
    }
}
