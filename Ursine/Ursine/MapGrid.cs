namespace Ursine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Linq;

    public class MapGrid
    {
        public int[,] MapArray { get; set; }

        public int[,] CandArray { get; set; }

        public int[,] PlayerAStarArray { get; set; }

        public int[,] LocalArray { get; set; }

        List<Terrain> OpenList  { get; set; }

        List<Terrain> ClosedList { get; set; }

        public List<Terrain> Path { get; set; }

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
                    CandArray[x, y] = 1;//ignore
                    if (t[x, y].PassCost == 999) { CandArray[x, y] = 1; }
                    PlayerAStarArray[x, y] = t[x, y].PassCost;
                }
            }

            LocalArray = new int[,] {{ 999, 999, 999 },{ 999,999,999 },{ 999,999,999 } };

        }

        public void PlotAStar(int targX, int targY, int startX, int startY, Terrain[,] terArray)    //startX is where you click
        {
            GeometryFactory gf = new GeometryFactory();

            if (PlayerAStarArray[targX, targY] != 999)
            {
                PlayerAStarArray[targX, targY] = 888;
            }

            int iterationCount = 0;
            int gTemp;
            int hTemp;

            Terrain targetCell = terArray[targX, targY];
            Terrain startCell = terArray[startX, startY];


            PlayerAStarArray[startX, startY] = 888;
            OpenList = new List<Terrain>();
            ClosedList = new List<Terrain>();
            Terrain currentCell = terArray[startX, startY];

            OpenList.Add(currentCell);  //drop in start

            while (OpenList.Count>0)
            {
                currentCell = OpenList[0];
                for(int i = 0; i < OpenList.Count(); i++)
                {
                    //CalcHeur(t.X, t.Y, targX, targY, out gTemp, out hTemp);
                    //t.g = gTemp;
                    //t.h = hTemp;
                    if (OpenList[i].f < currentCell.f || (OpenList[i].f == currentCell.f && OpenList[i].h < currentCell.h))
                    {
                        currentCell = OpenList[i];  //cheapest F cost cell.
                    }
                }

                OpenList.Remove(currentCell);
                ClosedList.Add(currentCell);

                if (currentCell.X == targX && currentCell.Y == targY)
                {
                    RetracePath(startCell, targetCell);
                    return;  //found the target cell
                }
                
                for (int xx = -1; xx <= 1; xx++) //loop around a single perimiter cell
                {
                    for (int yy = -1; yy <= 1; yy++)
                    {
                        if ((xx + currentCell.X) < PlayerAStarArray.GetLength(0)
                            && (xx + currentCell.X) >= 0
                            && (yy + currentCell.Y) < PlayerAStarArray.GetLength(1)
                            && (yy + currentCell.Y) >= 0
                            && !(xx == 0 && yy == 0) //not looking at centre square
                            )
                        {
                            if ((terArray[xx + currentCell.X, yy + currentCell.Y].Passable = false)
                                || (PlayerAStarArray[xx + currentCell.X, yy + currentCell.Y] == 999)
                                || (ClosedList.Contains(terArray[xx + currentCell.X, yy + currentCell.Y])))
                            {
                                continue;   //igonre this cell and jump to the next one                                
                            }


                            //g - cost of getting to that node from starting node.
                            //h - cost of getting to the goal node from current node. **

                            int moveCostToPerimiter = (int)currentCell.g + CalcHeur(currentCell, terArray[xx + currentCell.X, yy + currentCell.Y]);

                            if (moveCostToPerimiter < terArray[xx + currentCell.X, yy + currentCell.Y].g
                                || !OpenList.Contains(terArray[xx + currentCell.X, yy + currentCell.Y]))
                            {
                                terArray[xx + currentCell.X, yy + currentCell.Y].g = moveCostToPerimiter;
                                terArray[xx + currentCell.X, yy + currentCell.Y].h = CalcHeur(terArray[xx + currentCell.X, yy + currentCell.Y], targetCell/*, out gTemp, out hTemp*/);

                                PlayerAStarArray[xx + currentCell.X, yy + currentCell.Y] = (int)terArray[xx + currentCell.X, yy + currentCell.Y].f;

                                terArray[xx + currentCell.X, yy + currentCell.Y].Parent = currentCell;

                                if (!OpenList.Contains(terArray[xx + currentCell.X, yy + currentCell.Y]))
                                {
                                    OpenList.Add(terArray[xx + currentCell.X, yy + currentCell.Y]);
                                }
                            }

                        }   //if within array bouds
                    } //yy
                }//xx
                iterationCount++;
                if (iterationCount > 1000)
                { break; }
            }//while
        }

        public int CalcHeur(Terrain STerrain, Terrain TTerrain/*, out int G, out int H*/)
        {
            //return (int)(Math.Pow(Math.Abs(gx - sx),2) + Math.Pow(Math.Abs(gy - sy),2));
            //F = G + H
            //g - cost of getting to that node from starting node.
            //h - cost of getting to the goal node from current node.


           // H = Math.Abs(sx - tx) + Math.Abs(sy - ty);  //distance from goal

            int dx = Math.Abs(STerrain.X - TTerrain.X);
            int dy = Math.Abs(STerrain.Y - TTerrain.Y);

            if (dx > dy)
            {
                /*G =*/return 14 * dy + (10 * (dx - dy));
            }
            else
            {
                /*G =*/return 14 * dx + (10 * (dy - dx));
            }

            //https://www.youtube.com/watch?v=mZfyt03LDH4&t=155s
            //int F = G + H;

          //  return F = G + H;
        }

        void RetracePath(Terrain startCell, Terrain endCell)
        {
            List<Terrain> path = new List<Terrain>();
            Terrain currentCell = endCell;

            while (currentCell != startCell)
            {
                path.Add(currentCell);
                currentCell = currentCell.Parent;
            }
            path.Reverse();

            Path = path;

        }

    }
}
