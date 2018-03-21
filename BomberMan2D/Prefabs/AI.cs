using BehaviourEngine;
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
        private ChaseState chase;

        private IState currentState;
        #endregion

        public AI() : base("AI")
        {
            aiAnimation = new AnimationRenderer(FlyWeight.Get("AI"), ((int)(float)Math.Floor(18.5f)), 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false);
            AddComponent(aiAnimation);

            aiAnimation.RenderOffset = (int)RenderLayer.AI;

            patrol = new PatrolState(this);
            chase = new ChaseState(this);

            patrol.ChaseState = chase;
            chase.NextPatrol = patrol;

            patrol.OnStateEnter();
            currentState = patrol;

            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.CollisionMode = CollisionMode.Trigger;
            collider.TriggerEnter += OnTriggerEnter;
            AddComponent(collider);

            //AddComponent(new BoxCollider2DRenderer(new Vector4(-1f, -1f, 1f, 0f)));

            AddComponent(new FSMUpdater(currentState));
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

            

            return state;
        }

        public bool IsInRadius(out GameObject target)
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

        private class ChaseState : IState
        {
            public PatrolState NextPatrol { get; set; }

            private IWaypoint next;

            private IState chase;

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
                    chase = this;

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
            private IState patrol;
            private  Node pos;
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
                    patrol = this;

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
                return this;
            }
        }
    }
}
