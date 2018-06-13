using BehaviourEngine;
using System;
using System.Collections.Generic;
using OpenTK;
using Aiv.Fast2D;
using BehaviourEngine.Pathfinding;
using BomberMan2D.Components;

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

        #region Renderer
        protected AnimationRenderer Renderer { get; private set; }

        public abstract Transform RefTransform { get; }


        #endregion


        #region Interface Vars
        public abstract ulong Score { get; }
        public abstract float Speed { get; }
        public abstract float Radius { get; }
        public abstract bool CanPassWall { get; }
        #endregion

        protected AI(string name, Texture tex, int width, int height, int tilesPerRow, int[] keyFrames, float frameLength, bool show, bool stop) : base(name)
        {
            Renderer = new AnimationRenderer(tex, width, height, tilesPerRow, keyFrames, frameLength, show, stop);
            Renderer.RenderOffset = (int)RenderLayer.AI;
            AddComponent(Renderer);
            this.Layer = (uint)CollisionLayer.Enemy;

            BoxCollider2D collider = new BoxCollider2D(new Vector2(1, 1));
            collider.CollisionEnter += OnCollisionEnter; 
            AddComponent(collider);

            Rigidbody2D rigidBody = new Rigidbody2D();
            rigidBody.IsGravityAffected = false;
            AddComponent(rigidBody);

            AddComponent(new BoxCollider2DRenderer(new Vector4(1f,0f,0f,0f)));

            AddComponent(new AIUpdater(this));
        }
        #endregion

        #region Collision Events
        public abstract void OnCollisionEnter(Collider2D other, HitState hitstate);
        #endregion

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

    }
}