using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Interfaces
{
    public interface IEnemy : IPoolable
    {
        bool IsInRadius(out GameObject target);
        void DoPath();
    }
}
