using System.Collections.Generic;
using System.Linq;
using Aiv.Fast2D;
using OpenTK;
using BehaviourEngine;

namespace BomberMan2D
{
    public class OnScreenDisplay : GameObject
    {
        public int Score;
        public List<Text2D> text;
        private BoxRenderer background;

        public OnScreenDisplay() : base("GUI")
        {
            Score = 0;
            text = new List<Text2D>
            {
                new Text2D("Font01", string.Format("Time {0}", 1), new Vector2(1, 0.5f), new Vector4(0f, 0f, 0f, 0f), 0.7f, new Camera()),
                new Text2D("Font01", Score.ToString(), new Vector2(BehaviourEngine.Graphics.Instance.Window.OrthoWidth / 2, 0.5f), new Vector4(0f, 0f, 0f, 0f), 0.7f, new Camera())
            };

            text.ToList().ForEach(x => AddComponent(x));

            background = new BoxRenderer(BehaviourEngine.Graphics.Instance.Window.Width, 2, new Vector4(0.7f, 0.7f, 0.7f, 1f));
            AddComponent(background);
        }
    }
}
