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
        public Tile(Vector2 position, string textureName)
        {
            this.Transform.Position = position;
            SpriteRenderer Renderer = new SpriteRenderer(FlyWeight.Get(textureName));
            AddComponent(Renderer);
        }
    }
}
