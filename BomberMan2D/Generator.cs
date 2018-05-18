using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public static class Generator
    {
        public static string[] GenerateLevel(string fileName, int numOfWallsToGenerate)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            string[] lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains("0") && numOfWallsToGenerate > 0)
                {
                    int rnd = RandomManager.Instance.Random.Next(0, 10);

                    if (rnd > 0 && rnd < 4)
                    {
                        lines[i].Replace('0', '2');
                        numOfWallsToGenerate--;
                    }
                }
            }

            return lines;
        }

        public static void CreateMap(string fileName)
        {
            string[] newMapLines = GenerateLevel(fileName, 10);

            File.WriteAllLines("new.csv", newMapLines);
        }
    }
}
