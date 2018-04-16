using OpenTK;
using BehaviourEngine;
using Aiv.Fast2D;


namespace BomberMan2D
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
