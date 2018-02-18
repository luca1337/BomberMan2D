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
    public abstract class PowerUp : GameObject
    {
        private Rigidbody2D rigidBody;
        protected Texture texture;

        protected PowerUp()
        {
           
        }

        protected virtual void OnTriggerEnter(Collider2D other)
        {
        }
    }
}
