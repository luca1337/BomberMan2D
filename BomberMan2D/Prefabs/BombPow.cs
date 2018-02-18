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
            this.texture = FlyWeight.Get("Bomb_PW");
            PowerUpType = PowerUpType.PW_BOMB;
        }
        public void ApplyPowerUp(GameObject gameObject, PowerUpType type)
        {
            this.PowerUpType = type;
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
