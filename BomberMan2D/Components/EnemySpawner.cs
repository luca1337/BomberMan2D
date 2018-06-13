using BehaviourEngine;
using System.Linq;

namespace BomberMan2D
{
    public class EnemySpawner : BehaviourEngine.GameObject
    {
        public EnemySpawner() : base("AI Spawner")
        {
            int cnt = Map.GetEnemySpawnPoints().Count();
            for (int i = 0; i < cnt; i++)
            {
                IEnemy enemy = EnemyFactory.Get(EnemyType.Balloom);
                enemy.RefTransform.Position = Map.GetEnemySpawnPoints()[i];

                GameObject.Spawn((GameObject)enemy);
            }
        }
    }
}
