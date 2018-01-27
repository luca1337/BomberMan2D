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
        private delegate void AnimationHandler();

        public FSMUpdater(List<IState> states)
        {
            this.states = states;
        }

        public void UpdateState(IState state) => states.Add(state);

        public void Update()
        {
            states.ForEach(item => item.OnStateUpdate());
        }
    }
}
