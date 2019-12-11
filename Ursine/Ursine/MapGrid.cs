namespace Ursine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class MapGrid
    {
        public int[,] MapArray { get; set; }

        public int[,] CandArray { get; set; }

        public int[,] PlayerAStarArray { get; set; }

        public int[,] LocalArray { get; set; }

        public MapGrid(int x, int y)
        {
            MapArray = new int[x, y];
            CandArray = new int[x, y];
            PlayerAStarArray = new int[x, y];
        }

        public void ClearGrid(Terrain[,] t)
        {
            for (int x = 0; x < MapArray.GetLength(0); x++)
            {
                for (int y = 0; y < MapArray.GetLength(1); y++)
                {
                    int tt = t[x, y].PassCost;
                    MapArray[x, y] = t[x, y].PassCost;
                    CandArray[x, y] = 0;
                    if (t[x, y].PassCost == 999) { CandArray[x, y] = 1; }
                    PlayerAStarArray[x, y] = t[x, y].PassCost;

                }
            }

            LocalArray = new int[,] {{ 999, 999, 999 },{ 999,999,999 },{ 999,999,999 } };

        }

        public void PlotAStar(int x, int y, int playerX, int playerY)
        {
            if (PlayerAStarArray[x, y] != 999)
            {
                PlayerAStarArray[x, y] = 999;
                CandArray[x, y] = 1;
            }
            

            PlayerAStarArray[playerX, playerY] = 888;
            CandArray[playerX, playerY] = 999;

            int lowVal = 0;
            int prevLowVal = 0;
            int currentX = x;
            int currentY = y;
            int iterationCount = 1;

            int XWeighting = 0;
            int YWeighting = 0;
            //Use CAndArray to loop through Astar Array until player is hit
            //call a function to lad the terrain into these array a 99

            while (CandArray[playerX, playerY] != 1)
            {
                //XWeighting = 0;
                //YWeighting = 0;

                //LocalArray[1, 1] = 999;

                for (int xx = -1; xx <= 1; xx++) //loop around a single cell
                {
                    for (int yy = -1; yy <= 1; yy++)
                    {

                        if ((xx + x) < CandArray.GetLength(0)
                            && (xx + x) >= 0
                            && (yy + y) < CandArray.GetLength(1)
                            && (yy + y) >= 0
                            && (PlayerAStarArray[xx+x, yy+y] != 999)
                            )
                        {
                            if (CandArray[xx + x, yy + y] == 0)
                            {
                                CandArray[xx + x, yy + y] = 1;
                                PlayerAStarArray[xx + x, yy + y] = iterationCount + CalcHeur(playerX, playerY, xx + x, yy + y);
                                LocalArray[1 + xx, 1 + yy] = CalcHeur(playerX, playerY, xx + x, yy + y); 
                            }
                        }
                    }

                }
                iterationCount++;

                lowVal = LocalArray[0, 0];
                for (int p = 0; p < 3; p++)
                {
                    for (int q = 0; q < 3; q++)
                    {
                        if (LocalArray[p, q] < lowVal && !(p==1 && q ==1))
                        {
                            lowVal = LocalArray[p, q];
                        }
                    }
                }

                if (LocalArray[0, 0] == lowVal) { XWeighting = -1; YWeighting = -1; }
                if (LocalArray[1, 0] == lowVal) { XWeighting = 0; YWeighting = -1; }
                if (LocalArray[2, 0] == lowVal) { XWeighting = +1; YWeighting = -1; }

                if (LocalArray[0, 1] == lowVal) { XWeighting = -1; YWeighting = 0; }
                if (LocalArray[2, 1] == lowVal) { XWeighting = +1; YWeighting = 0; }

                if (LocalArray[0, 2] == lowVal) { XWeighting = -1; YWeighting = +1; }
                if (LocalArray[1, 2] == lowVal) { XWeighting = 0; YWeighting = +1; }
                if (LocalArray[2, 2] == lowVal) { XWeighting = +1; YWeighting = +1; }


                //if (x < playerX) { XWeighting = +1; }  //heuristic direction to be headed in
                //if (x > playerX) { XWeighting = -1; }

                //if (y < playerY) { YWeighting = +1; }
                //if (y > playerY) { YWeighting = -1; }


                x += XWeighting;    //start looking in the heuristically right direction
                y += YWeighting;
                



                if (iterationCount > 50)
                { break; }

            }

    
        }

        public int CalcHeur(int px, int py, int gx, int gy)
        {
            return (int)(Math.Pow(Math.Abs(gx - px),2) + Math.Pow(Math.Abs(gy - py),2));
           // return (int)(Math.Abs(gx - px) + Math.Abs(gy - py));
        }
    }
}
