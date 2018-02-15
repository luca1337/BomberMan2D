using BehaviourEngine;
using BehaviourEngine.Utils;
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
            Renderer.RenderOffset   = (int)RenderLayer.Tile;
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

                int xPos = (int)Transform.Position.X;
                int yPos = (int)Transform.Position.Y;
                int index = Map.GetIndex(xPos,yPos);

                if (LevelManager.CurrentMap.MapNodes[index] == null)
                    LevelManager.CurrentMap.MapNodes[index] = new Node(1, new Vector2(xPos, yPos));

                LevelManager.CurrentMap.GenerateNeighborNode();

                int randomPw = RandomManager.Instance.Random.Next(0, Enum.GetNames(typeof(PowerUpType)).Length);

                IPowerup p = Pool<IPowerup>.GetInstance(x =>
                {
                    x.PowerUpType =(PowerUpType)randomPw;
                    x.SetPosition(this.Transform.Position);
                });
                    
                GameObject.Spawn(p as GameObject);
            }
        }
    }
}
