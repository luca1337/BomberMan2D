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
        private short minPercentage = 0;
        private short maxPercentage = 100;
        private short halfPercentage = 50;


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
                collider.TriggerEnter += OnTriggerEnter;
                AddComponent(collider);

                AddComponent(new BoxCollider2DRenderer(new Vector4(-1f, 1f, -1f, 0f)));
            }
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if(other.Owner is Explosion)
            {
                this.Active = false;

                int chance = RandomManager.Instance.Random.Next(minPercentage, maxPercentage);

                if (chance > halfPercentage + 40)
                {
                    PowerUp p = Pool<PowerUp>.GetInstance(x =>
                    {
                        x.GetComponent<SpriteRenderer>().SetTexture("Bomb_PW");
                    });
                    
                    GameObject.Spawn(p, this.Transform.Position);
                }
            }
        }
    }
}
