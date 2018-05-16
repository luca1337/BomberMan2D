using BehaviourEngine;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Aiv.Fast2D.Utils.Input;

namespace BomberMan2D
{
    public class Menu : GameObject
    {
        List<Text2D> menuTexts = new List<Text2D>();

        public Menu() : base("Main Menu")
        {
            menuTexts.Add(new Text2D("Font01", "", new Vector2(Graphics.Instance.Window.OrthoWidth / 2 - 4.5f,
                Graphics.Instance.Window.OrthoHeight / 2 + 3), new Vector4(0f, 0f, 0f, 0f), 0.7f));

            menuTexts.Add(new Text2D("Font01", "", new Vector2(Graphics.Instance.Window.OrthoWidth / 2 - 4.5f,
               Graphics.Instance.Window.OrthoHeight / 2 + 4), new Vector4(0f, 0f, 0f, 0f), 0.7f));

            menuTexts.ToList().ForEach(t => t.RenderOffset = (int)RenderLayer.Gui_00);
            menuTexts.ToList().ForEach(t => AddComponent(t));

            AddComponent(new UpdateMenu(menuTexts));

      
        }
    }

    class UpdateMenu : Component, IUpdatable
    {
        public bool SinglePlayerMode { get; set; }

        private int MenuItem = 0;
        private List<Text2D> menuTexts;


        public UpdateMenu(List<Text2D> menuTexts)
        {
            this.menuTexts = menuTexts;
        }

        public void Update()
        {
            if (MenuItem == 0)
            {
                menuTexts[1].message = "Multi-Player";
                menuTexts[MenuItem].message = "Single-Player <";
                SinglePlayerMode = true;
            }
            else if (MenuItem == 1)
            {
                menuTexts[0].message        = "Single-Player";
                menuTexts[MenuItem].message = "Multi-Player <";
                SinglePlayerMode            = false;
            }

            if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.Down))
                MenuItem++;

            if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.Up))
                MenuItem--;

            //reset menu items indexes if out of bounds
            if (MenuItem < 0)
                MenuItem = 1;

            if (MenuItem > 1)
                MenuItem = 0;
        }
    }
}
