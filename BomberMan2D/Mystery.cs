using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using BomberMan2D.Prefabs;
using BomberMan2D.Components;
using BehaviourEngine.Interfaces;

namespace BomberMan2D
{
    public class Mystery: GameObject, IPowerup
    {
        private Rigidbody2D rigidBody;
        private InvulnerabilityManager manager;
        private bool oneTime = true;

        public Mystery() : base()
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.Powerup;
            #endregion

            SpriteRenderer renderer = new SpriteRenderer(FlyWeight.Get("Speed_PW"));
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
            {
                OnRecycle();
            }
        }

        public void ApplyPowerUp(GameObject gameObject)
        {
            (gameObject as Bomberman).Invulnerability = true;

            if (oneTime)
            {
                manager = new InvulnerabilityManager(gameObject as Bomberman);
                AddComponent(manager);
            }
        }

        public void OnGet()
        {
            this.Active = true;
        }

        public void OnRecycle()
        {
            this.Active ^= this.Active;
            this.Transform.Position = Vector2.Zero;
        }
        public void SetPosition(Vector2 position)
        {
            this.Transform.Position = position;
        }
    }

    public class InvulnerabilityManager : Component, IUpdatable
    {
        private Timer timer;
        private Bomberman player;

        public InvulnerabilityManager(Bomberman toBoostUp)
        {
            timer = new Timer(4f);
            player = toBoostUp;
        }

        public void Update()
        {
            if (player.Invulnerability)
                timer.Start();

            if (timer.IsActive)
                timer.Update(false);

            if (timer.IsOver())
            {
                player.Invulnerability = true;
                timer.Stop(true);
            }
        }
    }
}
