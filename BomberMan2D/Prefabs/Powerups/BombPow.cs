using BehaviourEngine;
using OpenTK;

namespace BomberMan2D
{
    public class BombPow : GameObject, IPowerup
    {
        private Rigidbody2D rigidBody;

        public BombPow() : base()
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.Powerup;
            #endregion

            SpriteRenderer renderer = new SpriteRenderer(FlyWeight.Get("Bomb_PW"));
            renderer.RenderOffset = (int)RenderLayer.Powerup;
            AddComponent(renderer);

            BoxCollider2D collider2D = new BoxCollider2D(new Vector2(1, 1));
            collider2D.CollisionMode = CollisionMode.Trigger;
            collider2D.TriggerEnter += OnTriggerEnter; ;
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
            if (gameObject is Bomberman)
                (gameObject as Bomberman).CurrentExplosion = 1;
        }

        public void SetPosition(Vector2 position)
        {
            this.Transform.Position = position;
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
    }
}
