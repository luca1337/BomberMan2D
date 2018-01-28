using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Prefabs
{
    public class AI : GameObject, IPathfind
    {
        #region Pathfinding
        public List<Node> CurrentPath { get; set; }
        public bool Computed
        {
            get
            {
                if ((CurrentPath.Count) == 0)
                    return true;
                return false;
            }
        }
        #endregion

        private AnimationRenderer aiAnimation;

        public AI() : base("AI")
        {
            aiAnimation = new AnimationRenderer(FlyWeight.Get("Balloon"), ((int)(float)Math.Floor(18.5f)), 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false);
            AddComponent(aiAnimation);
        }

        public void ComputePath<T>(T item, int x, int y) where T : IMap
        {
            int pX, pY;

            if (Math.Abs(Transform.Position.X - (int)Transform.Position.X) > 0.5f)
                pX = (int)Transform.Position.X + 1;
            else
                pX = (int)Transform.Position.X;

            if (Math.Abs(Transform.Position.Y - (int)Transform.Position.Y) > 0.5f)
                pY = (int)Transform.Position.Y + 1;
            else
                pY = (int)Transform.Position.Y;

            CurrentPath = AStar.GetPath(item, pX, pY, x, y);
        }
    }
}
