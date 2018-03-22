using BomberMan2D.Enums;
using BomberMan2D.Interfaces;
using BomberMan2D.Prefabs.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Factories
{
    public static class EnemyFactory
    {
        static EnemyFactory()
        {
            // .. register pools here

            GlobalFactory<Balloom>.RegisterPool(typeof(Balloom), () => new Balloom());

            // ..
        }

        public static IEnemy Get(EnemyType type)
        {
            IEnemy returnInstance = null;

            switch (type)
            {
                case EnemyType.Balloom:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Balloom));
                    break;
                case EnemyType.Doll:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Doll));
                    break;
                case EnemyType.Oneal:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Oneal));
                    break;
                case EnemyType.Minvo:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Minvo));
                    break;
                case EnemyType.Ovapi:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Ovapi));
                    break;
                case EnemyType.Kondoria:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Kondoria));
                    break;
                case EnemyType.Pass:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Pass));
                    break;
                case EnemyType.Pontan:
                    returnInstance = GlobalFactory<IEnemy>.Get(typeof(Pontan));
                    break;
                default:
                    break;
            }

            return returnInstance;
        }
    }
}
