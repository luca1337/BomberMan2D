using BehaviourEngine;
using BomberMan2D.AI;
using BomberMan2D.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Components
{
    public class EnemySpawner : BehaviourEngine.GameObject
    {
        IWaypoint Player { get; set; }

        public EnemySpawner(IWaypoint player) : base("AI Spawner")
        {
            this.Player = player;

            for (int i = 0; i < Map.GetEnemySpawnPoints().Count(); i++)
            {
                /*AI enemy = Pool<AI>.GetInstance(x =>
                {
                    x.Player = Player;
                    x.Transform.Position = Map.GetEnemySpawnPoints()[RandomManager.Instance.Random.Next(0, Map.GetEnemySpawnPoints().Count())];
                });*/

                //GameObject.Spawn(enemy);
            }
        }
    }
}
