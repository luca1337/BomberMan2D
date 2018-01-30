﻿using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using BomberMan2D.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Prefabs
{
    public class AI : GameObject, IPathfind
    {
        #region Pathfinding
        public List<Node> CurrentPath { get; set; }
        public bool Computed
        {
            get
            {
                if ((CurrentPath.Count) == 0)
                    return true;
                return false;
            }
        }
        #endregion

        private AnimationRenderer aiAnimation;
        private float radius = 4f;

        #region Target
        public IWaypoint Player { get; set; }
        public IWaypoint CurrentTarget { get; set; }
        public IMap map { get; set; }
        #endregion

        #region FSM
        private PatrolState patrol;
        private StateIdle idle;
        private ChaseState chase;
        private IState currentState;
        private List<IState> states = new List<IState>();
        #endregion

        public AI() : base("AI")
        {
            aiAnimation = new AnimationRenderer(FlyWeight.Get("AI"), ((int)(float)Math.Floor(18.5f)), 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false);
            AddComponent(aiAnimation);

            patrol = new PatrolState(this);
            idle = new StateIdle(this);
            chase = new ChaseState(this);

            patrol.ChaseState = chase;
            chase.NextPatrol = patrol;

            patrol.OnStateEnter();
            currentState = patrol;

            states.Add(currentState);

            AddComponent(new FSMUpdater(states));
        }

        public void ComputePath<T>(T item, int x, int y) where T : IMap
        {
            int pX, pY;

            if (Math.Abs(Transform.Position.X - (int)Transform.Position.X) > 0.5f)
                pX = (int)Transform.Position.X + 1;
            else
                pX = (int)Transform.Position.X;

            if (Math.Abs(Transform.Position.Y - (int)Transform.Position.Y) > 0.5f)
                pY = (int)Transform.Position.Y + 1;
            else
                pY = (int)Transform.Position.Y;

            CurrentPath = AStar.GetPath(item, pX, pY, x, y);
        }

        private IState CheckAgentPath(AI owner, out IState state)
        {
            state = null;

            if (owner.CurrentPath == null)
                return state;

            if (owner.CurrentPath.Count == 0)
            {
                owner.CurrentPath = null;
                return state;
            }

            if (!owner.Computed)
            {
                Vector2 targetPos = owner.CurrentPath[0].Position;
                if (targetPos != owner.Transform.Position)
                {
                    Vector2 direction = (targetPos - owner.Transform.Position).Normalized();
                    owner.Transform.Position += direction * 1.5f * Time.DeltaTime;
                }

                float distance = (targetPos - owner.Transform.Position).Length;

                if (distance <= 0.1f)
                {
                    owner.CurrentPath.RemoveAt(0);
                }
            }

            return state;
        }

        public bool IsInRadius(out GameObject target)
        {
            float distance = ((Player as GameObject).Transform.Position - this.Transform.Position).Length;

            if (distance < radius)
            {
                target = Player as GameObject;
                return true;
            }

            target = null;
            return false;
        }

        private class ChaseState : IState
        {
            public PatrolState NextPatrol { get; set; }
            public ChaseState NextChase { get; set; }
            public StateIdle NextIdle { get; set; }
            private IWaypoint next;

            private IState chase;

            private AI owner { get; set; }

            private GameObject target;

            public ChaseState(AI owner)
            {
                this.owner = owner;
                next = owner.CurrentTarget;
                chase = this;
            }

            public void OnStateEnter()
            {
                OnStateUpdate();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if(!owner.IsInRadius(out target))
                {
                    next = Game.GetAllPoints()[RandomManager.Instance.Random.Next(0, Game.GetPointsCount())];

                    if (next is TargetPoint)
                    {
                        owner.ComputePath(LevelManager.CurrentMap, (int)((next as TargetPoint).Transform.Position.X + 0.5f), (int)((next as TargetPoint).Transform.Position.Y + 0.5f));
                        Console.WriteLine("computing path");
                    }

                    return owner.CheckAgentPath(owner, out chase);
                }

                NextChase.OnStateEnter();
                return NextChase;
            }
        }
        private class PatrolState : IState
        {
            public ChaseState ChaseState { get; set; }
            private AI owner;
            private GameObject target;
            private IState patrol;

            public PatrolState(AI owner)
            {
                this.owner = owner;
                patrol = this;
            }

            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if (owner.IsInRadius(out target))
                {
                    if (target is BomberMan)
                    {
                        owner.CurrentTarget = target as IWaypoint;
                        owner.ComputePath(LevelManager.CurrentMap, (int)((owner.CurrentTarget as BomberMan).Transform.Position.X + 0.5f), (int)((owner.CurrentTarget as BomberMan).Transform.Position.Y + 0.5f));
                    }
                }
                else
                {
                    ChaseState.OnStateEnter();
                    return ChaseState;
                }

                return owner.CheckAgentPath(owner, out patrol);
            }
        }
        private class StateIdle : IState
        {
            private AI owner;
            public StateIdle(AI owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return this;
            }
        }
    }
}
