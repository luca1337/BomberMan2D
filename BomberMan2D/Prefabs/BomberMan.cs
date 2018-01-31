using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Prefabs
{
    public class Bomberman : GameObject, IPowerupable, IWaypoint
    {
        #region Animations
        private Dictionary<AnimationType, AnimationRenderer> playerAnimations = new Dictionary<AnimationType, AnimationRenderer>();
        #endregion

        #region FSM
        private StateDrop drop;
        private WalkUp walkUp;
        private WalkDown walkDown;
        private WalkLeft walkLeft;
        private WalkRight walkRight;
        private Idle idle;
        private IState walkState;
        private IState bombState;
        private List<IState> states = new List<IState>();

        public Vector2 Location { get; set; }
        #endregion

        public Bomberman() : base("BomberMan")
        {
            #region LayerMask
            this.Layer = (uint)CollisionLayer.BomberMan;
            #endregion

            #region Animations
            playerAnimations.Add(AnimationType.WALK_RIGHT, new AnimationRenderer(FlyWeight.Get("BomberMan"), 68, 88, 7, new int[] { 35, 35, 36, 36, 37, 37, 37 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.WALK_LEFT, new AnimationRenderer(FlyWeight.Get("BomberMan"), 69, 88, 7, new int[] { 44, 44, 43, 43, 42, 42, 42 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.WALK_DOWN, new AnimationRenderer(FlyWeight.Get("BomberMan"), 64, 87, 7, new int[] { 0, 0, 1, 1, 2, 2, 2 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.WALK_UP, new AnimationRenderer(FlyWeight.Get("BomberMan"), 64, 87, 7, new int[] { 7, 7, 8, 8, 9, 9, 9 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.IDLE, new AnimationRenderer(FlyWeight.Get("BomberMan"), 62, 87, 7, new int[] { 0 }, 0.04f, true, false));

            playerAnimations.ToList().ForEach(item => AddComponent(item.Value));
            playerAnimations.ToList().ForEach(item => item.Value.RenderOffset = (int)RenderLayer.BomberMan);
            #endregion

            #region FSM
            drop = new StateDrop();
            walkUp = new WalkUp();
            walkDown = new WalkDown();
            walkLeft = new WalkLeft();
            walkRight = new WalkRight();
            idle = new Idle();

            drop.Owner = this;
            walkUp.Owner = this;
            walkDown.Owner = this;
            walkLeft.Owner = this;
            walkRight.Owner = this;
            idle.Owner = this;
            //walk up
            walkUp.NextDown = walkDown;
            walkUp.NextLeft = walkLeft;
            walkUp.NextRight = walkRight;
            walkUp.NextIdle = idle;

            //walk down
            walkDown.NextUp = walkUp;
            walkDown.NextLeft = walkLeft;
            walkDown.NextRight = walkRight;
            walkDown.NextIdle = idle;

            //walk left
            walkLeft.NextUp = walkUp;
            walkLeft.NextDown = walkDown;
            walkLeft.NextRight = walkRight;
            walkLeft.NextIdle = idle;

            //walk right
            walkRight.NextLeft = walkLeft;
            walkRight.NextUp = walkUp;
            walkRight.NextDown = walkDown;
            walkRight.NextIdle = idle;

            //idle
            idle.NextDown = walkDown;
            idle.NextUp = walkUp;
            idle.NextLeft = walkLeft;
            idle.NextRight = walkRight;

            //assign current state
            idle.OnStateEnter();
            walkState = idle;

            //bomb fsm
            drop.OnStateEnter();
            bombState = drop;

            states.Add(walkState);
            states.Add(drop);

            #endregion

            #region Components
            AddComponent(new Components.CharacterController());
            AddComponent(new Components.FSMUpdater(states));

            BoxCollider2D collider2D = new BoxCollider2D(new Vector2(0.5f, 0.5f));
            collider2D.TriggerEnter += OnTriggerEnter;
            AddComponent(collider2D);

            Rigidbody2D rigidBody = new Rigidbody2D();
            rigidBody.IsGravityAffected = false;
            AddComponent(rigidBody);

            AddComponent(new Components.CameraFollow());

            #endregion
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if(other.Owner is IPowerup)
            {
                IPowerup powerup = other.Owner as IPowerup;
                powerup.ApplyPowerUp(this);
            }
        }

        private void EnableAnimation(AnimationType animationType, bool enable)
        {
            KeyValuePair<AnimationType, AnimationRenderer> first = playerAnimations.Single(x => x.Key == animationType);
            first.Value.Show = enable; first.Value.Stop = !enable;

            IEnumerable<KeyValuePair<AnimationType, AnimationRenderer>> second = playerAnimations.Where(x => x.Key != animationType);
            second.ToList().ForEach(x =>
            {
                x.Value.Show = !enable;
                x.Value.Stop = !enable;
            });
        }

        public void ApplySpeed(float amount)
        {
            Console.WriteLine("Speed Applied!");
        }

        public float ApplyHealth(int amount)
        {
            Console.WriteLine("Health Applied!");
            return 0.0f;
        }

        private class StateDrop : IState
        {
            public GameObject Owner { get; set; }

            private Timer timer;

            public StateDrop( )
            {
                timer = new Timer(2f);
            }

            public void OnStateEnter()
            {
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                if (Input.IsKeyDown(KeyCode.Space) && !timer.IsActive)
                {
                    //AudioManager.PlayClip(AudioType.SOUND_DROP);

                   GameObject.Spawn(Pool<Bomb>.GetInstance(x =>
                   {
                       x.Active = true;
                       x.Stop = false;
                       x.Show = true;
                       x.Transform.Position = new Vector2((int)Owner.Transform.Position.X, (int)Owner.Transform.Position.Y);
                   }));

                    timer.Start();
                }

                if (timer.IsActive)
                    timer.Update();

                return this;
            }
        }

        private class WalkLeft : IState
        {
            public WalkUp NextUp { get; set; }
            public WalkDown NextDown { get; set; }
            public WalkRight NextRight { get; set; }
            public Idle NextIdle { get; set; }
            public GameObject Owner { get; set; }

            public WalkLeft()
            {
            }

            public void OnStateEnter() => OnStateUpdate();

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                (Owner as Bomberman).EnableAnimation(AnimationType.WALK_LEFT, true);
                return this;
            }
        }

        private class WalkRight : IState
        {
            public WalkUp NextUp { get; set; }
            public WalkDown NextDown { get; set; }
            public WalkLeft NextLeft { get; set; }
            public Idle NextIdle { get; set; }
            public GameObject Owner { get; set; }

            public WalkRight( )
            {
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
                (Owner as Bomberman).EnableAnimation(AnimationType.WALK_RIGHT, true);
                return this;
            }
        }

        private class WalkUp : IState
        {
            public WalkLeft NextLeft { get; set; }
            public WalkDown NextDown { get; set; }
            public WalkRight NextRight { get; set; }
            public Idle NextIdle { get; set; }
            public GameObject Owner { get; set; }

            public WalkUp()
            {
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
                (Owner as Bomberman).EnableAnimation(AnimationType.WALK_UP, true);
                return this;
            }
        }

        private class WalkDown : IState
        {
            public WalkUp NextUp { get; set; }
            public WalkLeft NextLeft { get; set; }
            public WalkRight NextRight { get; set; }
            public Idle NextIdle { get; set; }
            private Bomberman owner { get; set; }
            public GameObject Owner { get; set; }

            public WalkDown()
            {
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
                (Owner as Bomberman).EnableAnimation(AnimationType.WALK_DOWN, true);
                return this;
            }
        }

        private class Idle : IState
        {
            public WalkUp NextUp { get; set; }
            public WalkDown NextDown { get; set; }
            public WalkRight NextRight { get; set; }
            public WalkLeft NextLeft { get; set; }
            public GameObject Owner { get; set; }

            public Idle()
            {
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
                if (Input.IsKeyPressed(KeyCode.S))
                {
                    NextDown.OnStateEnter();
                    return NextDown;
                }

                else if (Input.IsKeyPressed(KeyCode.W))
                {
                    NextUp.OnStateEnter();
                    return NextUp;
                }

                else if (Input.IsKeyPressed(KeyCode.D))
                {
                    NextRight.OnStateEnter();
                    return NextRight;
                }

                else if (Input.IsKeyPressed(KeyCode.A))
                {
                    NextLeft.OnStateEnter();
                    return NextLeft;
                }
                else
                {
                    (Owner as Bomberman).EnableAnimation(AnimationType.IDLE, true);
                    return this;
                }
            }
        }
    }
}
