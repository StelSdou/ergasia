using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    internal class Program
    {
        // Black table, false
        private static bool[,] MyShips = new bool[10, 10];
        // Green table, null
        private static bool?[,] myHits = new bool?[10, 10];

        // Blue table, false
        private static bool[,] enemyShips = new bool[10, 10];
        // Red table, null
        private static bool?[,] enemyHits = new bool?[10, 10];

        static void Main(string[] args)
        {
            Ship submarine = new Ship(2);
            submarine.SetPosition(MyShips, 3, 5, 0);

            Ship battleship = new Ship(3);
            battleship.SetPosition(MyShips, 6, 6, 1);

            Ship destroyer = new Ship(4);
            destroyer.SetPosition(MyShips, 0, 4, 0);

            Ship aircraft_carrier = new Ship(5);
            aircraft_carrier.SetPosition(MyShips, 1, 3, 1);

            print();
            Console.ReadLine();
        }

        static void MyAttack(int x, int y)
        {
            if (enemyShips[x, y])
                myHits[x, y] = true;
            else
                myHits[x, y] = false;
        }

        static void print()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    string x = " ";
                    if (MyShips[i, j])
                        x = "1";
                    else
                        x = " ";
                    Console.Write(x + "|");
                }
                Console.WriteLine();
            }
        }
    }
}
