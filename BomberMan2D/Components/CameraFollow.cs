using Aiv.Fast2D;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D
{
    public class CameraFollow : Component, IUpdatable
    {
        private Camera cam;
        private int blockSize = 1;

        public CameraFollow() : base()
        {
            cam = new Camera();

            float halfWidth = BehaviourEngine.Graphics.Instance.Window.OrthoWidth / 2;
            float halfHeight = BehaviourEngine.Graphics.Instance.Window.OrthoHeight / 2;
            cam.pivot = new Vector2(halfWidth, halfHeight);
            cam.position += cam.pivot;

            SetCamera(cam);
        }

        public bool IsStarted { get; set; }

        public void Update()
        {
            if (BehaviourEngine.Graphics.Instance.Window.CurrentCamera != cam)
                SetCamera(cam);

            if (Owner.Transform.Position.X > BehaviourEngine.Graphics.Instance.Window.OrthoWidth / 2
                && Owner.Transform.Position.X < BehaviourEngine.Graphics.Instance.Window.OrthoWidth / 2 + (5.25 * blockSize))
            {
                cam.position.X = Owner.Transform.Position.X;
            }
        }

        internal void SetCamera(Camera camera)
        {
            BehaviourEngine.Graphics.Instance.Window.SetCamera(camera);
        }
    }
}
