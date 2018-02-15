using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D.Prefabs
{
    public class SpeedPow : PowerUp, IPowerup
    {
        public PowerUpType PowerUpType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SpeedPow() : base()
        {
            this.texture = FlyWeight.Get("Speed_PW");
        }

        public void ApplyPowerUp(GameObject gameObject, PowerUpType type)
        {
            throw new NotImplementedException();
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
