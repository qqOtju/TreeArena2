﻿namespace Project.Scripts.DesignPattern.FSM
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();
        public abstract void OnDestroy();
    }
}