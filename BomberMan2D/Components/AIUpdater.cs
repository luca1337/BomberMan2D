using BehaviourEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Components
{
    public class AIUpdater : Component, IUpdatable
    {
        private bool doChase = true;
        private bool oneTimeChase;
        private IWaypoint next;
        private AI owner;

        public AIUpdater(AI owner)
        {
            this.owner = owner;
        }

        public void Update()
        {
            if (doChase)
            {
                oneTimeChase = true;
                doChase = false;
            }

            if (oneTimeChase)
            {
                next = GameManager.GetAllPoints()[RandomManager.Instance.Random.Next(0, GameManager.GetPointsCount())];
                oneTimeChase = !oneTimeChase;
            }

            owner.ComputePath(LevelManager.CurrentMap, (int)((next as TargetPoint).Transform.Position.X + 0.5f), (int)((next as TargetPoint).Transform.Position.Y + 0.5f));

            if ((next.Location - owner.Transform.Position).Length < 1f || owner.CurrentPath == null)
                oneTimeChase = !oneTimeChase;

            if (owner.CurrentPath == null)
                return;

            if (owner.CurrentPath.Count == 0)
            {
                owner.CurrentPath = null;
                return;
            }

            owner.ExecutePath();
        }
    }
}
