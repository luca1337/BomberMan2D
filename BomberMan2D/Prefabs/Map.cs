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
        private int[] cells;
        public Map()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject.Spawn(new Tile(new Vector2(i % 20 * 50, i / 20 * 50)));
            }
        }

        private void ReadFromFile(string csvFileName)
        {
            string[] lines = File.ReadAllLines(csvFileName);
            rows = lines.Length;

            foreach (string t1 in lines)
            {
                string[] values = t1.Trim().Split(',');
                if (columns == 0)
                    columns = values.Length;

                cells = new int[rows * columns];

                for (int i = 0; i < values.Length; i++)
                {
                    int value;
                    string currentVal = values[i].Trim();
                    bool success      = int.TryParse(currentVal, out value);
                    if (success)
                        cells[i] = value;

                }
             //  foreach (string t in values)
             //  {
             //      int value;
             //      string currentVal = t.Trim();
             //      bool success = int.TryParse(currentVal, out value);
             //
             //      if (success)
             //          map.Add(value);
             //  }
            }
        }
    }
}
