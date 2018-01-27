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
            window.SetClearColor(1.0f, 0.61f, 0.0f);
            Engine.Init(window);

            FlyWeight.Add("wall", "Assets/w.dat");
            FlyWeight.Add("obstacle", "Assets/obstacle.dat");

            GameObject.Spawn(new Map("Levels/Level00.csv"));
        }

        public static void Run()
        {
            Engine.Run();
        }
    }
}
