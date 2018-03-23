using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using BomberMan2D.AI;
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

        #region Target
        public IWaypoint Player { get; set; }
        public IWaypoint CurrentTarget { get; set; }
        public IMap Map { get; set; }
        #endregion

        private AnimationRenderer aiAnimation;
        private float radius = 4f;

        #region FSM

        private PatrolState patrol;
        private ChaseState chase;

        #endregion

        public AI() : base("AI")
        {
            aiAnimation = new AnimationRenderer(FlyWeight.Get("AI"), ((int)(float)Math.Floor(18.5f)), 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false);
            AddComponent(aiAnimation);

            aiAnimation.RenderOffset = (int)RenderLayer.AI;

            //Create state
            patrol = new PatrolState(this);
            chase = new ChaseState(this);

            //Link state
            patrol.ChaseState = chase;
            chase.NextPatrol = patrol;

            //Box collider
            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.CollisionMode = CollisionMode.Trigger;
            collider.TriggerEnter += OnTriggerEnter;
            AddComponent(collider);

            //AddComponent(new BoxCollider2DRenderer(new Vector4(-1f, -1f, 1f, 0f)));

            patrol.OnStateEnter();
            AddComponent(new FSMUpdater(patrol));
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

        #region Private Method

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.Owner is Explosion)
            {
                Console.WriteLine("AI Collided With: {0}", other.Owner);
            }
        }

        private void DoPath()
        {
            if (!Computed)
            {
                Vector2 targetPos = CurrentPath[0].Position;
                if (targetPos != Transform.Position)
                {
                    Vector2 direction = (targetPos - Transform.Position).Normalized();
                    Transform.Position += direction * 1.5f * Time.DeltaTime;
                }

                float distance = (targetPos - Transform.Position).Length;

                if (distance <= 0.1f)
                {
                    CurrentPath.RemoveAt(0);
                }
            }
        }

        private bool IsInRadius(out GameObject target)
        {
            target = null;

            float distance = ((Player as Bomberman).Transform.Position - this.Transform.Position).Length;

            if ((Player as Bomberman).IsBadIndex) return false;

            Console.WriteLine();

            if (distance < radius)
            {
                target = Player as Bomberman;
                return true;
            }

            return false;
        }

        #endregion

        #region Nested State

        private class ChaseState : IState
        {
            public PatrolState NextPatrol { get; set; }

            private IWaypoint next;

            private AI owner { get; set; }

            private GameObject target;
            private bool oneTimeChase;

            public ChaseState(AI owner)
            {
                this.owner = owner;
                next = owner.CurrentTarget;
                oneTimeChase = true;
            }

            public void OnStateEnter()
            {
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

                    if (owner.CurrentPath == null)
                        return this;

                    if (owner.CurrentPath.Count == 0)
                    {
                        owner.CurrentPath = null;
                        return this;
                    }

                    owner.DoPath();
                   
                }
                else
                {
                    OnStateExit();
                    NextPatrol.OnStateEnter();
                    return NextPatrol;
                }
                return this;
            }

           

        }

        private class PatrolState : IState
        {
            public ChaseState ChaseState { get; set; }

            private AI owner;
            private GameObject target;

            public PatrolState(AI owner)
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
                if (owner.IsInRadius(out target))
                {
                    if (target is Bomberman)
                    {
                        owner.CurrentTarget = target as IWaypoint;
                        owner.ComputePath(LevelManager.CurrentMap, (int)((owner.CurrentTarget as Bomberman).Transform.Position.X + 0.5f), (int)((owner.CurrentTarget as Bomberman).Transform.Position.Y + 0.5f));
                    }
                }
                else
                {
                    this.OnStateExit();
                    ChaseState.OnStateEnter();
                    return ChaseState;
                }

                if (owner.CurrentPath == null)
                    return this;

                if (owner.CurrentPath.Count == 0)
                {
                    owner.CurrentPath = null;
                    return this;
                }

                owner.DoPath();
          
                return this;
            }

        }

        #endregion
    }
}
