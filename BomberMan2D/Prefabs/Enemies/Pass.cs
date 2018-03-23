using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using BomberMan2D.AI;
using BomberMan2D.Interfaces;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Pass : AI
    {
        public override Transform RefTransform => throw new NotImplementedException();

        public override ulong Score => throw new NotImplementedException();

        public override float Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override float Radius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Pass() : base("Pass", FlyWeight.Get("AI"), (int)18.5f, 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {
            throw new NotImplementedException();
        }
    }
}
