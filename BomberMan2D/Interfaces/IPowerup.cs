using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public interface IPowerup
    {
        void ApplyPowerUp(IPowerupable powerUp);
    }
}
