using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tictactoe_console_game
{
    internal class Player
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    name = value;
                }
                else
                {
                    Console.WriteLine("name should contain at least 1 character");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }

        public int winCount;
    }
}
