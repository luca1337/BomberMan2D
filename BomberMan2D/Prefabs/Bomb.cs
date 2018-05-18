using System;
using System.Collections.Generic;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D
{
    public class Bomb : GameObject
    {
        //Property
        public List<Explosion> explosionList = new List<Explosion>();
        public bool Exploding { get; private set; }
        public  bool IsBig;

        //Private Field
        private int bigExplosion               = 9;
        private int littleExplosion            = 5;
        private AnimationRenderer renderer;
        private BoxCollider2D collider;
        private List<Vector2> locations;
        private StateExplode explode;
        private List<BoxCollider2D> colliders;
        private StateWait wait;

        public Bomb() : base("Bomb")
        {
            this.Layer = (uint)CollisionLayer.Bombs;

            collider = new BoxCollider2D(Vector2.One);
            collider.CollisionMode = CollisionMode.Collision;
            collider.CollisionEnter += OnCollisionEnter;
            colliders = new List<BoxCollider2D>();
            AddComponent(collider);
            AddComponent(new BoxCollider2DRenderer(Vector4.Zero));

      

            locations = new List<Vector2>();
            renderer  = new AnimationRenderer(FlyWeight.Get("Bomb"), 50, 50, 3, new int[] { 0, 1, 2, 1 }, 0.2f, true, false);
            AddComponent(renderer);

            #region FSM
            wait    = new StateWait(this);
            explode = new StateExplode(this);

            explode.Next = wait;
            wait.Next    = explode;

            wait.OnStateEnter();
            AddComponent(new FSMUpdater(wait));

            #endregion

            for (int i = 0; i < bigExplosion; i++)
            {
                Explosion toAdd = Pool<Explosion>.GetInstance(x => x.Active = false);
                explosionList.Add(toAdd);
            }
            // ChooseBomb();
        }

        private void OnCollisionEnter(Collider2D other, HitState hitState)
        {
        }

        public  List<Vector2> GetAdjacentLocation(Vector2 from)
        {
            List<Vector2> adjacentLocation = new List<Vector2>();

            if (Map.GetIndexExplosion(true, (int)from.X, (int)from.Y))
                adjacentLocation.Add(new Vector2(from.X, from.Y));

            if (Map.GetIndexExplosion(true, (int)from.X - 1, (int)from.Y))
                adjacentLocation.Add(new Vector2(from.X - 1, from.Y));

            if (Map.GetIndexExplosion(true, (int)from.X, (int)from.Y - 1))
                adjacentLocation.Add(new Vector2(from.X, from.Y - 1));

            if (Map.GetIndexExplosion(true, (int)from.X + 1, (int)from.Y))
                adjacentLocation.Add(new Vector2(from.X + 1, from.Y));

            if (Map.GetIndexExplosion(true, (int)from.X, (int)from.Y + 1))
                adjacentLocation.Add(new Vector2(from.X, from.Y + 1));


            if (IsBig)
            {
                if (Map.GetIndexExplosion(true, (int)from.X - 2, (int)from.Y) && Map.GetIndexExplosion(true, (int)from.X - 1, (int)from.Y))
                    adjacentLocation.Add(new Vector2(from.X - 2, from.Y));

                if (Map.GetIndexExplosion(true, (int)from.X, (int)from.Y - 2) && Map.GetIndexExplosion(true, (int)from.X, (int)from.Y - 1))
                    adjacentLocation.Add(new Vector2(from.X, from.Y - 2));

                if (Map.GetIndexExplosion(true, (int)from.X + 2, (int)from.Y) && Map.GetIndexExplosion(true, (int)from.X + 1, (int)from.Y))
                    adjacentLocation.Add(new Vector2(from.X + 2, from.Y));

                if (Map.GetIndexExplosion(true, (int)from.X, (int)from.Y + 2) && Map.GetIndexExplosion(true, (int)from.X, (int)from.Y + 1))
                    adjacentLocation.Add(new Vector2(from.X, from.Y + 2));
            }

            return adjacentLocation;
        }

        private void ChooseBomb()
        {
            if (!IsBig)
            {
                for (int i = 0; i < littleExplosion; i++)
                {
                    Explosion toAdd = GlobalFactory<Explosion>.Get(typeof(Explosion));
                    toAdd.Active = false;
                    explosionList.Add(toAdd);
                }
            }
            else
            {
                for (int i = 0; i < bigExplosion; i++)
                {
                    Explosion toAdd = GlobalFactory<Explosion>.Get(typeof(Explosion));
                    toAdd.Active = false;
                    explosionList.Add(toAdd);
                }
            }
        }

        private class StateExplode : IState
        {
            public StateWait Next { get; set; }

            private Bomb owner;
            private Timer timer;
            private bool oneTimeSpawn = true;

            public StateExplode(Bomb owner)
            {
                this.owner = owner;
                timer      = new Timer(2.1f);
            }

            public void OnStateEnter()
            {
                timer.Start(true);
                owner.Exploding = true;
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if (owner.Exploding)
                {
                    owner.locations = owner.GetAdjacentLocation(owner.Transform.Position);

                    owner.GetComponent<AnimationRenderer>().Enabled = false;

                    for (int i = 0; i < owner.locations.Count; i++)
                    {
                        owner.explosionList[i].Transform.Position = owner.locations[i];
                        owner.explosionList[i].Active             = true;
                    }

                    if (oneTimeSpawn)
                    {
                        foreach (Explosion item in owner.explosionList)
                        {
                            Spawn(item);
                        }
                        oneTimeSpawn = false;
                    }

                    owner.Exploding = false;
                }

                if (timer.IsActive)
                    timer.Update(false);

                if (!timer.IsActive)
                {
                    #region Explosions recycle

                    for (int i = 0; i < owner.explosionList.Count; i++)
                    {
                        owner.explosionList[i].Active = false;
                        owner.explosionList[i].Reset();
                    }

                    #endregion

                    owner.Active = false;
                    GlobalFactory<Bomb>.Recycle(typeof(Bomb), owner);
                    
                   // Pool<Bomb>.RecycleInstance
                   // (
                   //     owner, x =>
                   //     {
                   //         x.Active = false;
                   //     }
                   // );

                    OnStateExit();
                    Next.OnStateEnter();
                    return Next;
                }
                return this;
            }
        }

        private class StateWait : IState
        {
            public StateExplode Next { get; set; }

            private GameObject owner;
            private Timer timer;

            public StateWait(GameObject owner)
            {
                this.owner = owner;
                timer = new Timer(3f);
            }

            public void OnStateEnter()
            {
                timer.Start(true);
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if (timer.IsActive)
                    timer.Update(false);

                if (!timer.IsActive)
                {
                    Next.OnStateEnter();
                    return Next;
                }
                return this;
            }
        }
    }
}