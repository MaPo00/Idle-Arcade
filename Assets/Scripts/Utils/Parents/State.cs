using UnityEngine;

namespace Utils.Parents
{
    public abstract class State<TState> : MonoBehaviour
    {
        private TState _state;

        public TState GetState() => _state;

        public void SetState(TState state)
        {
            _state = state;
            OnStateChanged(state);
        }

        protected virtual void OnStateChanged(TState state)
        {
        }
    }
}