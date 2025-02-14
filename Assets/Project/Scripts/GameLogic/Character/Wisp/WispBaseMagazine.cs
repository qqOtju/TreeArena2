using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    public class WispBaseMagazine: WispBase
    {
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletContainer;

        private const int MagazineSize = 10;
        private const float ReloadTime = 2f;

        private WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _wispDecorator;
        private DiContainer _diContainer;
        private WispStats _wispStats;
        private Camera _mainCamera;
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
        
        private void Start()
        {
            _mainCamera = Camera.main;
            BulletFactory = new BulletFactory(_bulletPrefab, _bulletContainer, _diContainer, _bulletSpawnPoint);
            BulletSpawnPoint = _bulletSpawnPoint;
            _wispDecorator = new WispDecoratorStandard(new WispStandardComponent(BulletFactory, _bulletSpawnPoint, _wispStats), BulletFactory, _bulletSpawnPoint);
            _currentMagazineSize = MagazineSize;
        }
        
        private void Update()
        {
            if(!_isReloading)
                Attack();
            else
                Reload();
            if (Input.GetMouseButtonDown(0))
                RotateBulletSpawnPoint();
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

        private void RotateBulletSpawnPoint()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - _bulletSpawnPoint.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _bulletSpawnPoint.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }
        
        public override void AddDecorator<T>()
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator set!");
            _wispDecorator = _wispDecoratorFactory.CreateDecorator<T>(this, _wispDecorator);
        }
    }
}