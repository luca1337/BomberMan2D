using BehaviourEngine;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Pontan : AI
    {
        public override Transform RefTransform => Transform;
        public override ulong Score => 8000;
        public override float Speed => 1.7f;
        public override float Radius => 5.0f;
        public override bool CanPassWall => true;

        public Pontan() : base("Pontan", FlyWeight.Get("Pontan"), 50, 50, 6, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {
        }
    }
}
