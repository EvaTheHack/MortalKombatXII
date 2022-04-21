using MortalKombatXII.ClientConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MortalKombatXII.ClientConsole
{
    public class ConsoleMenu
    {
        private const int DELAY_MILLISECONDS = 1000;
        private readonly PlayerService _playerService = new PlayerService();
        private readonly Player _player;
        public ConsoleMenu()
        {
            _player = _playerService.CreatePlayer();
        }

        public bool Start()
        {
            Console.Clear();
            Console.WriteLine($"Your name is {_player.Name}");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create room");
            Console.WriteLine("2) Connect to room");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    CreateRoom();
                    return true;
                case "2":
                    ConnectToRoom();
                    return true;
                case "3":
                    return false;
                default:
                    Console.WriteLine("Unknown option");
                    return true;
            }
        }

        private void CreateRoom()
        {
            var room = _playerService.CreateRoom(_player.Id);
            Console.WriteLine($"Room name - {room.Name}");
            RoomStatus status;

            do
            {
                Thread.Sleep(DELAY_MILLISECONDS);
                var roomStatus = _playerService.GetRoomStatus(room.Id);
                status = roomStatus.Status;

                ShowRoomStatus(roomStatus);
            }
            while (status != RoomStatus.Finished);
        }

        private void ConnectToRoom()
        {
            var rooms = _playerService.GetRooms();
            if (!rooms.Any())
            {
                Console.WriteLine("Create room for game");
                return;
            }

            ShowAllRooms(rooms);

            var room = ConnectToRoom(rooms);
            if(room == null)
            {
                Console.WriteLine("You cannot connect to this room");
                return;
            }
            RoomStatus status;

            do
            {
                Thread.Sleep(DELAY_MILLISECONDS);

                var roomStatus = _playerService.GetRoomStatus(room.Id);
                status = roomStatus.Status;

                ShowRoomStatus(roomStatus);
            } while (status != RoomStatus.Finished);
        }

        private Room ConnectToRoom(List<Room> rooms)
        {
            try
            {
                Console.WriteLine("Please choose the room");
                var choice = Convert.ToInt32(Console.ReadLine());
                var room = rooms[choice - 1];
                _playerService.ConnectToRoom(room.Id, _player.Id);
                return room;
            }
            catch
            {
                return null;
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
                        Console.Write($"\r{w.Key} - {w.Value} HP");
                        Console.WriteLine();
                    }
                    break;
                case RoomStatus.Finished:
                    Console.WriteLine($"Battle finished. Warrior {roomStatus.Winner} is champion");
                    return;
            }
        }

        private void ShowAllRooms(List<Room> rooms)
        {
            var count = 1;

            foreach (var r in rooms)
            {
                Console.WriteLine($"{count++}. Name: {r.Name}  Players: {r.Warriors.Count}/2 {r.Winner}");
            }
        }
    }
}
