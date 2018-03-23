using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BomberMan;
using BehaviourEngine;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Balloom : AI
    {
        public override Transform RefTransform => this.Transform;

        public override ulong Score { get { return 10; } }
        public override float Speed { get; set; } = 1.5f;
        public override float Radius { get; set; } = 4.0f;

        public Balloom() : base("Balloom", FlyWeight.Get("AI"), (int)18.5f, 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {

        }
    }
}
