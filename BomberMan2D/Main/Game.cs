using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Utils;
using BomberMan2D.Main;
using BomberMan2D.Prefabs;
using OpenTK;

namespace BomberMan2D
{
    public sealed class Game
    {
        public static void Init()
        {
            #region Context
            Window window = new Window(1200, 700, "BomberMan");
            window.SetDefaultOrthographicSize(14);
            window.SetClearColor(0.0f, 0.61f, 0.0f);
            Engine.Init(window);
            #endregion

            #region GameManager
            GameObject.Spawn(new GameManager());
            #endregion
        }

        public static void Run()
        {
            Engine.Run();
        }
    }
}
