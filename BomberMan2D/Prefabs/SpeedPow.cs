using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using BomberMan2D.Components;
using OpenTK;

namespace BomberMan2D.Prefabs
{
    public class SpeedPow : PowerUp, IPowerup
    {
        public PowerUpType PowerUpType { get; set; }

        public SpeedPow() : base()
        {
            this.texture = FlyWeight.Get("Speed_PW");
            PowerUpType = PowerUpType.PW_SPEED;
        }

        public void ApplyPowerUp(GameObject gameObject, PowerUpType type)
        {
            this.PowerUpType = type;
            (gameObject as Bomberman).GetComponent<CharacterController>().Speed = 3.0f;
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
}
