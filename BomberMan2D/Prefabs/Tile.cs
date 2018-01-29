using BehaviourEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Prefabs
{
    public class Tile : GameObject
    {
        public Tile(Vector2 position, string textureName, bool solidBlock)
        {
            this.Layer = (uint)CollisionLayer.Wall;

            this.Transform.Position = position;
            SpriteRenderer Renderer = new SpriteRenderer(FlyWeight.Get(textureName));
            Renderer.RenderOffset = (int)RenderLayer.Tile;
            AddComponent(Renderer);

            if (solidBlock)
            {
                BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
                AddComponent(collider);
            }
        }
    }
}
