using System;

namespace MortalKombatXII.ClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var menu = new ConsoleMenu();
            bool showMenu = true;

            while (showMenu)
            {
                showMenu = menu.Start();
                Console.WriteLine("Please click enter to continue");
                Console.ReadLine();
            }

        }
    }
}
