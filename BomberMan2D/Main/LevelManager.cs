using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Main
{
    public static class LevelManager
    {
        private static Dictionary<string, Map> maps;
        private static Map map;

        public static Map CurrentMap { get => map; private set => map = value; }

        static LevelManager()
        {
            maps = new Dictionary<string, Map>();
        }

        public static IMap Add(string mapName)
        {
            try
            {
                map = new Map(mapName);
                maps.Add(mapName, map);
                CurrentMap = maps[mapName];
                GameObject.Spawn(CurrentMap);
                return map;
            }
            catch
            {
                throw new Exception("Key already or not registered yet!");
            }
        }
        public static void RefreshMap(Node[] nodes)
        {
        }
        public static IMap Get(string mapName)
        {
            try
            {
                return maps[mapName];
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
