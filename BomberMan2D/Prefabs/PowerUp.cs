using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine.Utils;

namespace BomberMan2D.Prefabs
{
    public class PowerUp : GameObject
    {
        private Rigidbody2D rigidBody;
        protected Texture texture;

        protected PowerUp()
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.Powerup;
            #endregion

            texture = FlyWeight.Get("Speed_PW");
            SpriteRenderer renderer = new SpriteRenderer(texture);
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

        protected virtual void OnTriggerEnter(Collider2D other)
        {

        }
    }
}
