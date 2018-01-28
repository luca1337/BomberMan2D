using BehaviourEngine;
using BomberMan2D.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Prefabs
{
    public class PowerupSpawner : GameObject
    {
        public PowerupSpawner(int spawnUnits)
        {
            for (int i = 0; i < spawnUnits; i++)
            {
                PowerUp powerUp = Pool<PowerUp>.GetInstance(p =>
                {
                    p.Transform.Position = Map.GetPowerupSpawnPoint()[RandomManager.Instance.Random.Next(0, Map.GetPowerupSpawnPoint().Count)];

                    p.powerUpType = (PowerUpType)RandomManager.Instance.Random.Next(0, Enum.GetNames(typeof(PowerUpType)).Length);

                    if (p.powerUpType == PowerUpType.PRP_HEALTH)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture(p.textures[(int)PowerUpType.PRP_HEALTH]);
                    }
                    else
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture(p.textures[(int)PowerUpType.PRP_SPEED]);
                    }
                });

                GameObject.Spawn(powerUp);
            }
        }
    }
}
