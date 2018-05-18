using System.Collections.Generic;
using BehaviourEngine;
using OpenTK;
using System.IO;
using BehaviourEngine.Pathfinding;

namespace BomberMan2D
{
    public class Map : GameObject, IMap
    {
        public Node[] MapNodes;
        public List<Tile> WalkablePass;

        private int blockSize = 1;
        private static int rows;
        private static int columns;
        private static List<int> cells;
        private static Vector2 playerSpawnPoint;
        private static List<Vector2> powerUpSpawnPoints = new List<Vector2>();
        private static List<Vector2> enemySpawnPoints = new List<Vector2>();
        private bool firstTimeIteration;


        public Map(string fileName)
        {
            WalkablePass = new List<Tile>();
            firstTimeIteration = true;
            cells              = new List<int>();
            ReadFromFile(fileName);
            GenerateMap();

            MapNodes = new Node[cells.Count];
            GenerateNodes();
            GenerateNeighborNode();
        }

        public void GenerateNeighborNode()
        {
            // Here we move between the indexes
            for (int y = 0; y < rows + 2; y++)
            {
                for (int x = 0; x < (columns - 1); x++)
                {
                    int index = y * (columns - 1) + x;

                    if (MapNodes[index] == null)
                        continue;

                    // top
                    Node top = GetNodeByIndex(x, y - 1);
                    if (top != null)
                        MapNodes[index].AddNeighbour(top);

                    // right
                    Node right = GetNodeByIndex(x + 1, y);
                    if (right != null)
                        MapNodes[index].AddNeighbour(right);

                    // bottom
                    Node bottom = GetNodeByIndex(x, y + 1);
                    if (bottom != null)
                        MapNodes[index].AddNeighbour(bottom);

                    // left
                    Node left = GetNodeByIndex(x - 1, y);
                    if (left != null)
                        MapNodes[index].AddNeighbour(left);
                }
            }
        }
        public Node GetNodeByIndex(int x, int y)
        {
            if (x < 0 || x  > (columns - 1))
                return null;
            if (y  < 0 || y > rows + 2)
                return null;

            int index = y  * (columns - 1) + x ;

            return MapNodes[index];
        }
        public static int GetLevelEnumeratedIndex(int x, int y)
        {
            int index = y * (columns - 1) + x;

            return cells[index];
        }
        private void GenerateMap()
        {
            for (int iterator = 0; iterator < cells.Count; iterator++)
            {
                if (cells[iterator] == 3 || cells[iterator] == 4)
                {
                    Spawn(new Tile(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize))), "Wall", true));
                }
                else if (cells[iterator] == 2)
                {
                    Tile walkable = new Tile(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize))), "Obstacle", true);
                    WalkablePass.Add(walkable);
                    Spawn(walkable);
                }
                else if (cells[iterator] == 5)
                {
                    playerSpawnPoint = new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize)));
                }
                else if (cells[iterator] == 5 || cells[iterator] == 0)
                {
                    powerUpSpawnPoints.Add(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize))));
                }
                else if (cells[iterator] == 12)
                {
                    enemySpawnPoints.Add(new Vector2((iterator % (columns - 1) * blockSize), ((iterator / (columns - 1) * blockSize))));
                }
            }
        }
        private void GenerateNodes()
        {
            for (int y = 2; y < rows + 2; y++)
            {
                for (int x = 0; x < (columns - 1); x++)
                {
                    int index = y * (columns - 1) + x;

                    if (cells[index] == 3 ||cells[index] == 2)
                        MapNodes[index] = null;
                    else if(cells[index] == 5 || cells[index] == 12 || cells[index] == 0)
                        MapNodes[index] = new Node(1, new Vector2(x, y));
                }
            }
        }

        private void ReadFromFile(string csvFileName)
        {
            //string[] lines = File.ReadAllLines(csvFileName);
            string[] lines = Generator.GenerateLevel(csvFileName, 30);
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
                    string currentVal = values[i].Trim();
                    bool success = int.TryParse(currentVal, out int value);
                    if (success)
                    cells.Add(value);
                }
            }
        }
        public static bool GetSwap(int x, int y)
        {
            int index = y * (columns - 1) + x;
            if (cells[index] == 3)
                return true;

            return false;
        }

        public static bool GetIndexExplosion(bool explosion, int x, int y) // for explosion spawn
        {
            int index = y * (columns - 1) + x ;

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

        public static int GetIndex(int x, int y)
        {
            int index = y  * (columns - 1) + x ;
            return index;
        }

        public static Vector2 GetPlayerSpawnPoint()
        {
            return playerSpawnPoint;
        }

        public static List<Vector2> GetPowerupSpawnPoint()
        {
            return powerUpSpawnPoints;
        }

        public static List<Vector2> GetEnemySpawnPoints()
        {
            return enemySpawnPoints;
        }
    }
}
