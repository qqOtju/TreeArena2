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
    public class WispMagazine: WispBase
    {
        [SerializeField] private Bullet _bulletPrefab;

        private const int MagazineSize = 10;
        private const float ReloadTime = 2f;

        private WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _wispDecorator;
        private DiContainer _diContainer;
        private WispStats _wispStats;
        private float _attackTimer;
        private int _currentMagazineSize;
        private float _reloadTimer;
        private bool _isReloading;

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
            _currentMagazineSize = MagazineSize;
        }
        
        protected override void Update()
        {
            base.Update();
            if(!_isReloading)
                Attack();
            else
                Reload();
        }

        private void Attack()
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _wispStats.AttackSpeed)
            {
                _currentMagazineSize--;
                DebugSystem.Instance.Log(LogType.Wisp, 
                    $"Current magazine size: <color=blue>{_currentMagazineSize}</color>");
                _wispDecorator.Shoot();
                _attackTimer = 0;
                if (_currentMagazineSize <= 0)
                {
                    DebugSystem.Instance.Log(LogType.Wisp, 
                        "<color=red>Reloading...</color>");    
                    _isReloading = true;
                }
            }
        }

        private void Reload()
        {
            _reloadTimer += Time.deltaTime;
            if (_reloadTimer >= ReloadTime)
            {
                DebugSystem.Instance.Log(LogType.Wisp, 
                    "<color=green>Reloaded!</color>");
                _currentMagazineSize = MagazineSize;
                _isReloading = false;
                _reloadTimer = 0;
            }
        }
        
        public override void AddDecorator<T>()
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator set!");
            _wispDecorator = _wispDecoratorFactory.CreateDecorator<T>(this, _wispDecorator);
        }
    }
}