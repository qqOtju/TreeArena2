using System;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    public interface IWisp
    {
        public Transform BulletSpawnPoint { get; }
        public BulletFactory BulletFactory { get; }
        public void AddDecorator(Type wispDecoratorType);
    }
}