using Aiv.Fast2D;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using BomberMan2D.Prefabs;
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
            if (Owner.Transform.Position.X > BehaviourEngine.Graphics.Instance.Window.OrthoWidth / 2
                && Owner.Transform.Position.X < BehaviourEngine.Graphics.Instance.Window.OrthoWidth / 2 + (6 * blockSize))
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
