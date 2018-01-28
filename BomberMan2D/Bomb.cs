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
        public bool Exploding { get; private set; }
        public bool Stop { get; set; }
        public bool Show { get; set; }
        
        //Private Field
        private AnimationRenderer   renderer;
        private List<BoxCollider2D> colliders;
        private List<Vector2> locations;
        private StateExplode explode;
        private StateWait wait;
        private IState currentState;


        public Bomb( ) : base("Bomb")
        {
            this.Layer = (uint)CollisionLayer.BomberMan;

            colliders = new List<BoxCollider2D>();
            locations = new List<Vector2>();
            renderer  = new AnimationRenderer(FlyWeight.Get("Bomb"), 150, 150, 4, new int[] { 0, 1, 2, 3, 2 }, 0.2f, true, false);
            AddComponent(renderer);
            #region FSM
            wait    = new StateWait();
            explode = new StateExplode();
            wait.Owner = this;
            explode.Owner = this;

            explode.Next = wait;
            wait.Next    = explode;

            wait.OnStateEnter();
            currentState = wait;
            AddComponent(new UpdateBomb(currentState));

            #endregion
        }

        public void SetAnimation(string animation, Vector2 direction)
        {
            renderer.Owner.Transform.Position = direction;
        }
        public void EnableAnimation(string name, bool stop, bool render)
        {
            renderer.Stop = stop;
            renderer.Show = render;
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
                currentState.Owner = Owner;
                currentState = currentState.OnStateUpdate();
            }
        }

        public static List<Vector2> GetAdjacentLocation(Vector2 from)
        {
            List<Vector2> adjacentLocation = new List<Vector2>();

            if (Map.GetIndex(true, (int)from.X, (int)from.Y))
                adjacentLocation.Add(new Vector2(from.X, from.Y));

            if (Map.GetIndex(true, (int)from.X - 50, (int)from.Y))
                adjacentLocation.Add(new Vector2(from.X - 50, from.Y));

            if (Map.GetIndex(true, (int)from.X, (int)from.Y - 150))
                adjacentLocation.Add(new Vector2(from.X, from.Y - 150));

            if (Map.GetIndex(true, (int)from.X + 50, (int)from.Y))
                adjacentLocation.Add(new Vector2(from.X + 50, from.Y));

            if (Map.GetIndex(true, (int)from.X, (int)from.Y + 150))
                adjacentLocation.Add(new Vector2(from.X, from.Y + 150));

            return adjacentLocation;
        }

        private class StateExplode : IState
        {
            public StateWait Next { get; set; }
            public GameObject Owner { get; set; }

            private Timer timer;
         //   private Explosion explosion;

            public StateExplode( )
            {
                timer      = new Timer(1.8f);
            }

            public void OnStateEnter()
            {
                timer.Start();
                (Owner as Bomb).Exploding = true;
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if ((Owner as Bomb).Exploding)
                {
                    //camera shake
                    CameraManager.Instance.Shake(0.09f, 1.0f);

                    (Owner as Bomb).GetComponent<AnimationRenderer>().Enabled = false;

               //     AudioManager.PlayClip(AudioType.SOUND_EXPLOSION);
                    (Owner as Bomb).locations = GetAdjacentLocation(Owner.Transform.Position);

                    for (int i = 0; i < (Owner as Bomb).locations.Count; i++)
                    {
                        GameObject toSpawn = Spawn(Pool<Explosion>.GetInstance(x =>
                        {
                            x.Transform.Position = (Owner as Bomb).locations[i];
                            x.Active = true;
                            foreach (BehaviourEngine.Component component in x.Components)
                            {
                                if (!component.Enabled)
                                    component.Enabled = true;
                            }
                        }));
                        (Owner as Bomb).explosionList.Add(toSpawn as Explosion);
                    }
                   
                    (Owner as Bomb).Exploding = false;
                }

                if (timer.IsActive)
                    timer.Update();

                if (!timer.IsActive)
                {
                    #region Explosions recycle
                    for (int i = 0; i < (Owner as Bomb).explosionList.Count; i++)
                    {
                        Pool<Explosion>.RecycleInstance
                        (
                             (Owner as Bomb).explosionList[i], x =>
                            {
                                x.Reset();
                                x.Active = false;
                                foreach (BehaviourEngine.Component component in x.Components)
                                {
                                     component.Enabled = false;
                                }
                            }
                        );
                    }
                    
                    (Owner as Bomb).explosionList.Clear();

                    #endregion

                    Pool<Bomb>.RecycleInstance
                    (
                         (Owner as Bomb), x =>
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
            public GameObject Owner { get; set; }
            private Timer timer;

            public StateWait( )
            {
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

                (Owner as Bomb).EnableAnimation("Bomb",(Owner as Bomb).Stop, (Owner as Bomb).Show);

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
