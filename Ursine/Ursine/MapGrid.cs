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

        public MapGrid(int x, int y)
        {
            MapArray = new int[x, y];
            CandArray = new int[x, y];
            PlayerAStarArray = new int[x, y];
        }

        public void ClearGrid()
        {
            for (int x = 0; x < MapArray.GetLength(0); x++)
            {
                for (int y = 0; y < MapArray.GetLength(1); y++)
                {
                    MapArray[x, y] = 0;
                    CandArray[x, y] = 0;
                    PlayerAStarArray[x, y] = 0;
                }
            }

        }

        public void PlotAStar(int x, int y, int playerX, int playerY)
        {
            PlayerAStarArray[x, y] = 0;
            CandArray[x, y] = 0;

            PlayerAStarArray[playerX, playerY] = 999;
            CandArray[playerX, playerY] = 999;
            
            int currentX = x;
            int currentY = y;
            int iterationCount = 1;

            int XWeighting = 0;
            int YWeighting = 0;

            //Use CAndArray to loop through Astar Array until player is hit
            //call a function to lad the terrain into these array a 999

            while (CandArray[playerX, playerY] != 1)
            {
                XWeighting = 0;
                YWeighting = 0;

                if (x < playerX) { XWeighting = +1; }  //heuristic direction to be headed in
                if (x > playerX) { XWeighting = -1; }

                if (y < playerY) { YWeighting = +1; }
                if (y > playerY) { YWeighting = -1; }

                for (int xx = -1; xx <= 1; xx++) //loop around a single cell
                {
                    for (int yy = -1; yy <= 1; yy++)
                    {
                        if ((xx + x) < CandArray.GetLength(0)
                            && (xx + x) >= 0
                            && (yy + y) < CandArray.GetLength(1)
                            && (yy + y) >= 0
                            )
                        {
                            if (CandArray[xx + x, yy + y] == 0)
                            {
                                CandArray[xx + x, yy + y] = 1;
                                PlayerAStarArray[xx + x, yy + y] = iterationCount;
                            }
                        }
                    }


                    //for (int pxx = -1; pxx <= 1; pxx++) //loop around a single cell
                    //{
                    //    for (int pyy = -1; pyy <= 1; pyy++)
                    //    {
                    //        if ((pxx + x) < CandArray.GetLength(0)
                    //            && (pxx + x) >= 0
                    //            && (pyy + y) < CandArray.GetLength(1)
                    //            && (pyy + y) >= 0
                    //            )
                    //        {
                    //            if (CandArray[pxx + x, pyy + y] == 1)
                    //            {
                    //                PlayerAStarArray[pxx + x, pyy + y] = iterationCount;
                    //                CandArray[pxx + x, pyy + y] = 2;
                    //            }
                    //        }
                    //    }
                    //}

                }
                iterationCount++;
                x += XWeighting;    //start looking in the heuristically right direction
                y += YWeighting;

                if (iterationCount > 50)
                { break; }

            }

    
        }
    }
}
