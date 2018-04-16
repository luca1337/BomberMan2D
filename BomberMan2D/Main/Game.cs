using Aiv.Fast2D;
using BehaviourEngine;

namespace BomberMan2D
{
    public sealed class Game
    {
        private static GameManager manager;
        public static void Init()
        {
            #region Context
            Window window = new Window(1200, 700, "BomberMan");
            window.SetDefaultOrthographicSize(15);
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
