using System;
using System.Collections.Generic;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.GameLogic.Character.Wisp;
using Project.Scripts.Module.Stats;
using Project.Scripts.Module.Stats.Wisp;
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
        private WispStats _wispStats;

        [Inject]
        private void Construct(DiContainer diContainer, WispStats wispStats)
        {
            _diContainer = diContainer;
            _wispStats = wispStats;
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
            _decorators.Add(typeof(WispDecoratorSniper), CreateSniperDecorator);
        }

        private WispDecorator CreateDoubleAttackDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorDoubleAttack(wispBase.BulletFactory, wispBase.BulletSpawnPoint, wispDecorator, _wispStats);
        }
        
        private WispDecorator CreateBackShotDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorBackShot(wispBase.BulletFactory, wispBase.BulletSpawnPoint, wispDecorator, _wispStats);
        }
        
        private WispDecorator CreateUniqueBulletDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            var uniqueFactory = new BulletFactory(_uniqueBullet, _uniqueBulletContainer, _diContainer, wispBase.BulletSpawnPoint);
            return new WispDecoratorUniqueBullet(wispDecorator, wispBase.BulletFactory, wispBase.BulletSpawnPoint, uniqueFactory, _wispStats);
        }
        
        private WispDecorator CreateInterestingMovementDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorInterestingMovement(wispDecorator, wispBase.BulletFactory, _wispStats);
        }
        
        private WispDecorator CreateBulletsOnHealthHitDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorBulletsOnTargetDeath(wispDecorator, wispBase.BulletFactory, _wispStats);
        }
        
        private WispDecorator CreateReAimDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorReAim(wispDecorator, wispBase.BulletFactory, _wispStats);
        }
        
        private WispDecorator CreateMeleeDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorMelee(wispDecorator, wispBase.BulletFactory, _wispStats);
        }
        
        private WispDecorator CreateSniperDecorator(IWisp wispBase, WispDecorator wispDecorator)
        {
            return new WispDecoratorSniper(wispDecorator, wispBase.BulletFactory, _wispStats);
        }
        
        public WispDecorator CreateDecorator(Type wispDecoratorType, IWisp wispBase, WispDecorator wispDecorator)
        {
            if (!typeof(WispDecorator).IsAssignableFrom(wispDecoratorType))
            {
                UnityEngine.Debug.LogError($"{wispDecoratorType} не є WispDecorator");
                return null;
            }
            if (_decorators.TryGetValue(wispDecoratorType, out var decorator))
                return decorator(wispBase, wispDecorator);
            return null;
        }
    }
}