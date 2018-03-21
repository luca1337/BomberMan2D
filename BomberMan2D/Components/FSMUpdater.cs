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
        private List<IState> states = new List<IState>();
        private IState currentState;

        public FSMUpdater(List<IState> states, IState currentState)
        {
            this.states = states;
            this.currentState = currentState;
        }

        public void Update()
        {
            currentState = currentState.OnStateUpdate();
        }
    }
}
