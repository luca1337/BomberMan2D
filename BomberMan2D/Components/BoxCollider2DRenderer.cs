using BehaviourEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public class BoxCollider2DRenderer : SpriteRenderer, IStartable
    {
        BoxCollider2D collider;
        private Vector4 color;

        public BoxCollider2DRenderer(Vector4 color) : base(FlyWeight.Get("Box2D")) { this.color = color; }
        void IStartable.Start()
        {
            base.Start();
            collider = this.Owner.GetComponent<BoxCollider2D>();
        }

        public override void Update()
        {
            this.Sprite.position = collider.internalTransform.Position;
            this.Sprite.Rotation = collider.internalTransform.Rotation;
            this.Sprite.scale = collider.Size;
            this.Sprite.SetAdditiveTint(color);
        }
    }
}
