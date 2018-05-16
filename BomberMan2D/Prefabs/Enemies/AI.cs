using BehaviourEngine;
using System;
using System.Collections.Generic;
using OpenTK;
using Aiv.Fast2D;
using BehaviourEngine.Pathfinding;

namespace BomberMan2D
{
    public abstract class AI : GameObject, IEnemy, IPathfind
    {
        #region Navmesh
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

        public Bomberman Player { get; set; }
        public IWaypoint Target { get; set; }
        #endregion

        #region Renderer
        protected AnimationRenderer Renderer { get; private set; }

        public abstract Transform RefTransform { get; }


        #endregion

        #region FSM
        public ChaseState Chase => chaseState;

        private PatrolState patrolState;
        private ChaseState chaseState;
        #endregion

        #region Interface Vars
        public abstract ulong Score { get; }
        public abstract float Speed { get; }
        public abstract float Radius { get; }
        public abstract bool CanPassWall { get; }
        #endregion

        #region Constructor
        protected AI(string name, Texture tex, int width, int height, int tilesPerRow, int[] keyFrames, float frameLength, bool show, bool stop) : base(name)
        {
            Renderer = new AnimationRenderer(tex, width, height, tilesPerRow, keyFrames, frameLength, show, stop);
            Renderer.RenderOffset = (int)RenderLayer.AI;
            AddComponent(Renderer);
            this.Layer = (uint)CollisionLayer.Enemy;
            patrolState = new PatrolState(this);
            chaseState = new ChaseState(this);

            patrolState.Next = chaseState;
            chaseState.Next = patrolState;

            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.CollisionEnter += OnCollisionEnter; 
            AddComponent(collider);

            Rigidbody2D rigidBody = new Rigidbody2D();
            rigidBody.IsGravityAffected = false;
            AddComponent(rigidBody);

            AddComponent(new BoxCollider2DRenderer(new Vector4(1f,0f,0f,0f)));
            patrolState.OnStateEnter();

            AddComponent(new FSMUpdater(patrolState));
        }
        #endregion

        #region Collision Events
        public abstract void OnCollisionEnter(Collider2D other, HitState hitstate);
        #endregion

        #region Pathfinding
        public virtual void ComputePath<T>(T item, int x, int y) where T : IMap
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

        public virtual void ExecutePath()
        {
            if (!Computed)
            {
                Vector2 targetPos = CurrentPath[0].Position;
                if (targetPos != Transform.Position)
                {
                    Vector2 direction = (targetPos - Transform.Position).Normalized();
                    Transform.Position += direction * Speed * Time.DeltaTime;
                }

                float distance = (targetPos - Transform.Position).Length;

                if (distance <= 0.1f)
                {
                    CurrentPath.RemoveAt(0);
                }
            }
        }

        public virtual bool IsInRadius(out GameObject target)
        {
            float distance = ((Player as Bomberman).Transform.Position - this.Transform.Position).Length;

            target = null;

            if ((Player as Bomberman).IsBadIndex) return false;

            if (distance < Radius)
            {
                target = Player as Bomberman;
                return true;
            }

            return false;
        }
        #endregion

        #region IPoolable
        public virtual void OnGet()
        {
            this.Active = true;
        }

        public virtual void OnRecycle()
        {
            this.Active = false;
        }
        #endregion

        #region FSM
        public class PatrolState : IState
        {
            private AI owner { get; set; }
          
            public ChaseState Next { get; set; }

            private GameObject target = null;

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
                        owner.Target = target as IWaypoint;
                        owner.ComputePath(LevelManager.CurrentMap, (int)((owner.Target as Bomberman).Transform.Position.X + 0.5f), (int)((owner.Target as Bomberman).Transform.Position.Y + 0.5f));
                    }
                }
                else
                {
                    this.OnStateExit();
                    Next.OnStateEnter();
                    return Next;
                }

                if (owner.CurrentPath == null)
                    return this;

                if (owner.CurrentPath.Count == 0)
                {
                    owner.CurrentPath = null;
                    return this;
                }

                owner.ExecutePath();

                return this;
            }
        }

        public class ChaseState : IState
        {
            private AI owner { get; set; }
            public PatrolState Next { get; set; }
            public bool doChase;

            private IWaypoint next;
            private GameObject target = null;
            private bool oneTimeChase;

            public ChaseState(AI owner)
            {
                this.owner = owner;
                next = owner.Target;
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
                    if (doChase)
                    {
                        oneTimeChase = true;
                        doChase      = false;
                    }

                    if (oneTimeChase)
                    {
                        next = GameManager.GetAllPoints()[RandomManager.Instance.Random.Next(0, GameManager.GetPointsCount())];
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

                    owner.ExecutePath();

                }
                else
                {
                    OnStateExit();
                    Next.OnStateEnter();
                    return Next;
                }
                return this;
            }
        }
        #endregion

    }
}
