﻿using BehaviourEngine;
using BehaviourEngine.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using BehaviourEngine.Interfaces;
using Aiv.Fast2D.Utils.Input;

namespace BomberMan2D.Prefabs
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

            menuTexts.ToList().ForEach(t => t.RenderOffset = (int)RenderLayer.Gui);
            menuTexts.ToList().ForEach(t => AddComponent(t));

            AddComponent(new UpdateMenu(menuTexts));

      
        }
    }

    class UpdateMenu : Component, IUpdatable
    {
        int MenuItem = 0;
        private List<Text2D> menuTexts;

        public UpdateMenu(List<Text2D> menuTexts)
        {
            this.menuTexts = menuTexts;
        }
        //public IState ChangeScene( IState NextState, IState currentState)
        //{
        //    if (Input.IsKeyDown(Aiv.Fast2D.KeyCode.Space))
        //    {
        //        currentState.OnStateExit();
        //        currentState = NextState.OnStateUpdate();
        //        currentState.OnStateEnter();
        //        return currentState;
        //    }
        //    else
        //        return currentState;

        //}

        public void Update()
        {
            if (MenuItem == 0)
            {
                menuTexts[1].message = "Multi-Player";
                menuTexts[MenuItem].message = "Single-Player <";
            }
            else if (MenuItem == 1)
            {
                menuTexts[0].message = "Single-Player";
                menuTexts[MenuItem].message = "Multi-Player <";
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
