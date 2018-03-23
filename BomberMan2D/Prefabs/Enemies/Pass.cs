using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.AI;
using BomberMan2D.Interfaces;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Pass : GameObject, IEnemy, IPathfind
    {
        public Bomberman Player { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IWaypoint Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Node> CurrentPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Computed => throw new NotImplementedException();

        public Pass()
        {

        }

        public void ComputePath<T>(T item, int x, int y) where T : IMap
        {
            throw new NotImplementedException();
        }

        public void DoPath()
        {
            throw new NotImplementedException();
        }

        public bool IsInRadius(GameObject target)
        {
            throw new NotImplementedException();
        }

        public void OnGet()
        {
            throw new NotImplementedException();
        }

        public void OnRecycle()
        {
            throw new NotImplementedException();
        }
    }
}
