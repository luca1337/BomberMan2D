using BehaviourEngine;
using BomberMan2D.AI;
using BomberMan2D.Factories;
using BomberMan2D.Interfaces;
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
                IEnemy enemy = EnemyFactory.Get(Enums.EnemyType.Balloom);
                enemy.Player = (Bomberman)player;
                enemy.RefTransform.Position = Map.GetEnemySpawnPoints()[RandomManager.Instance.Random.Next(0, Map.GetEnemySpawnPoints().Count())];

                GameObject.Spawn((GameObject)enemy);
            }
        }
    }
}
