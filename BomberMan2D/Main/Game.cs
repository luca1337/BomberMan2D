using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Fast2D.Utils.TextureHelper;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Utils;
using BomberMan2D.Main;
using BomberMan2D.Prefabs;
using OpenTK;

namespace BomberMan2D.Main
{
    public sealed class Game
    {
        private static GameManager manager;
        public static void Init()
        {
            #region Context
            Window window = new Window(1200, 700, "BomberMan");
            window.SetDefaultOrthographicSize(14);
            window.SetClearColor(0.0f, 0.61f, 0.0f);
            Engine.Init(window);
            #endregion
            manager = new GameManager();

            #region GameManager
            GameObject.Spawn(manager);
            #endregion
        }

        public static void Run()
        {
            Engine.Run();
        }
    }
}
