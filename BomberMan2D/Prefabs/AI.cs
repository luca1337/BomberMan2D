﻿using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using BomberMan2D.Components;
using BomberMan2D.Main;
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
        public IMap Map { get; set; }
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

            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.CollisionMode = CollisionMode.Trigger;
            collider.TriggerEnter += OnTriggerEnter;
            AddComponent(collider);

            AddComponent(new BoxCollider2DRenderer(new Vector4(-1f, -1f, 1f, 0f)));

            AddComponent(new FSMUpdater(states));
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if(other.Owner is Explosion)
            {
                Console.WriteLine("AI Collided With: {0}", other.Owner);
            }
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
            public StateIdle NextIdle { get; set; }
            private IWaypoint next;

            private IState chase;

            private AI owner { get; set; }

            private GameObject target;
            private bool oneTimeChase;

            public ChaseState(AI owner)
            {
                this.owner = owner;
                next = owner.CurrentTarget;
                chase = this;
                oneTimeChase = true;
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
                if (!owner.IsInRadius(out target))
                {
                    if (oneTimeChase)
                    {
                        next = GameManager.GetAllPoints()[RandomManager.Instance.Random.Next(0, GameManager.GetPointsCount())];
                        Console.WriteLine(next);
                        oneTimeChase = !oneTimeChase;
                    }


                    if (next is TargetPoint && target != next)
                    {
                        owner.ComputePath(LevelManager.CurrentMap, (int)((next as TargetPoint).Transform.Position.X + 0.5f), (int)((next as TargetPoint).Transform.Position.Y + 0.5f));
                    }

                    if ((next.Location - owner.Transform.Position).Length < 1f || owner.CurrentPath == null)
                        oneTimeChase = !oneTimeChase;

                    return owner.CheckAgentPath(owner, out chase);
                }
                else
                {
                    NextPatrol.OnStateEnter();
                    return NextPatrol;
                }

            }
        }

        private class PatrolState : IState
        {
            public ChaseState ChaseState { get; set; }
            private AI owner;
            private GameObject target;
            private IState patrol;
            private  Node pos;
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
                //TODO : to fix
                if (target is Bomberman)
                {
                   pos  = LevelManager.CurrentMap.GetNodeByIndex((int)target.Transform.Position.X, (int)target.Transform.Position.Y);
                    if (pos == null)
                        Console.WriteLine(" Null");
                }
                  
               
                if (owner.IsInRadius(out target) && pos != null )
                {

                    if (target is Bomberman)
                    {
                        owner.CurrentTarget = target as IWaypoint;
                        owner.ComputePath(LevelManager.CurrentMap, (int)((owner.CurrentTarget as Bomberman).Transform.Position.X + 0.5f), (int)((owner.CurrentTarget as Bomberman).Transform.Position.Y + 0.5f));
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
