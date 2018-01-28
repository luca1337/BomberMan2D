﻿using System;
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
        private int                     blockSize = 50;
        private static int                     rows;
        private static int                     columns;
        private static List<int>               cells;
        private float                   offset;
        private static Vector2          playerSpawnPoint;
        private static List<Vector2> powerUpSpawnPoints = new List<Vector2>();

        public Map(string fileName)
        {
            //represent blockSize * 2 (2 times blocksize)
            offset = 2 * blockSize;
            cells  = new List<int>();

            ReadFromFile(fileName);
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int iterator = 0; iterator < cells.Count; iterator++)
            {
                if (cells[iterator] == 3)
                {
                    Spawn(new Tile(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) + offset)), "Wall", true));
                }
                else if (cells[iterator] == 2)
                {
                    Spawn(new Tile(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) + offset)), "Obstacle", true));
                }
                else if (cells[iterator] == 5)
                {
                    playerSpawnPoint = new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) + offset));
                }
                else if (cells[iterator] == 5 || cells[iterator] == 0)
                {
                    powerUpSpawnPoints.Add(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) + offset)));
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


        public static bool GetIndex(bool explosion, int x, int y) // for explosion spawn
        {
            int index = ((x/ 51) + (columns - 1) * (y /51));

            if (explosion)
            {
                if (cells[index ] == 0 || cells[index] == 5 || cells[index] == 12)
                    return true;

                if (cells[index] == 3)
                    return false;

                if (cells[index] == 2)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (cells[index] == 0)
                    return true;

                return false;
            }
        }

        public static Vector2 GetPlayerSpawnPoint()
        {
            return playerSpawnPoint;
        }

        public static List<Vector2> GetPowerupSpawnPoint()
        {
            return powerUpSpawnPoints; ;
        }
    }
}
