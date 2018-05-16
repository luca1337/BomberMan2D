using BehaviourEngine;

namespace BomberMan2D
{
    public class Pass : AI
    {
        public override Transform RefTransform => Transform;
        public override ulong Score => 4000;
        public override float Speed => 1.6f;
        public override float Radius => 5f;
        public override bool CanPassWall => false;

        public Pass() : base("Pass", FlyWeight.Get("Pass"), 50, 50, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnCollisionEnter(Collider2D other, HitState hitstate)
        {
        }
    }
}
