namespace BomberMan2D
{
    public static class PowerUpFactory
    {
        static PowerUpFactory()
        {
            //init all pools and add them to the dictionary
            GlobalFactory<SpeedPow>.RegisterPool(typeof(SpeedPow), () => new SpeedPow());
            GlobalFactory<BombPow>.RegisterPool(typeof(BombPow), () => new BombPow());
            GlobalFactory<WallPass>.RegisterPool(typeof(WallPass), () => new WallPass());
            GlobalFactory<Mystery>.RegisterPool(typeof(Mystery), () => new Mystery());
        }

        public static IPowerup Get(PowerUpType type)
        {
            IPowerup toReturn = null;
            switch (type)
            {
                case PowerUpType.PW_SPEED:
                    toReturn = GlobalFactory<SpeedPow>.Get(typeof(SpeedPow));
                    break;
                case PowerUpType.PW_BOMB:
                    toReturn = GlobalFactory<BombPow>.Get(typeof(BombPow));
                    break;
                case PowerUpType.PW_BOMB_PASS:
                    break;
                case PowerUpType.PW_FLAME:
                    break;
                case PowerUpType.PW_FLAME_PASS:
                    break;
                case PowerUpType.PW_WALL_PASS:
                    toReturn = GlobalFactory<WallPass>.Get(typeof(WallPass));
                    break;
                case PowerUpType.PW_MYSTERY:
                    toReturn = GlobalFactory<Mystery>.Get(typeof(Mystery));
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
