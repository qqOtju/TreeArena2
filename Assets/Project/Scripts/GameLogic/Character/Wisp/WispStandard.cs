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
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletContainer;

        private WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _wispDecorator;
        private DiContainer _diContainer;
        private WispStats _wispStats;
        private Camera _mainCamera;
        private float _attackTimer;

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
            BulletSpawnPoint = _bulletSpawnPoint;
            BulletFactory = new BulletFactory(_bulletPrefab, _bulletContainer, _diContainer, _bulletSpawnPoint);
            _wispDecorator = new WispDecoratorStandard(new WispComponentStandard(BulletFactory, _bulletSpawnPoint, _wispStats), BulletFactory, _bulletSpawnPoint);
        }
        
        private void Update()
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= _wispStats.AttackSpeed)
            {
                _wispDecorator.Shoot();
                _attackTimer = 0;
            }
            if (Input.GetMouseButtonDown(0))
                RotateBulletSpawnPoint();
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
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator added!");
            _wispDecorator = _wispDecoratorFactory.CreateDecorator<T>(this, _wispDecorator);
        }
    }
}