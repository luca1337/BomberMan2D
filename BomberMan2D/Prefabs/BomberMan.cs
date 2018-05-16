using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;

namespace BomberMan2D
{
    public class Bomberman : GameObject, IWaypoint
    {
        public CSteamID ID { get; set; }

        public Vector2 Location { get; set; }
        public bool Invulnerability { get; set; }

        private BoxCollider2D collider2D;
        //Animations
        private Dictionary<AnimationType, AnimationRenderer> playerAnimations = new Dictionary<AnimationType, AnimationRenderer>();

        //Bomb drop
        private StateDrop drop;

        private InvulnerabilityManager manager;

        private bool go = false;
        #region Powerups

        public int CurrentExplosion = 0;

        public bool IsBadIndex
        {
            get
            {
                float PosX = this.Transform.Position.X + 0.5f;
                float PosY = this.Transform.Position.Y + 0.5f;

                int index = Map.GetLevelEnumeratedIndex((int)PosX, (int)PosY);

                if (index == 2) // Walkable wall index
                    return true;
                return false;
            }
        }

        private IPowerup powerup { get; set; }
        private PowerUpType pType { get; set; }

        #endregion

        public Bomberman() : base("BomberMan")
        {
            // LayerMask
            this.Layer = (uint)CollisionLayer.BomberMan;

            #region Animations
            playerAnimations.Add(AnimationType.WALK_RIGHT, new AnimationRenderer(FlyWeight.Get("BomberMan"), 68, 88, 7, new int[] { 35, 35, 36, 36, 37, 37, 37 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.WALK_LEFT, new AnimationRenderer(FlyWeight.Get("BomberMan"), 69, 88, 7, new int[] { 44, 44, 43, 43, 42, 42, 42 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.WALK_DOWN, new AnimationRenderer(FlyWeight.Get("BomberMan"), 64, 87, 7, new int[] { 0, 0, 1, 1, 2, 2, 2 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.WALK_UP, new AnimationRenderer(FlyWeight.Get("BomberMan"), 64, 87, 7, new int[] { 7, 7, 8, 8, 9, 9, 9 }, 0.04f, false, true));
            playerAnimations.Add(AnimationType.IDLE, new AnimationRenderer(FlyWeight.Get("BomberMan"), 62, 87, 7, new int[] { 0 }, 0.04f, true, false));

            playerAnimations.ToList().ForEach(item => AddComponent(item.Value));
            playerAnimations.ToList().ForEach(item => item.Value.RenderOffset = (int)RenderLayer.Player);
            #endregion


            //Bomb fsm
            drop = new StateDrop(this);
            drop.OnStateEnter();
            AddComponent(new FSMUpdater(drop));

            #region Components

            AddComponent(new CharacterController());
            AddComponent(new UpdateAnimation(this));

            //Collider
            collider2D = new BoxCollider2D(new Vector2(1f, 1f));
            collider2D.CollisionMode = CollisionMode.Collision;
            collider2D.CollisionStay += OnCollisionStay;
            collider2D.TriggerEnter += OnTriggerEnter;
            AddComponent(collider2D);

            AddComponent(new BoxCollider2DRenderer(new Vector4(1f, 0f, 0f, 0f)));
            Rigidbody2D rigidBody = new Rigidbody2D();
            rigidBody.IsGravityAffected = false;
            AddComponent(rigidBody);

            AddComponent(new CameraFollow());

            manager = new InvulnerabilityManager(this);
            AddComponent(manager);
            #endregion
        }

        private void OnCollisionStay(Collider2D other, HitState hitState)
        {

            bool isWall = Map.GetSwap((int)other.Center.X, (int)other.Center.Y);

            if (isWall)
            {
                int spriteSize = 1;
                float deadzone = 0.15f;
                float offset   = 0.25f;
               
                //Top & Bottom Collision (Player)
                bool yLeft  = collider2D.internalTransform.Position.X < other.internalTransform.Position.X + spriteSize + deadzone && collider2D.internalTransform.Position.X > other.internalTransform.Position.X + spriteSize - offset - deadzone;
                bool yRight = collider2D.internalTransform.Position.X + spriteSize < other.internalTransform.Position.X + offset + deadzone && collider2D.internalTransform.Position.X + spriteSize > other.internalTransform.Position.X - deadzone;
                //Left & Right Collision (Player)
                bool xTop = collider2D.internalTransform.Position.Y < other.internalTransform.Position.Y + spriteSize + deadzone && collider2D.internalTransform.Position.Y > other.internalTransform.Position.Y + spriteSize - offset - deadzone;
                bool xBottom = collider2D.internalTransform.Position.Y + spriteSize < other.internalTransform.Position.Y + deadzone + offset && collider2D.internalTransform.Position.Y + spriteSize > other.internalTransform.Position.Y - deadzone;

                float swapDist = 0.05f;

                if (hitState.normal.Y > 0 && yLeft)
                    Transform.Position += new Vector2(swapDist, 0f);
                else if (hitState.normal.Y > 0 && yRight)
                    Transform.Position += new Vector2(-swapDist, 0f);
                else if (hitState.normal.Y < 0 && yLeft)
                    Transform.Position += new Vector2(swapDist, 0f);
                else if (hitState.normal.Y < 0 && yRight)
                    Transform.Position += new Vector2(-swapDist, 0f);
                else if (hitState.normal.X > 0 && xTop)
                    Transform.Position += new Vector2(0f, swapDist);
                else if (hitState.normal.X > 0 && xBottom)
                    Transform.Position += new Vector2(0f, -swapDist);
                else if (hitState.normal.X < 0 && xTop)
                    Transform.Position += new Vector2(0f, swapDist);
                else if (hitState.normal.X < 0 && xBottom)
                    Transform.Position += new Vector2(0f, -swapDist);

            }
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.Owner is IPowerup)
            {
                powerup = other.Owner as IPowerup;
                powerup.ApplyPowerUp(this);
            }

            if (other.Owner is AI && !Invulnerability)
            {
                foreach (Component item in Components)
                {
                    item.Enabled = false;
                }

                this.Active = false;

                Console.WriteLine("Collided With AI");
            }

        }

        private void EnableAnimation(AnimationType animationType, bool enable)
        {
            KeyValuePair<AnimationType, AnimationRenderer> first = playerAnimations.Single(a => a.Key == animationType);
            first.Value.Show = enable; first.Value.Stop = !enable;

            IEnumerable<KeyValuePair<AnimationType, AnimationRenderer>> second = playerAnimations.Where(a => a.Key != animationType);
            second.ToList().ForEach(a =>
            {
                a.Value.Show = !enable;
                a.Value.Stop = !enable;
            });
        }

        private class StateDrop : IState
        {
            private Bomberman owner { get; set; }

            private Timer timer { get; set; }

            private bool firstTimeSpawn { get; set; } = true;

            public StateDrop(GameObject owner)
            {
                this.owner = owner as Bomberman;
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
                    Bomb bomb = GlobalFactory<Bomb>.Get(typeof(Bomb));
                    bomb.Active = true;
                    bomb.GetComponent<AnimationRenderer>().Enabled = true;
                    bomb.Transform.Position = new Vector2((int)(owner.Transform.Position.X + 0.5f), (int)(owner.Transform.Position.Y + 0.5f));

                    //AudioManager.PlayClip(AudioType.SOUND_DROP);

                    if (owner.CurrentExplosion == 0)
                        bomb.IsBig = false;
                    else
                        bomb.IsBig = true;


                    if (firstTimeSpawn)
                    {
                        GameObject.Spawn(bomb);
                        Console.WriteLine("first spawn time of bomb");
                        firstTimeSpawn = false;
                    }

                    timer.Start(true);
                }

                //Maybe no restart
                if (timer.IsActive)
                    timer.Update(true);

                return this;
            }
        }

        private class UpdateAnimation : Component, IUpdatable
        {
            private Bomberman bomberman;

            public UpdateAnimation(Bomberman toUpdate)
            {
                this.bomberman = toUpdate;
            }
            public void Update()
            {
                if (Input.IsKeyPressed(KeyCode.S))
                    bomberman.EnableAnimation(AnimationType.WALK_DOWN, true);
                else if (Input.IsKeyPressed(KeyCode.W))
                    bomberman.EnableAnimation(AnimationType.WALK_UP, true);
                else if (Input.IsKeyPressed(KeyCode.D))
                    bomberman.EnableAnimation(AnimationType.WALK_RIGHT, true);
                else if (Input.IsKeyPressed(KeyCode.A))
                    bomberman.EnableAnimation(AnimationType.WALK_LEFT, true);
                else
                    bomberman.EnableAnimation(AnimationType.IDLE, true);
            }
        }
    }
}
