using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D
{
    public class MenuBackground : GameObject
    {
        private SpriteRenderer image;
        public MenuBackground(string nameTexture) : base("Menu Background")
        {
            image = new SpriteRenderer(FlyWeight.Get(nameTexture));
            image.RenderOffset = (int)RenderLayer.Gui_01;
            AddComponent(image);
            this.Transform.Scale = new Vector2(Graphics.Instance.Window.OrthoWidth, Graphics.Instance.Window.OrthoHeight);

            AddComponent(new SetPosition());
        }


        private class SetPosition : Component, IUpdatable
        {
            private Aiv.Fast2D.Camera camera;

            public SetPosition()
            {
                camera = new Aiv.Fast2D.Camera();

            }
            public void Update()
            {
                camera.position = new Vector2(Owner.Transform.Position.X, Owner.Transform.Position.Y);
                Graphics.Instance.Window.SetCamera(camera);

            }
        }
    }
}
