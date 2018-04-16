namespace BomberMan2D
{
    public static class EnemyFactory
    {
        static EnemyFactory()
        {
            GlobalFactory<IEnemy>.RegisterPool(typeof(Balloom), () => new Balloom());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Doll), () => new Doll());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Oneal), () => new Oneal());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Minvo), () => new Minvo());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Ovapi), () => new Ovapi());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Pass), () => new Pass());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Kondoria), () => new Kondoria());
            GlobalFactory<IEnemy>.RegisterPool(typeof(Pontan), () => new Pontan());
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
