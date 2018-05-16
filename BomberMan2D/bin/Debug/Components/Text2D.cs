using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace BehaviourEngine
{
    public class Text2D : Component , IUpdatable, IDrawable
    {
        public string message;
        public int RenderOffset { get; set; }

        private TextMesh text;

        public Text2D(string spriteSheetText, string message, Vector2 position, Vector4 color, float horizontalSpacing, Camera camera = null) : base()
        {
            RenderOffset = (int)RenderLayer.Gui_00;
            text          = new TextMesh(FlyWeight.Get(spriteSheetText));
            text.SetTextColor(color);
            text.SetHorizontalSpacing(horizontalSpacing);
            text.position = position;
            text.Camera = camera;
            this.message  = message;
        }

        public void SetTextColor(Vector4 newColor)
        {
            text.SetTextColor(newColor);
        }
  
        public virtual void Draw()
        {
            text.DrawText();
        }
    
        public void Update()
        {
            text.UpdateText(message);
        }
    }
}
