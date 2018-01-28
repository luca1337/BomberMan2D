using Aiv.Fast2D;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Components
{
    public class CameraFollow : Component, IUpdatable
    {
        private Camera cam;
        private int blockSize = 50;

        public CameraFollow() : base()
        {
            cam = new Camera();

            float halfWidth = BehaviourEngine.Graphics.Instance.Window.Width / 2;
            float halfHeight = BehaviourEngine.Graphics.Instance.Window.Height / 2;
            cam.pivot = new Vector2(halfWidth, halfHeight);
            cam.position += cam.pivot;

            SetCamera(cam);
        }

        public bool IsStarted { get; set; }

        public void Update()
        {
            if(Owner.Transform.Position.X > BehaviourEngine.Graphics.Instance.Window.Width / 2 - Owner.Transform.Scale.X
                && Owner.Transform.Position.X < BehaviourEngine.Graphics.Instance.Window.Width / 2 + (6 * blockSize))
            cam.position.X = Owner.Transform.Position.X;
        }

        private void SetCamera(Camera camera)
        {
            BehaviourEngine.Graphics.Instance.Window.SetCamera(camera);
        }
    }
}
