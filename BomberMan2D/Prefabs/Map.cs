using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;
using System.IO;

namespace BomberMan2D.Prefabs
{
    public class Map : GameObject
    {

        private int rows;
        private int columns;
        private List<int> cells;
        private float offset;
        public Map(string fileName)
        {
            cells = new List<int>();
            ReadFromFile(fileName);
            offset = 100f;
            GenerateMap();

        }

        private void GenerateMap()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i] == 3)
                {
                    Spawn(new Tile(new Vector2(i % (columns - 1) * 50, (i / (columns - 1) * 50) + offset), "wall"));
                }
                else if (cells[i] == 2)
                {
                    Spawn(new Tile(new Vector2(i % (columns - 1) * 50, (i / (columns - 1) * 50) + offset), "obstacle"));
                }
            }
        }

        private void ReadFromFile(string csvFileName)
        {
            string[] lines = File.ReadAllLines(csvFileName);
            rows = lines.Length;

            foreach (string t1 in lines)
            {
                string[] values = t1.Trim().Split(',',' ');
                if (columns == 0)
                    columns = values.Length;

               for (int i = 0; i < values.Length; i++)
               {
                   int value;
                   string currentVal = values[i].Trim();
                   bool success      = int.TryParse(currentVal, out value);
                   if (success)
                        cells.Add(value);
                }
            }
        }
    }
}
