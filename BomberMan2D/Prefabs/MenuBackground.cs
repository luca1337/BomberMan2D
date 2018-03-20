using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;

namespace BomberMan2D.Prefabs
{
    public class MenuBackground : GameObject
    {
        private SpriteRenderer image;

        public MenuBackground() : base("Menu Background")
        {
            image = new SpriteRenderer(FlyWeight.Get("MainScreen"));
            image.RenderOffset = (int)RenderLayer.Gui2;
            AddComponent(image);
            this.Transform.Scale = new Vector2(Graphics.Instance.Window.OrthoWidth, Graphics.Instance.Window.OrthoHeight);
        }
    }
}
