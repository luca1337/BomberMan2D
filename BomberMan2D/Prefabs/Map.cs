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
        private int blockSize = 50;
        private static int rows;
        private static int columns;
        private static List<int> cells;
        private float offset;
        private static Vector2 playerSpawnPoint;
        private static List<Vector2> powerUpSpawnPoints = new List<Vector2>();
        private bool firstTimeIteration;
        private Node[] mapNodes;

        public Map(string fileName)
        {
            cells = new List<int>();
            firstTimeIteration = true;
            mapNodes = new Node[cells.Count];

            ReadFromFile(fileName);
            GenerateMap();
            GenerateNodes();
            GenerateNeighborNode();
        }

        private void GenerateMap()
        {
            for (int iterator = 0; iterator < cells.Count; iterator++)
            {
                if (cells[iterator] == 3)
                {
                    Spawn(new Tile(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize))), "Wall", true));
                }
                else if (cells[iterator] == 2)
                {
                    Spawn(new Tile(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) )), "Obstacle", true));
                }
                else if (cells[iterator] == 5)
                {
                    playerSpawnPoint = new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) ));
                }
                else if (cells[iterator] == 5 || cells[iterator] == 0)
                {
                    powerUpSpawnPoints.Add(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize) )));
                }
            }
        }

        private void GenerateNodes()
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < (columns - 1); x++)
                {
                    int index = (y / 50) * (columns - 1) + (x / 50);

                    if (cells[index] == 0 || cells[index] == 5 || cells[index] == 12)
                    {
                        mapNodes[index] = new Node(1, new Vector2(x, y));
                    }
                }
            }
        }

        private void GenerateNeighborNode()
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < (columns - 1); x++)
                {
                    int index = (y / 50) * (columns - 1) + (x / 50);

                    if (mapNodes[index] == null)
                        continue;

                    // top
                    Node top = GetNodeByIndex(x, y - 1);
                    if (top != null)
                        mapNodes[index].AddNeighbour(top);

                    // right
                    Node right = GetNodeByIndex(x + 1, y);
                    if (right != null)
                        mapNodes[index].AddNeighbour(right);

                    // bottom
                    Node bottom = GetNodeByIndex(x, y + 1);
                    if (bottom != null)
                        mapNodes[index].AddNeighbour(bottom);

                    // left
                    Node left = GetNodeByIndex(x - 1, y);
                    if (left != null)
                        mapNodes[index].AddNeighbour(left);
                }
            }
        }

        private void ReadFromFile(string csvFileName)
        {
            string[] lines = File.ReadAllLines(csvFileName);
            rows = lines.Length;

            foreach (string t1 in lines)
            {
                string[] values = t1.Trim().Split(',', ' ');
                if (columns == 0)
                    columns = values.Length;
                if (firstTimeIteration)
                {
                    //represent the insertion of two rows

                    for (int i = 0; i < (values.Length - 1) * 2; i++)
                    {
                        cells.Add(-10);
                    }
                    firstTimeIteration = false;
                }


                for (int i = 0; i < values.Length; i++)
                {
                    int value;
                    string currentVal = values[i].Trim();
                    bool success = int.TryParse(currentVal, out value);
                    if (success)
                        cells.Add(value);
                }
            }
        }


        public static bool GetIndex(bool explosion, int x, int y) // for explosion spawn
        {
            int index = (y / 50) * (columns - 1) + (x / 50);

            if (explosion)
            {
                if (cells[index] == 0 || cells[index] == 5 || cells[index] == 12)
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

        public Node GetNodeByIndex(int x, int y)
        {
            if (x < 0 || x > (columns - 1))
                return null;
            if (y < 0 || y > rows)
                return null;

            int index = (y / 50) * (columns - 1) + (x / 50);
            return mapNodes[index];
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
