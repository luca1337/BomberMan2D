using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BehaviourEngine.Utils;
using BomberMan2D.Prefabs;
using OpenTK;

namespace BomberMan2D
{
    public class Game
    {
        private static List<IWaypoint> targetPoints = new List<IWaypoint>();

        public static void Init()
        {
            //Setup context
            Window window = new Window(1200, 700, "BomberMan");
            window.SetDefaultOrthographicSize(14);
            window.SetClearColor(0.0f, 0.61f, 0.0f);
            Engine.Init(window);

            //do we want physics?
            Physics.Instance.Gravity *= 2f;

            //Collision masks
            LayerManager.AddLayer((uint)CollisionLayer.BomberMan, (uint)CollisionLayer.Wall + (uint)CollisionLayer.Powerup);
            LayerManager.AddLayer((uint)CollisionLayer.Explosion, (uint)CollisionLayer.Wall);

            //Load texture and initialize object pools and sounds
            LoadTextures();
            //InitSound();
            ObjectPools();

            //Levels
            GameObject.Spawn(new Map("Levels/Level00.csv"));
            Bomberman bomberMan = new Bomberman();

            //Player
            GameObject.Spawn(bomberMan, Map.GetPlayerSpawnPoint());

            //AI
            GameObject.Spawn(new EnemySpawner(bomberMan));

            //TargetPoints
            GameObject.Spawn(new TargetSpawner(5, 3.5f));
        }

        internal static List<IWaypoint> GetAllPoints()
        {
            return targetPoints;
        }

        internal static void AddTargetPoint(IWaypoint current)
        {
            targetPoints.Add(current);
        }

        private static void LoadTextures()
        {
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
        }

        internal static int GetPointsCount()
        {
            return targetPoints.Count();
        }

        private static void ObjectPools()
        {
            Pool<PowerUp>.Register(() => new PowerUp(), 100);
            Pool<Bomb>.Register(() => new Bomb(), 100);
            Pool<Explosion>.Register(() => new Explosion(), 100);
            Pool<AI>.Register(() => new AI(), 100);
        }

        private static void InitSound()
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

        public static void Run()
        {
            Engine.Run();
        }
    }
}
