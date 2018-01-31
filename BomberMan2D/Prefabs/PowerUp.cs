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
    public class PowerUp : GameObject, IPowerup
    {
        public List<string> textures = new List<string>();
        public PowerUpType powerUpType;
        private List<float> speedValues = new List<float>();

        private Rigidbody2D rigidBody;

        public PowerUp()
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.Powerup;
            #endregion

            textures.Add("Speed");
            textures.Add("Health");

            SpriteRenderer renderer = new SpriteRenderer(FlyWeight.Get(textures[(int)powerUpType]));
            renderer.RenderOffset = (int)RenderLayer.Powerup;
            AddComponent(renderer);

            BoxCollider2D collider2D = new BoxCollider2D(new Vector2(1, 1));
            collider2D.CollisionMode = CollisionMode.Trigger;
            collider2D.TriggerEnter += OnTriggerEnter;
            AddComponent(collider2D);

            rigidBody = new Rigidbody2D();
            rigidBody.IsGravityAffected = false;
            AddComponent(rigidBody);

            speedValues = GetRandomFloats(5, 1.5f, 3.4f);
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if(other.Owner is Bomberman)
            {
                Pool<PowerUp>.RecycleInstance(this, p => p.OnRecycle());
            }
        }

        public void OnRecycle()
        {
            this.Active = false;
            rigidBody.Velocity = Vector2.Zero;
        }

        public void ApplyPowerUp(IPowerupable powerUp)
        {
            if (powerUpType == PowerUpType.PRP_HEALTH)
            {
                powerUp.ApplyHealth(1);
            }
            else
            {
                powerUp.ApplySpeed(speedValues[RandomManager.Instance.Random.Next(0, speedValues.Count)]);
            }
        }

        private List<float> GetRandomFloats(int size, float min, float max)
        {
            List<float> floatList = new List<float>();

            for (int i = 0; i < size; i++)
            {
                floatList.Add(Utils.GenerateRandomFloatInRange(min, max));
            }

            return floatList;
        }
    }
}
