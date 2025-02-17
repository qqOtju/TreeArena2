using System;
using ModestTree.Util;
using Project.Scripts.DesignPattern.Factory;
using Project.Scripts.GameLogic.Character.Attack;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory
{

    public class BulletFactory: ObjectFactory<Bullet>
    {
        private const int BasePoolSize = 10;
        
        private readonly DiContainer _diContainer;
        private readonly Transform _muzzle;

        private Func<Bullet, Bullet> _configBulletFunc;
        private BulletActionsArgs _args;

        public BulletFactory(Bullet prefab, Transform container, 
            DiContainer diContainer, Transform muzzle) : base(prefab, container)
        {
            Pool.Initialize(BasePoolSize);
            _muzzle = muzzle;
            _diContainer = diContainer;
        }

        public override Bullet Get()
        {
            var bullet = Pool.Get();
            bullet.transform.position = _muzzle.position;
            bullet.Init(_args);
            _diContainer.Inject(bullet);
            _configBulletFunc?.Invoke(bullet);
            return bullet;
        }

        public override void Release(Bullet obj)
        {
            Pool.Release(obj);
        }

        public void SetActions(BulletActionsArgs args)
        {
            _args = args;
        }
        
        public void SetConfigBulletFunc(Func<Bullet, Bullet> func)
        {
            _configBulletFunc = func;
        }
    }
}