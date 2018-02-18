using BomberMan2D.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public static class PowerUpFactory
    {
        static PowerUpFactory()
        {
            //init all pools and add them to the dictionary
            GlobalFactory.RegisterPool(typeof(SpeedPow), () => new SpeedPow());
            GlobalFactory.RegisterPool(typeof(BombPow), () => new BombPow());
        }

        public static IPowerup Get(PowerUpType type)
        {
            IPowerup toReturn = null;
            switch (type)
            {
                case PowerUpType.PW_SPEED:
                    toReturn = GlobalFactory.Get(typeof(SpeedPow)) as IPowerup;
                    break;
                case PowerUpType.PW_BOMB:
                    toReturn = GlobalFactory.Get(typeof(BombPow)) as IPowerup;
                    break;
                case PowerUpType.PW_BOMB_PASS:
                    break;
                case PowerUpType.PW_FLAME:
                    break;
                case PowerUpType.PW_FLAME_PASS:
                    break;
                case PowerUpType.PW_WALL_PASS:
                    break;
                case PowerUpType.PW_MYSTERY:
                    break;
                case PowerUpType.PW_DETONATOR:
                    break;
                default:
                    break;
            }

            return toReturn;
        }

    }
}
