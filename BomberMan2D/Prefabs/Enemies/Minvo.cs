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
    public class Minvo : AI
    {
        public override Transform RefTransform => Transform;
        public override ulong Score => 800;
        public override float Speed => 1.5f;
        public override float Radius => 3.0f;
        public override bool CanPassWall => false;

        public Minvo() : base("Minvo", FlyWeight.Get("Minvo"), 50, 50, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {
        }
    }
}
