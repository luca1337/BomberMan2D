using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D.Prefabs
{
    public class Map : GameObject
    {
        public Map()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject.Spawn(new Tile(new Vector2(i % 20 * 50, i / 20 * 50)));
            }
        }
    }
}
