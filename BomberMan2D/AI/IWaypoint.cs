using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BomberMan2D
{
    public interface IWaypoint
    {
        Vector2 Location { get; set; }
    }
}
