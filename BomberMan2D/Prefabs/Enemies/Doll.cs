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
    public class Doll : AI
    {
        public override Transform RefTransform => throw new NotImplementedException();

        public override ulong Score => 400;
        public override float Speed => 1.2f;
        public override float Radius => 3.0f;
        public override bool CanPassWall => false;

        public Doll() : base("Doll", FlyWeight.Get("Doll"), 50, 50, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {
        }
    }
}
