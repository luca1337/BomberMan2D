using BehaviourEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public interface IPowerup
    {
        void ApplyPowerUp(GameObject gameObject, PowerUpType type);
        void SetPosition(Vector2 position);
        PowerUpType PowerUpType { get; set; }
    }
}
