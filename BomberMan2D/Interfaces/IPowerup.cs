using BehaviourEngine;
using OpenTK;

namespace BomberMan2D
{
    public interface IPowerup : IPoolable
    {
        void ApplyPowerUp(GameObject gameObject);
        void SetPosition(Vector2 position);
    }
}
