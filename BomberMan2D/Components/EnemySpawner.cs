using BehaviourEngine;
using System.Linq;

namespace BomberMan2D
{
    public class EnemySpawner : BehaviourEngine.GameObject
    {
        IWaypoint Player { get; set; }

        public EnemySpawner(IWaypoint player) : base("AI Spawner")
        {
            this.Player = player;

            for (int i = 0; i < Map.GetEnemySpawnPoints().Count(); i++)
            {
                IEnemy enemy = EnemyFactory.Get(EnemyType.Balloom);
                enemy.Player = (Bomberman)player;
                enemy.RefTransform.Position = Map.GetEnemySpawnPoints()[RandomManager.Instance.Random.Next(0, Map.GetEnemySpawnPoints().Count())];

                GameObject.Spawn((GameObject)enemy);
            }
        }
    }
}
