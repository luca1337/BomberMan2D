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
    public class Bomberman : GameObject, IWaypoint
    {
        public Vector2 Location { get; set; }

        //Animations
        private Dictionary<AnimationType, AnimationRenderer> playerAnimations = new Dictionary<AnimationType, AnimationRenderer>();

        //Bomb drop
        private StateDrop drop;

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

        private int littleExp = 0;
        private int bigExp    = 1;

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
            playerAnimations.ToList().ForEach(item => item.Value.RenderOffset = (int)RenderLayer.BomberMan);
            #endregion


            //Bomb fsm
            drop = new StateDrop(this);
            drop.OnStateEnter();
            AddComponent(new Components.FSMUpdater(drop));

            #region Components

            AddComponent(new Components.CharacterController());
            AddComponent(new UpdateAnimation(this));
          
            //Collider
            BoxCollider2D collider2D = new BoxCollider2D(new Vector2(0.5f, 0.5f));
            collider2D.CollisionMode = CollisionMode.Collision;
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
            if (other.Owner is IPowerup)
            {
                powerup = other.Owner as IPowerup;
                powerup.ApplyPowerUp(this);
            }

            if (other.Owner is AI)
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
                    Bomb bomb = Pool<Bomb>.GetInstance(x =>
                    {
                        x.Active = true;
                        x.GetComponent<AnimationRenderer>().Enabled = true;
                        // Maybe this is the way for spawn in the middle of the cell
                        x.Transform.Position = new Vector2((int)(owner.Transform.Position.X + 0.5f), (int)(owner.Transform.Position.Y + 0.5f));
                    });
                    //AudioManager.PlayClip(AudioType.SOUND_DROP);
                    if (owner.CurrentExplosion == 0)
                        bomb.IsBig = false;
                    else
                        bomb.IsBig = true;
              

                    if(firstTimeSpawn)
                    {
                        GameObject.Spawn(bomb);
                        Console.WriteLine("first spawn time of bomb");
                        firstTimeSpawn = false;
                    }

                    timer.Start();
                }

                if (timer.IsActive)
                    timer.Update();

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
                {
                    bomberman.EnableAnimation(AnimationType.WALK_UP,    false);
                    bomberman.EnableAnimation(AnimationType.WALK_RIGHT, false);
                    bomberman.EnableAnimation(AnimationType.WALK_LEFT,  false);
                    bomberman.EnableAnimation(AnimationType.IDLE, false);

                    bomberman.EnableAnimation(AnimationType.WALK_DOWN, true);
                }
                else if (Input.IsKeyPressed(KeyCode.W))
                {
                    bomberman.EnableAnimation(AnimationType.WALK_DOWN, false);
                    bomberman.EnableAnimation(AnimationType.WALK_RIGHT, false);
                    bomberman.EnableAnimation(AnimationType.WALK_LEFT, false);
                    bomberman.EnableAnimation(AnimationType.IDLE, false);

                    bomberman.EnableAnimation(AnimationType.WALK_UP, true);
                }
                else if (Input.IsKeyPressed(KeyCode.D))
                {
                    bomberman.EnableAnimation(AnimationType.WALK_DOWN, false);
                    bomberman.EnableAnimation(AnimationType.WALK_UP, false);
                    bomberman.EnableAnimation(AnimationType.WALK_LEFT, false);
                    bomberman.EnableAnimation(AnimationType.IDLE, false);

                    bomberman.EnableAnimation(AnimationType.WALK_RIGHT, true);
                }
                else if (Input.IsKeyPressed(KeyCode.A))
                {
                    bomberman.EnableAnimation(AnimationType.WALK_DOWN, false);
                    bomberman.EnableAnimation(AnimationType.WALK_UP, false);
                    bomberman.EnableAnimation(AnimationType.WALK_RIGHT, false);
                    bomberman.EnableAnimation(AnimationType.IDLE, false);

                    bomberman.EnableAnimation(AnimationType.WALK_LEFT, true);
                }
                else
                {
                    bomberman.EnableAnimation(AnimationType.WALK_DOWN, false);
                    bomberman.EnableAnimation(AnimationType.WALK_UP, false);
                    bomberman.EnableAnimation(AnimationType.WALK_RIGHT, false);
                    bomberman.EnableAnimation(AnimationType.WALK_LEFT, false);

                    bomberman.EnableAnimation(AnimationType.IDLE, true);
                }
            }
        }
    }
}
