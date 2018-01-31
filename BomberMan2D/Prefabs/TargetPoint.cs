using BehaviourEngine;
using BehaviourEngine.Interfaces;
using System.Linq;
using OpenTK;

namespace BomberMan2D.Prefabs
{
    public class TargetPoint : GameObject, IWaypoint
    {
        public Vector2 Location { get => this.Transform.Position; set => this.Transform.Position = value; }

        public TargetPoint() : base("TargetPoint")
        {
            Location = Map.GetPowerupSpawnPoint()[RandomManager.Instance.Random.Next(0, Map.GetPowerupSpawnPoint().Count)];
        }
    }

    public class TargetSpawner : GameObject
    {
        public TargetSpawner(int size, float shuffleTimeStep) : base("TargetSpawner") => AddComponent(new TargetPointBehaviour(size, shuffleTimeStep));
    }

    public class TargetPointBehaviour : Component, IUpdatable
    {
        private float tMin;
        private float tMax;
        public TargetPoint current;

        public TargetPointBehaviour(int size, float shuffleTimeStep)
        {
            tMax = shuffleTimeStep;

            for (int i = 0; i < size; i++)
            {
                current = new TargetPoint();
                Game.AddTargetPoint(current);
                GameObject.Spawn(current);
            }
        }

        public void Update()
        {
            if (!Enabled) return;

            tMin += Time.DeltaTime;
            if (tMin > tMax)
            {
                Game.GetAllPoints().Where( x => x.GetType() != typeof(Bomberman)).ToList().ForEach(item => (item as GameObject).GetComponent<Transform>().Position = Map.GetPowerupSpawnPoint()[RandomManager.Instance.Random.Next(0, Map.GetPowerupSpawnPoint().Count)]);
                ResetTiming();
            }
        }

        private void ResetTiming()
        {
            this.tMin = 0.0f;
        }
    }
}
