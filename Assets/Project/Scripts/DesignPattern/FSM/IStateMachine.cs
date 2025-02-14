using System;
using System.Collections.Generic;

namespace Project.Scripts.DesignPattern.FSM
{
    public interface IStateMachine
    {
        public Dictionary<Type, State> States { get; }
        public State CurrentState { get; }
        public void Initialize(State initState);
        public void ChangeState(State newState);
        public void AddState(Type type, State newState);
    }
}