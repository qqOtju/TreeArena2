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
            _decorators.Add(typeof(WispDecoratorInterestingMovement), CreateInterestingMovementDecorator);
            _decorators.Add(typeof(WispDecoratorBulletsOnTargetDeath), CreateBulletsOnHealthHitDecorator);
            _decorators.Add(typeof(WispDecoratorReAim), CreateReAimDecorator);
            _decorators.Add(typeof(WispDecoratorMelee), CreateMeleeDecorator);
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
        
        private WispDecorator CreateInterestingMovementDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            return new WispDecoratorInterestingMovement(wispDecorator, wisp.BulletFactory);
        }
        
        private WispDecorator CreateBulletsOnHealthHitDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            return new WispDecoratorBulletsOnTargetDeath(wispDecorator, wisp.BulletFactory);
        }
        
        private WispDecorator CreateReAimDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            return new WispDecoratorReAim(wispDecorator, wisp.BulletFactory);
        }
        
        private WispDecorator CreateMeleeDecorator(IWisp wisp, WispDecorator wispDecorator)
        {
            return new WispDecoratorMelee(wispDecorator, wisp.BulletFactory);
        }
        
        public WispDecorator CreateDecorator<T>(IWisp wisp, WispDecorator wispDecorator) where T: WispDecorator
        {
            if (_decorators.TryGetValue(typeof(T), out var decorator))
                return decorator(wisp, wispDecorator);
            return null;
        }
    }
}