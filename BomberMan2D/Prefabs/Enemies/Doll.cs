using System;
using BehaviourEngine;

namespace BomberMan2D
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

        public override void OnCollisionEnter(Collider2D other, HitState hitstate)
        {
        }
    }
}
