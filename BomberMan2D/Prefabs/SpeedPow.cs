using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;

namespace BomberMan2D.Prefabs
{
    class SpeedPow : PowerUp, IPowerup
    {
        public PowerUpType PowerUpType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ApplyPowerUp(GameObject gameObject, PowerUpType type)
        {
            throw new NotImplementedException();
        }

        public override void OnRecycle()
        {
            throw new NotImplementedException();
        }
    }
}
