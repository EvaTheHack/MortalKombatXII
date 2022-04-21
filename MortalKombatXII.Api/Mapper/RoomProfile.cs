using AutoMapper;
using MortalKombatXII.Core.Models;

namespace MortalKombatXII.Api.Mapper
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, PendingRoomStatus>()
                .ForMember(x => x.CountPlayers, opt => opt.MapFrom(x => x.Warriors.Count));

            CreateMap<Room, BattleRoomStatus>()
                .ForMember(x => x.Warriors, opt => opt.MapFrom(x => x.Warriors.ToDictionary(x => x.Name, x => x.Health)));

            CreateMap<Room, FinishedRoomStatus>();
        }
    }
}
