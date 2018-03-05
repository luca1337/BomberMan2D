using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine.Utils;
using BehaviourEngine;
using BomberMan2D.Prefabs;

namespace BomberMan2D
{
    public class WallPass : GameObject, IPowerup
    {
        private Rigidbody2D rigidBody;

        public WallPass()
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.Powerup;
            #endregion

            SpriteRenderer renderer = new SpriteRenderer(FlyWeight.Get("Wallpass_PW"));
            renderer.RenderOffset = (int)RenderLayer.Powerup;
            AddComponent(renderer);

            BoxCollider2D collider2D = new BoxCollider2D(new Vector2(1, 1));
            collider2D.CollisionMode = CollisionMode.Trigger;
            collider2D.TriggerEnter += OnTriggerEnter; 
            AddComponent(collider2D);

            rigidBody = new Rigidbody2D();
            rigidBody.IsGravityAffected = false;
            AddComponent(rigidBody);
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.Owner is Bomberman)
                OnRecycle();

            for (int i = 0; i < LevelManager.CurrentMap.WalkablePass.Count ; i++)
            {
                LevelManager.CurrentMap.WalkablePass[i].Layer = (uint)CollisionLayer.Wall;
            }
        }

        public void ApplyPowerUp(GameObject gameObject)
        {

        }

        public void OnGet()
        {
            this.Active = true;
        }

        public void OnRecycle()
        {
            this.Active = false;
            this.Transform.Position = Vector2.Zero;
        }

        public void SetPosition(Vector2 position)
        {
            this.Transform.Position = position;
        }
    }
}
