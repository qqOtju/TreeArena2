using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    public abstract class WispBase: MonoBehaviour, IWisp
    {
        public Transform BulletSpawnPoint { get; protected set; }
        public BulletFactory BulletFactory { get; protected set; }
        public abstract void AddDecorator<T>() where T : WispDecorator;
    }
}