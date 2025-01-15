using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    internal class Ship
    {
        private int shipSize;
        private int[,] coordinates = new int[2, 2];
        // Horizontal
        private int[] direction = { 0, 1 };
        int sumOfHits = 0;
        public Ship(int shipSize)
        {
            this.shipSize = shipSize;
        }
        public void SetPosition(bool[,] ourShips, int x, int y, int d)
        {
            SetDirection(d);
            // start
            coordinates[0, 0] = x;
            coordinates[0, 1] = y;
            // end
            coordinates[1, 0] = x + (shipSize - 1) * direction[0];
            coordinates[1, 1] = y + (shipSize - 1) * direction[1];
            
            ourShips[x, y] = true;
            
            for (int i = 1; i < shipSize; i++)
                 ourShips[x + i * direction[0], y + i * direction[1]] = true;
        }
        private void SetDirection(int d)
        {
            this.direction[0] = d;
            this.direction[1] = 1 - d;
        }
        private void SumHits(int sum)
        {
            sum++;
        }
    }
}
