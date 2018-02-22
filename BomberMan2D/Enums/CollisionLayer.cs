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
        SolidWall = 16,
        Bombs = 32,
        Powerup = 64,
        Explosion = 128
    }
}
