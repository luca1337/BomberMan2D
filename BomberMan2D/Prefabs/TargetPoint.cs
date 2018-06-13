using BehaviourEngine;
using System.Linq;
using OpenTK;

namespace BomberMan2D
{
    public class TargetPoint : GameObject, IWaypoint
    {
        public Vector2 Location { get => this.Transform.Position; set => this.Transform.Position = value; }
        SpriteRenderer sr;

        public TargetPoint() : base("TargetPoint")
        {
            int n = Map.GetPowerupSpawnPoint().Count;
            Location = Map.GetPowerupSpawnPoint()[RandomManager.Instance.Random.Next(0, n)];
            sr = AddComponent<SpriteRenderer>(new SpriteRenderer(FlyWeight.Get("Box2D")));
        }
    }

    public class TargetSpawner : GameObject
    {
        public TargetSpawner(int size, float shuffleTimeStep) : base("TargetSpawner") => AddComponent(new TargetPointComponent(size, shuffleTimeStep));
    }

    public class TargetPointComponent : Component, IUpdatable
    {
        private float tMin;
        private float tMax;
        public TargetPoint current;

        public TargetPointComponent(int numOfTargets, float shuffleTimeStep)
        {
            tMax = shuffleTimeStep;

            for (int i = 0; i < numOfTargets; i++)
            {
                current = new TargetPoint();
                GameManager.AddTargetPoint(current);
                GameObject.Spawn(current);
            }
        }

        public void Update()
        {
            if (!Enabled) return;

            tMin += Time.DeltaTime;
            if (tMin > tMax)
            {
                GameManager.GetAllPoints().Where(x => x.GetType() != typeof(Bomberman)).ToList().ForEach(item => (item as GameObject).GetComponent<Transform>().Position = Map.GetPowerupSpawnPoint()[RandomManager.Instance.Random.Next(0, Map.GetPowerupSpawnPoint().Count)]);
                ResetTiming();
            }
        }

        private void ResetTiming()
        {
            this.tMin = 0.0f;
        }
    }
}
