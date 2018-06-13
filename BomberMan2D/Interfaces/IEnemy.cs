using BehaviourEngine;

namespace BomberMan2D
{
    /// <summary>
    /// this interface exposes these fields and determines the exact behavior of the enemy
    /// </summary>
    public interface IEnemy : IPoolable
    {
        /// <summary>
        /// void method that just incapsulate the execution path for the Enemy to the target.
        /// </summary>
        void ExecutePath();

        Transform RefTransform { get; }

        /// <summary>
        /// Score that each enemy will give when killed.
        /// </summary>
        ulong Score { get; }

        /// <summary>
        /// Speed that tells how fast the enemy will move.
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Radius of the enemy that will turn him to move towards the target.
        /// </summary>
        float Radius { get; }

        /// <summary>
        /// Tells if the current enemy can go beyond walls.
        /// </summary>
        bool CanPassWall { get; }
    }
}
