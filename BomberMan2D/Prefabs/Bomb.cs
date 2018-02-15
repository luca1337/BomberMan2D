using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using BomberMan2D.Prefabs;
using OpenTK;

namespace BomberMan2D
{
    public class Bomb : GameObject
    {
        //Property
        public List<Explosion> explosionList = new List<Explosion>();
        public int ExplosionForce            = 5;
        public bool Exploding { get; private set; }
    
        //Private Field
        private AnimationRenderer renderer;
        private List<BoxCollider2D> colliders;
        private List<Vector2> locations;
        private StateExplode explode;
        private StateWait wait;
        private IState currentState;

        public Bomb() : base("Bomb")
        {
            this.Layer = (uint)CollisionLayer.BomberMan;

            colliders = new List<BoxCollider2D>();
            locations = new List<Vector2>();
            renderer = new AnimationRenderer(FlyWeight.Get("Bomb"), 150, 150, 4, new int[] { 0, 1, 2, 3, 2 }, 0.2f, true, false);
            AddComponent(renderer);

            #region FSM
            wait = new StateWait(this);
            explode = new StateExplode(this);

            explode.Next = wait;
            wait.Next = explode;

            wait.OnStateEnter();
            currentState = wait;
            AddComponent(new UpdateBomb(currentState));

            #endregion

            for (int i = 0; i < ExplosionForce; i++)
            {
                Explosion toAdd = Pool<Explosion>.GetInstance(x => x.Active = false);
                explosionList.Add(toAdd);
            }
        }

        private class UpdateBomb : BehaviourEngine.Component, IUpdatable
        {
            private IState currentState;

            public UpdateBomb(IState state) : base()
            {
                this.currentState = state;
            }

            public void Update()
            {
                currentState = currentState.OnStateUpdate();
            }
        }

        public static List<Vector2> GetAdjacentLocation(Vector2 from)
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

            return adjacentLocation;
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
                timer.Start();
                owner.Exploding = true;
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if (owner.Exploding)
                {
                    owner.locations = GetAdjacentLocation(owner.Transform.Position);

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
                    timer.Update();

                if (!timer.IsActive)
                {
                    #region Explosions recycle

                    for (int i = 0; i < owner.explosionList.Count; i++)
                    {
                        owner.explosionList[i].Active = false;
                        owner.explosionList[i].Reset();
                    }

                    #endregion

                    Pool<Bomb>.RecycleInstance
                    (
                        owner, x =>
                        {
                            x.Active = false;
                        }
                    );

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
                timer = new Timer(1f);
            }

            public void OnStateEnter()
            {
                timer.Start();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if (timer.IsActive)
                    timer.Update();

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