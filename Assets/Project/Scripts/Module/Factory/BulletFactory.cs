using System;
using Project.Scripts.DesignPattern.Factory;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory
{

    public class BulletFactory: ObjectFactory<Bullet>
    {
        private const int BasePoolSize = 10;
        
        private readonly DiContainer _diContainer;

        private BulletActionsArgs _args;
        private Transform _muzzle;

        public BulletFactory(Bullet prefab, Transform container, 
            DiContainer diContainer, Transform muzzle) : base(prefab, container)
        {
            Pool.Initialize(BasePoolSize);
            _muzzle = muzzle;
            _diContainer = diContainer;
        }

        public void SetActions(BulletActionsArgs args)
        {
            _args = args;
        }

        public override Bullet Create()
        {
            var bullet = Pool.Get();
            bullet.transform.position = _muzzle.position;
            bullet.Init(_args);
            _diContainer.Inject(bullet);
            return bullet;
        }

        public override void Release(Bullet obj)
        {
            Pool.Release(obj);
        }
    }
}