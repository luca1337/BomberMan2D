using BehaviourEngine;

namespace BomberMan2D
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
