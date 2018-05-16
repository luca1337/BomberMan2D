using BehaviourEngine;

namespace BomberMan2D
{
    public class Kondoria : AI
    {
        public override Transform RefTransform => Transform;

        public override ulong Score => 1000;
        public override float Speed => 0.7f;
        public override float Radius => 5.0f;
        public override bool CanPassWall => true;

        public Kondoria() : base("Kondoria", FlyWeight.Get("Kondoria"), (int)18.5f, 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnCollisionEnter(Collider2D other, HitState hitstate)
        {
        }
    }
}
