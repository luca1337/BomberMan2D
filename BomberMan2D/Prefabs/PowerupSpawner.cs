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

                    if (p.powerUpType == PowerUpType.PW_SPEED)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Speed_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_WALL_PASS)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Wallpass_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_FLAME_PASS)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Flamepass_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_FLAME)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Flame_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_DETONATOR)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Detonator_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_MYSTERY)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Mystery_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_BOMB_PASS)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Bombpass_PW");
                    }
                    else if (p.powerUpType == PowerUpType.PW_BOMB)
                    {
                        p.GetComponent<SpriteRenderer>().SetTexture("Bomb_PW");
                    }
                });

                GameObject.Spawn(powerUp);
            }
        }
    }
}
