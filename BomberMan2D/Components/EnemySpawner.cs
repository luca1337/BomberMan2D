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
                //IEnemy it's a generic interface which permitt us to get the istance between an enum,
                //after we specify the enum (we also can put a random number bewtween 0 and the length of the enum)
                //we can get the right instance and assign the target too...

                IEnemy enemy = EnemyFactory.Get(Enums.EnemyType.Balloom);
                enemy.Player = (Bomberman)Player;
                enemy.RefTransform.Position = Map.GetEnemySpawnPoints()[RandomManager.Instance.Random.Next(0, Map.GetEnemySpawnPoints().Count())];

                GameObject.Spawn(enemy as Prefabs.Enemies.AI);
            }
        }
    }
}
