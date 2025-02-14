using System;
using System.Collections.Generic;

namespace Project.Scripts.DesignPattern.FSM
{
    public class StateMachine: IStateMachine
    {
        public Dictionary<Type, State> States { get; } = new();
        public State CurrentState { get; private set; }
        
        public void Initialize(State initState)
        {
            CurrentState = initState;
            initState.Enter();
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        public void AddState(Type type, State newState) =>
            States.Add(type, newState);
    }
}