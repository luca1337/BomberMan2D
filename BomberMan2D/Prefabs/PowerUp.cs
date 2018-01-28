using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine.Utils;

namespace BomberMan2D.Prefabs
{
    public class PowerUp : GameObject, IPowerup
    {
        List<string> textures = new List<string>();
        PowerUpType powerUpType = PowerUpType.PRP_NONE;
        private List<float> speedValues = new List<float>();

        public PowerUp()
        {
            textures.Add("Health");
            textures.Add("Speed");

            SpriteRenderer renderer = new SpriteRenderer(FlyWeight.Get(textures[(int)powerUpType]));
            AddComponent(renderer);

            speedValues = GetRandomFloats(5, 1.5f, 3.4f);
        }

        public void ApplyPowerUp(IPowerupable powerUp)
        {
            if (powerUpType == PowerUpType.PRP_HEALTH)
                powerUp.ApplyHealth(1);
            else
                powerUp.ApplySpeed(speedValues[RandomManager.Instance.Random.Next(0, speedValues.Count)]);
        }

        private List<float> GetRandomFloats(int size, float min, float max)
        {
            List<float> floatList = new List<float>();

            for (int i = 0; i < size; i++)
            {
                floatList.Add(Utils.GenerateRandomFloatInRange(min, max));
            }

            return floatList;
        }
    }
}
