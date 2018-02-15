﻿using BehaviourEngine;
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
    public abstract class PowerUp : GameObject
    {
       // public PowerUpType powerUpType;
        //private List<float> speedValues = new List<float>();

        private Rigidbody2D rigidBody;
        protected Texture texture;

     //   public PowerUpType PowerUpType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected PowerUp()
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.Powerup;
            #endregion

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

           // speedValues = GetRandomFloats(5, 1.5f, 3.4f);
        }

        protected virtual void OnTriggerEnter(Collider2D other)
        {
            if(other.Owner is Bomberman)
            {
                Pool<PowerUp>.RecycleInstance(this, p => p.OnRecycle());
            }
        }

        public abstract void OnRecycle();

        protected List<float> GetRandomFloats(int size, float min, float max)
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
