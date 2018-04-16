using BehaviourEngine;
using OpenTK;

namespace BomberMan2D
{
    public class Mystery : GameObject, IPowerup
    {
        private Rigidbody2D rigidBody;

        public Mystery() : base()
        {
            #region LayerMask
            this.Layer = (uint) CollisionLayer.Powerup;
            #endregion
            //ChangeTexture
            SpriteRenderer renderer = new SpriteRenderer(FlyWeight.Get("Mystery_PW"));
            renderer.RenderOffset = (int) RenderLayer.Powerup;
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
            timer = new Timer(10f);
            player = toBoostUp;
        }

        public void Update()
        {
            if (player.Invulnerability)
                timer.Start(false);

            if (timer.IsActive)
                timer.Update(false);

            if (timer.IsOver())
            {
                player.Invulnerability = false;
                timer.Stop(true);
            }
        }
    }
}
