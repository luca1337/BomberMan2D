using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BomberMan2D.AI;
using BomberMan;
using BomberMan2D.Components;
using BomberMan2D.Main;
using OpenTK;

namespace BomberMan2D.Prefabs.Enemies
{
    public class Balloom : GameObject, IEnemy, IPathfind
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
        private AnimationRenderer renderer;
        #endregion       

        #region FSM
        private PatrolState patrolState;
        private ChaseState chaseState;
        #endregion

        #region Global Private Vars
        private float radius = 4f;
        #endregion

        #region Constructor
        public Balloom()
        {
            renderer = new AnimationRenderer(FlyWeight.Get("AI"), (int)18.5f, 17, 4, new int[] { 0, 1, 2, 3 }, 0.2f, true, false);
            renderer.RenderOffset = (int)RenderLayer.AI;
            AddComponent(renderer);

            patrolState = new PatrolState(this);
            chaseState = new ChaseState(this);

            patrolState.Next = chaseState;
            chaseState.Next = patrolState;

            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.CollisionMode = CollisionMode.Trigger;
            collider.TriggerEnter += OnTriggerEnter; ;
            AddComponent(collider);

            patrolState.OnStateEnter();

            AddComponent(new FSMUpdater(patrolState));
        }
        #endregion

        #region Collision Events
        private void OnTriggerEnter(Collider2D other)
        {
            if (other.Owner is Explosion)
            {
                Console.WriteLine("Collidng with explosion");
            }
        }
        #endregion

        #region Pathfinding
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

        public void DoPath()
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

        public bool IsInRadius(GameObject target)
        {
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

        #region IPoolable
        public void OnGet()
        {
            throw new NotImplementedException();
        }

        public void OnRecycle()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FSM

        internal class PatrolState : IState
        {
            private Balloom owner { get; set; }
            public ChaseState Next { get; set; }

            private GameObject target;

            public PatrolState(Balloom owner)
            {
                this.owner = owner;
            }

            public void OnStateEnter()
            {
                throw new NotImplementedException();
            }

            public void OnStateExit()
            {
                throw new NotImplementedException();
            }

            public IState OnStateUpdate()
            {
                if (owner.IsInRadius(target))
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

                owner.DoPath();

                return this;
            }
        }

        internal class ChaseState : IState
        {
            private Balloom owner { get; set; }
            public PatrolState Next { get; set; }

            private IWaypoint next;
            private GameObject target;
            private bool oneTimeChase;

            public ChaseState(Balloom owner)
            {
                this.owner = owner;
                next = owner.Target;
                oneTimeChase = true;
            }

            public void OnStateEnter()
            {
                throw new NotImplementedException();
            }

            public void OnStateExit()
            {
                throw new NotImplementedException();
            }

            public IState OnStateUpdate()
            {
                if (!owner.IsInRadius(target))
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
                    Next.OnStateEnter();
                    return Next;
                }
                return this;
            }
        }

        #endregion

    }
}
