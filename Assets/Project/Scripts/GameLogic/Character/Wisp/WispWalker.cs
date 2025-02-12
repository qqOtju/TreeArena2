using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Module.Factory;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class WispWalker: MonoBehaviour, IWisp
    {
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletContainer;

        private const float DistanceToShoot = 1.25f;
        
        private WispDecoratorFactory _wispDecoratorFactory;
        private BasicMovement _basicMovement;
        private WispDecorator _wispDecorator;
        private DiContainer _diContainer;
        private Vector3 _pastPosition;
        private Camera _mainCamera;
        private float _attackTimer;
        private float _currentDistance;
        private Rigidbody2D _rb;

        public Transform BulletSpawnPoint => _bulletSpawnPoint;
        public BulletFactory BulletFactory { get; private set; }
  
        [Inject]
        private void Construct(DiContainer diContainer, WispDecoratorFactory wispDecoratorFactory)
        {
            _diContainer = diContainer;
            _wispDecoratorFactory = wispDecoratorFactory;
        }
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _basicMovement = new BasicMovement(_rb);
            _mainCamera = Camera.main;
            BulletFactory = new BulletFactory(_bulletPrefab, _bulletContainer, _diContainer, _bulletSpawnPoint);
            _wispDecorator = new WispDecoratorStandard(new WispStandardComponent(BulletFactory, _bulletSpawnPoint), BulletFactory, _bulletSpawnPoint);
        }
        
        private void Update()
        {
            var currentPos = transform.position;
            var distance = Vector3.Distance(currentPos, _pastPosition);
            _currentDistance += distance;
            _pastPosition = currentPos;
            if (_currentDistance >= DistanceToShoot)
            {
                _wispDecorator.Shoot();
                _currentDistance = 0;
            }

            MoveForward();
            if (Input.GetMouseButtonDown(0))
                RotateBulletSpawnPoint();
        }

        private void MoveForward()
        {
            var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _basicMovement.Move(moveInput, 6f);
        }

        private void RotateBulletSpawnPoint()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - _bulletSpawnPoint.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _bulletSpawnPoint.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }
        
        public void AddDecorator<T>() where T: WispDecorator
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator added!");
            _wispDecorator = _wispDecoratorFactory.CreateDecorator<T>(this, _wispDecorator);
        }
        
    }
}