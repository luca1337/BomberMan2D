using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using BehaviourEngine.Renderer;
using OpenTK;
using BehaviourEngine.Interfaces;
using BomberMan;
using static BehaviourEngine.Collider2D;
using BomberMan2D.Enums;
using BomberMan2D.Components;

namespace BomberMan2D
{
    public class Explosion : GameObject
    {
        public BoxCollider2D BoxCollider { get; set; }

        private AnimationRenderer anim;

        public Explosion() : base("Explosion")
        {
            this.Layer = (uint)CollisionLayer.Explosion;

            anim = new AnimationRenderer(FlyWeight.Get("Explosion"), 100, 100, 9, new int[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                41, 42, 43, 44, 45, 56, 57, 58, 59, 50,
                51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                61, 62, 63, 64, 65, 66, 67, 68, 69, 70,
                71, 72, 73, 74, 75
            }, 0.018f, true, false);
           
            BoxCollider  = new BoxCollider2D(new Vector2(1f, 1f));
            BoxCollider.CollisionMode = CollisionMode.Trigger;
            AddComponent(BoxCollider);

            AddComponent(new BoxCollider2DRenderer(new Vector4(1f, -1f, -1f, 0f)));

            AddComponent(anim);
        }

        public void Reset()
        {
            anim.Reset();
        }
    }
}
