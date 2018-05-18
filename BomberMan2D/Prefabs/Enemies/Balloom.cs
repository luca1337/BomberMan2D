using BehaviourEngine;

namespace BomberMan2D
{
    public class Balloom : AI
    {
        public override Transform RefTransform => Transform;
        public override ulong Score => 100;
        public override float Radius => 4.0f;
        public override float Speed => 0.9f;
        public override bool CanPassWall => false;

        public Balloom() : base("Balloom", FlyWeight.Get("Balloom"), 50, 50, 11, new int[] { 0, 1, 2, 3, 4, 5 }, 0.2f, true, false)
        {

        }

        public override void OnCollisionEnter(Collider2D other, HitState hitstate)
        {
            if (other.Owner is AI )
                Chase.doChase = true;

            //FIX
            //if (other.Owner is Bomb)
            //    Chase.doChase = true;

        }
    }
}
