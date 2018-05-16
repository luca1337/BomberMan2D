using BehaviourEngine;

namespace BomberMan2D
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

        public override void OnCollisionEnter(Collider2D other, HitState hitstate)
        {
        }
    }
}
