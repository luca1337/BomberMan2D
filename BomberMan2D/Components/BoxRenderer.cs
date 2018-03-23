using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using Aiv.Fast2D;


namespace BomberMan2D.Components
{
    public class BoxRenderer : Component, IDrawable
    {
        public int RenderOffset { get; set; }

        private Sprite box;
        private Vector4 color;

        public BoxRenderer(float width, float height, Vector4 color) : base()
        {
            RenderOffset = (int)RenderLayer.Collider;
            box = new Sprite(width,height);
            this.color = color;
        }
        public void Draw()
        {
            box.DrawSolidColor(color);
        }
    }
}
