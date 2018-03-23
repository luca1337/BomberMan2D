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
    public class Pass : AI
    {
        public override AnimationRenderer renderer { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public Pass() : base("Pass")
        {
            renderer = new AnimationRenderer(FlyWeight.Get("AI"), (int)18.5f, 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false);
        }

        public override void OnTriggerEnter(Collider2D other)
        {
            base.OnTriggerEnter(other);
        }

        public override void OnGet()
        {
            base.OnGet();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}
