using BehaviourEngine;
using BehaviourEngine.Pathfinding;
using OpenTK;
using System;

namespace BomberMan2D
{
    public class Tile : GameObject
    {
        public Tile(Vector2 position, string textureName, bool solidBlock)
        {
            if(solidBlock)
                this.Layer = (uint)CollisionLayer.SolidWall;
            else
                this.Layer = (uint)CollisionLayer.Wall;
            
            this.Transform.Position = position;
            SpriteRenderer Renderer = new SpriteRenderer(FlyWeight.Get(textureName));
            Renderer.RenderOffset   = (int)RenderLayer.Tile;
            AddComponent(Renderer);

            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.TriggerEnter += OnTriggerEnter;
            AddComponent(collider);

            //AddComponent(new BoxCollider2DRenderer(new Vector4(-1f, 1f, -1f, 0f)));
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

                //this is the random to generate our powerups..
                int randomPw = RandomManager.Instance.Random.Next(0, Enum.GetNames(typeof(PowerUpType)).Length);

                //once the random is thrown we can generate it
                //at the moment we only have 2 active powerups so 
                //we make some test using the first one on the enum
                IPowerup p = PowerUpFactory.Get(PowerUpType.PW_MYSTERY);
                p.SetPosition(this.Transform.Position);
                    
                GameObject.Spawn(p as GameObject);
            }
        }
    }
}
