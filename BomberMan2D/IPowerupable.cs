using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D
{
    public interface IPowerupable
    {
        void ApplySpeed(float amount);
        float ApplyHealth(int amount);
    }
}
