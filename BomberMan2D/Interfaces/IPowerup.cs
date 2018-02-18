using BehaviourEngine;
using BomberMan2D.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public interface IPowerup : IPoolable
    {
        void ApplyPowerUp(GameObject gameObject, PowerUpType type);
        void SetPosition(Vector2 position);
    }
}
