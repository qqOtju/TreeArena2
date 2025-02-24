using System;
using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats.Wisp;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class WispWalker: WispBase
    {
        [SerializeField] private Bullet _bulletPrefab;

        private const float DistanceToShoot = 1.25f;
        
        private WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _wispDecorator;
        private DiContainer _diContainer;
        private Vector3 _pastPosition;
        private WispStats _wispStats;
        private float _attackTimer;
        private float _currentDistance;

        [Inject]
        private void Construct(DiContainer diContainer, WispDecoratorFactory wispDecoratorFactory, WispStats wispStats)
        {
            _diContainer = diContainer;
            _wispDecoratorFactory = wispDecoratorFactory;
            _wispStats = wispStats;
        }
        
        protected override void Start()
        {
            base.Start();
            BulletFactory = new BulletFactory(_bulletPrefab, BulletContainer, _diContainer, _bulletSpawnPoint);
            _wispDecorator = new WispDecoratorStandard(new WispComponentStandard(BulletFactory, _bulletSpawnPoint, _wispStats), BulletFactory, _bulletSpawnPoint);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            AttackCycle();
        }

        private void AttackCycle()
        {
            var currentPos = transform.position;
            var distance = Vector3.Distance(currentPos, _pastPosition);
            _currentDistance += distance;
            _pastPosition = currentPos;
            //ToDo: make it depend on _wispStats attack speed
            if (_currentDistance >= DistanceToShoot)
            {
                _wispDecorator.Shoot();
                _currentDistance = 0;
            }
        }

        public override void AddDecorator(Type wispDecoratorType)
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator added!");
            _wispDecorator = _wispDecoratorFactory.CreateDecorator(wispDecoratorType, this, _wispDecorator);
        }
    }
}