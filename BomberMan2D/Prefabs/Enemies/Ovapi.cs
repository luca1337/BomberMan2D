using BehaviourEngine;

namespace BomberMan2D
{
    public class Ovapi : AI
    {
        public override Transform RefTransform => Transform;
        public override ulong Score => 2000;
        public override float Speed => 0.9f;
        public override float Radius => 3.0f;
        public override bool CanPassWall => true;

        public Ovapi() : base("Ovapi", FlyWeight.Get("Ovapi"), 50, 50, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnCollisionEnter(Collider2D other, HitState hitstate)
        {
        }
    }
}
