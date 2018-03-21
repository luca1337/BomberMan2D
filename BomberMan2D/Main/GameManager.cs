using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Components;
using BomberMan2D.Prefabs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BomberMan2D.Main
{
    public class GameManager : GameObject
    {
        private IState currentState;

        private static List<IWaypoint> targetPoints = new List<IWaypoint>();

        internal static List<IWaypoint> GetAllPoints()
        {
            return targetPoints;
        }

        internal static void AddTargetPoint(IWaypoint current)
        {
            targetPoints.Add(current);
        }

        internal static int GetPointsCount()
        {
            return targetPoints.Count();
        }

        public GameManager() : base("GameManager")
        {
            SetupState setupState = new SetupState(this);
            MenuState menuState = new MenuState(this);
            LoopState loopState = new LoopState(this);
            PauseState pauseState = new PauseState(this);
            LoseState loseState = new LoseState(this);
            WinState winState = new WinState(this);

            //Link State
            setupState.MenuState   = menuState;
            menuState.Loop    = loopState;
            loopState.NextWin = winState;
            loopState.NextLose = loseState;
            loopState.NextPause = pauseState;

            setupState.OnStateEnter();
            currentState = setupState;

            AddComponent(new FSMUpdater(currentState));
        }

        private static void SetupCollisionsRulesAndPhysics()
        {
            //do we want physics?
            Physics.Instance.Gravity *= 2f;

            LayerManager.AddLayer((uint)CollisionLayer.BomberMan, (uint)CollisionLayer.SolidWall | (uint)CollisionLayer.Powerup);
            LayerManager.AddLayer((uint)CollisionLayer.Explosion, (uint)CollisionLayer.SolidWall);
        }



        private static void SetupTextures()
        {
            FlyWeight.Add("Font01", "Assets/Font.dat");
            FlyWeight.Add("Wall", "Assets/Wall_01.dat");
            FlyWeight.Add("Obstacle", "Assets/Obstacle_01.dat");
            FlyWeight.Add("BomberMan", "Assets/Bombertab1.dat");
            FlyWeight.Add("Speed", "Assets/Speed.dat");
            FlyWeight.Add("Health", "Assets/Health.dat");
            FlyWeight.Add("Bomb", "Assets/Bomb.dat");
            FlyWeight.Add("Explosion", "Assets/Explosion.dat");
            FlyWeight.Add("AI", "Assets/ballon.dat");
            FlyWeight.Add("Bomb_PW", "Assets/BombsPw.dat");
            FlyWeight.Add("Bombpass_PW", "Assets/BombpassPw.dat");
            FlyWeight.Add("Flamepass_PW", "Assets/FlamepassPw.dat");
            FlyWeight.Add("Mystery_PW", "Assets/MysteryPw.dat");
            FlyWeight.Add("Detonator_PW", "Assets/DetonatorPw.dat");
            FlyWeight.Add("Wallpass_PW", "Assets/WallpassPw.dat");
            FlyWeight.Add("Speed_PW", "Assets/SpeedPw.dat");
            FlyWeight.Add("Flame_PW", "Assets/FlamesPw.dat");
            FlyWeight.Add("MainScreen", "Assets/mainscreen.dat");
        }

        private static void SetupObjectPools()
        {
            Pool<Bomb>.Register(() => new Bomb(), 100);
            Pool<Explosion>.Register(() => new Explosion(), 100);
            Pool<AI>.Register(() => new AI(), 100);

            //powerups
            //Pool<PowerUp>.Register(() => new PowerUp(), 50);

        }

        private static void SetupSounds()
        {
            AudioManager.AddSource(AudioType.SOUND_EXPLOSION);
            AudioManager.AddClip("Sounds/Explosion.ogg", AudioType.SOUND_EXPLOSION);

            AudioManager.AddSource(AudioType.SOUND_DROP);
            AudioManager.AddClip("Sounds/Drop.ogg", AudioType.SOUND_DROP);

            AudioManager.AddSource(AudioType.SOUND_WALK_FAST);
            AudioManager.AddClip("Sounds/StepFast.ogg", AudioType.SOUND_WALK_FAST);

            AudioManager.AddSource(AudioType.SOUND_WALK_SLOW);
            AudioManager.AddClip("Sounds/StepSlow.ogg", AudioType.SOUND_WALK_SLOW);

            AudioManager.AddSource(AudioType.SOUND_PICKUP);
            AudioManager.AddClip("Sounds/Powerup.ogg", AudioType.SOUND_PICKUP);

            AudioManager.AddSource(AudioType.SOUND_DIE);
            AudioManager.AddClip("Sounds/Dead.ogg", AudioType.SOUND_DIE);

            AudioManager.AddSource(AudioType.SOUND_BACKGROUND);
            AudioManager.AddClip("Sounds/03_Stage_Theme.ogg", AudioType.SOUND_BACKGROUND);
        }



        private class SetupState : IState
        {
            public MenuState MenuState { get; set; }
          
            private GameManager owner { get; set; }

            public SetupState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                GameManager.SetupCollisionsRulesAndPhysics();
                GameManager.SetupTextures();
                GameManager.SetupObjectPools();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                MenuState.OnStateEnter();
                return MenuState;
                //  Node.ShowPath();
            }
        }

        private class MenuState : IState
        {
            public LoopState Loop { get; set; }

            private GameManager owner { get; set; }
            private Menu mainMenu;
            private MenuBackground menuBg;

            public MenuState(GameObject owner)
            {
                this.owner = owner as GameManager;

            }

            public void OnStateEnter()
            {
                if (mainMenu == null)
                {
                    mainMenu = new Menu();
                    GameManager.Spawn(mainMenu);
                }
                else
                    mainMenu.Active = true;

                if (menuBg == null)
                {
                    menuBg = new MenuBackground();
                    GameObject.Spawn(menuBg);
                }
                else
                    menuBg.Active = true;


            }

            public void OnStateExit()
            {
                menuBg.Active = false;
                mainMenu.Active = false;
            }

            public IState OnStateUpdate()
            {
                if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.Space))
                {
                    this.OnStateExit();
                    Loop.OnStateEnter();
                    return Loop;
                }

                return this;
            }
        }


        private class LoopState : IState
        {
            public WinState NextWin { get; set; }
            public LoseState NextLose { get; set; }
            public PauseState NextPause { get; set; }

            private GameManager owner { get; set; }

            public LoopState(GameObject owner)
            {
                this.owner = owner as GameManager;
            }

            public void OnStateEnter()
            {
                LoadLevels();
                LoadGameObjects();
            }

            public void OnStateExit()
            {
            }

            public IState OnStateUpdate()
            {
                return this;
            }

            private static void LoadLevels()
            {
                GameObject.Spawn(new Map("Levels/Level00.csv"));
            }

            private static void LoadGameObjects()
            {
                //OSD
                GameObject.Spawn(new OnScreenDisplay());

                //Player
                Bomberman bomberMan = new Bomberman();
                GameObject.Spawn(bomberMan, Map.GetPlayerSpawnPoint());

                ////TargetPoints
                GameObject.Spawn(new TargetSpawner(5, 3.5f));

                //AI
                GameObject.Spawn(new EnemySpawner(bomberMan));
            }
        }

        private class WinState : IState
        {
            private GameManager owner { get; set; }

            public WinState(GameObject owner)
            {
                this.owner = owner as GameManager;
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
                throw new NotImplementedException();
            }
        }

        private class LoseState : IState
        {
            private GameManager owner { get; set; }

            public LoseState(GameObject owner)
            {
                this.owner = owner as GameManager;
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
                throw new NotImplementedException();
            }
        }

        private class PauseState : IState
        {
            public LoopState Next { get; set; }

            private GameManager owner { get; set; }

            public PauseState(GameObject owner)
            {
                this.owner = owner as GameManager;
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
                throw new NotImplementedException();
            }
        }
    }
}