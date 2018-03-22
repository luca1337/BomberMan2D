using BomberMan2D.Enums;
using BomberMan2D.Interfaces;
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

        }

        public static IEnemy Get(EnemyType type)
        {
            return null;
        }
    }
}
