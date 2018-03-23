﻿using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.AI;
using BomberMan2D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Interfaces
{
    /// <summary>
    /// this interface exposes these fields and determines the exact behavior of the enemy
    /// </summary>
    public interface IEnemy : IPoolable
    {
        /// <summary>
        /// Determines if ref target is inside enemy's radius.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool IsInRadius(GameObject target);

        /// <summary>
        /// void method that just incapsulate the execution path for the Enemy to the target.
        /// </summary>
        void ExecutePath();

        Transform RefTransform { get; }

        /// <summary>
        /// Player which will be the out target.
        /// </summary>
        Prefabs.Bomberman Player { get; set; }

        /// <summary>
        /// Target that enemy will switch to, from player to a generic target point around the map.
        /// </summary>
        IWaypoint Target { get; set; }

        /// <summary>
        /// Score that each enemy will give when killed.
        /// </summary>
        ulong Score { get; }

        /// <summary>
        /// Speed that tells how fast the enemy will move.
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// Radius of the enemy that will turn him to move towards the target.
        /// </summary>
        float Radius { get; set; }
    }
}
