using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using BehaviourEngine;
using BomberMan2D.Prefabs;

namespace BomberMan2D
{
    public class Game
    {
        public static void Init()
        {
            Window window = new Window(1200, 700, "BomberMan");
            window.SetClearColor(0.0f, 0.61f, 0.0f);
            Engine.Init(window);

            Physics.Instance.Gravity *= 20f;

            //Collision masks
            LayerManager.AddLayer((uint)CollisionLayer.BomberMan, (uint)CollisionLayer.Wall);

            LoadTextures();

            GameObject.Spawn(new Map("Levels/Level00.csv"));
            GameObject.Spawn(new Prefabs.BomberMan(), Map.GetPlayerSpawnPoint());
        }

        private static void LoadTextures()
        {
            FlyWeight.Add("Wall", "Assets/Wall_01.dat");
            FlyWeight.Add("Obstacle", "Assets/Obstacle_01.dat");
            FlyWeight.Add("BomberMan", "Assets/Bombertab1.dat");
        }

        public static void Run()
        {
            Engine.Run();
        }
    }
}
