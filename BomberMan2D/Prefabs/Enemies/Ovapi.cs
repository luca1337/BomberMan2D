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
    public class Ovapi : AI
    {
        public override AnimationRenderer renderer { get; protected set; }

        public override ulong Score { get; set; }
        public override float Speed { get; set; }
        public override float Radius { get; set; }
        public override Transform RefTransform => this.Transform;

        public Ovapi() : base("Ovapi", FlyWeight.Get("AI"), (int)18.5f, 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {

        }
    }
}
