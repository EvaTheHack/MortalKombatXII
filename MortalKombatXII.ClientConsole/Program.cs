using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalKombatXII.ClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var playerService = new PlayerService();
            var menu = new ConsoleMenu(playerService);
            
            while (true)
            {
                menu.Start();
            }
            
        }
    }
}
