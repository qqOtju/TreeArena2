using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using Project.Scripts.Module.Stats.Wisp;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    public class WispStandard: WispBase
    {
        [SerializeField] private Bullet _bulletPrefab;

        private WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _wispDecorator;
        private DiContainer _diContainer;
        private WispStats _wispStats;
        private float _attackTimer;

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
        
        protected override void Update()
        {
            base.Update();
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= _wispStats.AttackSpeed)
            {
                _wispDecorator.Shoot();
                _attackTimer = 0;
            }
        }
        
        public override void AddDecorator<T>()
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator added!");
            _wispDecorator = _wispDecoratorFactory.CreateDecorator<T>(this, _wispDecorator);
        }
    }
}