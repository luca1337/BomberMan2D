using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    [Flags]
    public enum CollisionLayer : uint
    {
        Default = 1,
        BomberMan = 2,
        Enemy = 4,
        Wall = 8,
        Bombs = 16,
        Powerup = 32
    }
}
