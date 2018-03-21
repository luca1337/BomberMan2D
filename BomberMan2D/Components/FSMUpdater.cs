using BehaviourEngine;
using BehaviourEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMan2D.Components
{
    public class FSMUpdater : Component, IUpdatable
    {
        private IState currentState;

        public FSMUpdater(IState currentState)
        {
            this.currentState = currentState;
        }

        public void Update()
        {
            currentState = currentState.OnStateUpdate();
        }
    }
}
