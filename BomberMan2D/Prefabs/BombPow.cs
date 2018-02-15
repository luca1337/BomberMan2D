using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D.Prefabs
{
    public class BombPow : PowerUp, IPowerup
    {
        public PowerUpType PowerUpType { get ; set ; }

        public BombPow() : base()
        {

        }
        public void ApplyPowerUp(GameObject gameObject, PowerUpType type)
        {
        }

        public override void OnRecycle()
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
