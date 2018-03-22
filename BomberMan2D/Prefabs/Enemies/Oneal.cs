using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Interfaces;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Oneal : GameObject, IEnemy, IPathfind
    {
        public List<Node> CurrentPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Computed => throw new NotImplementedException();

        public Oneal()
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

        public bool IsInRadius(out GameObject target)
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
