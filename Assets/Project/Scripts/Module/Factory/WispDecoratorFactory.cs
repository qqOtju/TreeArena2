using System;
using System.Collections.Generic;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.GameLogic.Character.Wisp;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Module.Factory
{
    public class WispDecoratorFactory: MonoBehaviour
    {
        [SerializeField] private Bullet _uniqueBullet;
        [SerializeField] private Transform _uniqueBulletContainer;
        
        private readonly Dictionary<Type, Func<IWisp, WispDecorator, WispDecorator>> _decorators = new();

        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        private void Awake()
        {
            _decorators.Add(typeof(WispDecoratorDoubleAttack), CreateDoubleAttackDecorator);
            _decorators.Add(typeof(WispDecoratorBackShot), CreateBackShotDecorator);
            _decorators.Add(typeof(WispDecoratorUniqueBullet), CreateUniqueBulletDecorator);
        }

        private WispDecorator CreateDoubleAttackDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            return new WispDecoratorDoubleAttack(wisp.BulletFactory, wisp.BulletSpawnPoint, wispDecorator);
        }
        
        private WispDecorator CreateBackShotDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            return new WispDecoratorBackShot(wisp.BulletFactory, wisp.BulletSpawnPoint, wispDecorator);
        }
        
        private WispDecorator CreateUniqueBulletDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            var uniqueFactory = new BulletFactory(_uniqueBullet, _uniqueBulletContainer, _diContainer, wisp.BulletSpawnPoint);
            return new WispDecoratorUniqueBullet(wispDecorator, wisp.BulletFactory, wisp.BulletSpawnPoint, uniqueFactory);
        }
        
        public WispDecorator CreateDecorator<T>(IWisp wisp, WispDecorator wispDecorator) where T: WispDecorator
        {
            if (_decorators.TryGetValue(typeof(T), out var decorator))
                return decorator(wisp, wispDecorator);
            return null;
        }
    }
}