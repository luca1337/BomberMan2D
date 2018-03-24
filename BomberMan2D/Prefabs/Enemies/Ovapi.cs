﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using BomberMan2D.AI;
using BomberMan2D.Interfaces;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Ovapi : AI
    {
        public override Transform RefTransform => Transform;
        public override ulong Score => 2000;
        public override float Speed => 0.9f;
        public override float Radius => 3.0f;
        public override bool CanPassWall => true;

        public Ovapi() : base("Ovapi", FlyWeight.Get("Ovapi"), 50, 50, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false)
        {

        }

        public override void OnTriggerEnter(Collider2D other)
        {
        }
    }
}
