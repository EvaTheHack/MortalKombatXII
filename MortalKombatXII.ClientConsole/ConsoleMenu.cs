using MortalKombatXII.ClientConsole.Models;
using System;
using System.Linq;
using System.Threading;

namespace MortalKombatXII.ClientConsole
{
    public class ConsoleMenu
    {
        private readonly PlayerService _playerService;
        private readonly Player _player;
        public ConsoleMenu(PlayerService playerService)
        {
            _playerService = playerService;
            _player = _playerService.CreatePlayerAsync().Result;
        }

        public void Start()
        {
            Console.WriteLine($"Hello, your nickname - {_player.Name}");
            Console.WriteLine("1. Create room");
            Console.WriteLine("2. Connect to room");
            GetChoice();
        }

        public void GetChoice()
        {
            var choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    CreateRoom();
                    Start();
                    break;
                case 2:
                    ConnectToRoom();
                    Start();
                    break;
                default:
                    break;
            }
        }


        private void CreateRoom()
        {
            var room = _playerService.CreateRoomAsync(_player.Id).Result;
            Console.WriteLine($"Room name - {room.Name}");
            while (true)
            {
                Thread.Sleep(1000);
                var roomStatus = _playerService.GetRoomStatusAsync(room.Id).Result;
                ShowRoomStatus(roomStatus);
                if(roomStatus.Status == RoomStatus.Finished)
                {
                    return;
                }
            }
        }

        private void ConnectToRoom()
        {
            var count = 1;
            var rooms = _playerService.GetRoomsAsync().Result;
            if (!rooms.Any())
            {
                Console.WriteLine("Create room for game");
                Start();
                return;
            }

            foreach (var room in rooms)
            {
                Console.WriteLine($"{count}. Name: {room.Name}  Players: {room.Warriors.Count}/3 {room.Winner}");
                count++;
            }

            while (true)
            {
                Console.WriteLine("Please choose the room");
                var choice = Convert.ToInt32(Console.ReadLine());
                var room = rooms[choice - 1];
                if(room.Warriors.Count == 3)
                {
                    Console.WriteLine("You cannot connect to this room");
                    Start();
                    return;
                }
                _playerService.ConnectToRoomAsync(room.Id, _player.Id);
                while (true)
                {
                    Thread.Sleep(1000);
                    var roomStatus = _playerService.GetRoomStatusAsync(room.Id).Result;
                    ShowRoomStatus(roomStatus);
                    if (roomStatus.Status == RoomStatus.Finished)
                    {
                        return;
                    }
                }

            }            
        }

        private void ShowRoomStatus(dynamic roomStatus)
        {
            switch (roomStatus.Status)
            {
                case RoomStatus.Pending:
                    Console.Write($"\rCurrently room {roomStatus.Name} has {roomStatus.CountPlayers} players");
                    break;
                case RoomStatus.Battle:
                    Console.WriteLine("There is a fight");
                    foreach (var w in roomStatus.Warriors)
                    {
                        Console.WriteLine($"{w.Key} - {w.Value} HP");
                    }
                    break;
                case RoomStatus.Finished:
                    Console.WriteLine($"Battle finished. Warrior {roomStatus.Winner} is champion");
                    return;
            }
        }
    }
}
