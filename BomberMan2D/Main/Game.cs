using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Prefabs;
using OpenTK;

namespace BomberMan2D
{
    public class Game
    {
        private static List<IWaypoint> targetPoints = new List<IWaypoint>();

        public static void Init()
        {
            Window window = new Window(1200, 700, "BomberMan");
            window.SetDefaultOrthographicSize(14);
            window.SetClearColor(0.0f, 0.61f, 0.0f);
            Engine.Init(window);

            //do we want physics?
            Physics.Instance.Gravity *= 2f;

            //Collision masks
            LayerManager.AddLayer((uint)CollisionLayer.BomberMan, (uint)CollisionLayer.Wall + (uint)CollisionLayer.Powerup);

            LoadTextures();
            ObjectPools();

            GameObject.Spawn(new Map("Levels/Level00.csv"));
            Prefabs.BomberMan bomberMan = new Prefabs.BomberMan();
            GameObject.Spawn(bomberMan, Map.GetPlayerSpawnPoint());

            AI enemy = new AI();
            enemy.Player = bomberMan;
            enemy.CurrentTarget = bomberMan;
            GameObject.Spawn(enemy, new Vector2(5, 5));

            GameObject.Spawn(new PowerupSpawner(5));
        }

        private static void LoadTextures()
        {
            FlyWeight.Add("Wall", "Assets/Wall_01.dat");
            FlyWeight.Add("Obstacle", "Assets/Obstacle_01.dat");
            FlyWeight.Add("BomberMan", "Assets/Bombertab1.dat");
            FlyWeight.Add("Speed", "Assets/Speed.dat");
            FlyWeight.Add("Health", "Assets/Health.dat");
            FlyWeight.Add("Bomb", "Assets/Bomb.dat");
            FlyWeight.Add("Explosion", "Assets/Explosion.dat");
            FlyWeight.Add("AI", "Assets/ballon.dat");
        }

        internal static List<IWaypoint> GetAllPoints()
        {
            return targetPoints;
        }

        internal static void AddTargetPoint(IWaypoint current)
        {
            targetPoints.Add(current);
        }

        internal static int GetPointsCount()
        {
            return targetPoints.Count();
        }

        private static void ObjectPools()
        {
            Pool<PowerUp>.Register(() => new PowerUp(), 100);
            Pool<Bomb>.Register(() => new Bomb(),100);
            Pool<Explosion>.Register(() => new Explosion(), 12);
        }

        public static void Run()
        {
            Engine.Run();
        }

    }
}
