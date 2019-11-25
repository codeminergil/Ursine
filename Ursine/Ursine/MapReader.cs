
namespace Ursine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class MapReader
    {
        public string MapPath { get; set; }

        public List<Terrain> MapTerrainList { get; set; }



        public MapReader(string map)
        {
            MapPath = map;
        }

        public void PlotMap(List<Texture2D> TextureList)
        {
            Terrain ter;

            List<Terrain> TerList = new List<Terrain>();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = MapPath;
            string result;

           string[] t = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            //string[,] item = new string[40, 40];
            string[] line = result.Split(new[] { "\r\n", "\r", "\n" },
                                                StringSplitOptions.None
                                            );
            MapGrid mapGrid = new MapGrid(40, 40);

            //File.ReadAllLines(result);
            for (int y = 0; y < 40; y++)
            {
                for (int x = 0; x < 40; x++)
                {
                   // item[x, y] = line[y].Substring(x, 1);
                    mapGrid.MapArray[x,y] = Int32.Parse(line[y].Substring(x, 1)); 

                    if (mapGrid.MapArray[x, y] == 1)
                    { ter = new Terrain(x, y, 0, TextureList[0], 100, 50, true, 1); }

                    else if (mapGrid.MapArray[x, y] == 2)
                    { ter = new Terrain(x, y, 0, TextureList[1], 100, 50, false, 999); }

                    else if (mapGrid.MapArray[x, y] == 3)
                    { ter = new Terrain(x, y, 0, TextureList[2], 100, 50, true, 5); }

                    else if (mapGrid.MapArray[x, y] == 4)
                    { ter = new Terrain(x, y, 0, TextureList[3], 100, 50, true, 5); }

                    else if (mapGrid.MapArray[x, y] == 5)
                    { ter = new Terrain(x , y , 0, TextureList[4], 100, 50, true, 1); }

                    else
                    { ter = new Terrain(x, y, 0, TextureList[0], 100, 50, true, 1); }

                    TerList.Add(ter);
                  
                }
            }

        MapTerrainList = TerList;
        }

    }
}
