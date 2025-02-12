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
    public class WispMode: MonoBehaviour, IWisp
    {
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private Bullet _bulletAPrefab;
        [SerializeField] private Bullet _bulletBPrefab;
        [SerializeField] private Transform _bulletAContainer;
        [SerializeField] private Transform _bulletBContainer;

        private WispDecoratorFactory _wispDecoratorFactory;
        private DiContainer _diContainer;
        private IWispModeState _currentState;
        private WispModeStateA _stateA;
        private WispModeStateB _stateB;
        
        public Transform BulletSpawnPoint => _bulletSpawnPoint;
        public BulletFactory BulletFactory { get; private set; }
        public WispDecorator WispDecorator { get; private set; }
        
        [Inject]
        private void Construct(DiContainer diContainer, WispDecoratorFactory wispDecoratorFactory)
        {
            _diContainer = diContainer;
            _wispDecoratorFactory = wispDecoratorFactory;
        }
        
        private void Start()
        {
            _stateA = new WispModeStateA(_bulletAPrefab, _bulletAContainer, _bulletSpawnPoint, _diContainer, _wispDecoratorFactory);
            _stateB = new WispModeStateB(_bulletBPrefab, _bulletBContainer, _bulletSpawnPoint, _diContainer, _wispDecoratorFactory);
            _currentState = _stateA;
        }
        
        private void Update()
        {
            _currentState.Update();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DebugSystem.Instance.Log(LogType.Wisp, "<color=red>StateA selected!</color>");
                _currentState = _stateA;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                DebugSystem.Instance.Log(LogType.Wisp, "<color=blue>StateB selected!</color>");
                _currentState = _stateB;
            }
        }
        
        public void AddDecorator<T>() where T: WispDecorator
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator added!");
            _stateA.AddDecorator<T>();
            _stateB.AddDecorator<T>();
        }
    }

    public interface IWispModeState
    {
        public void Update();
    }
    
    public class WispModeStateA: IWispModeState, IWisp
    {
        //Add damage, but decrease attack speed

        private const float AttackSpeed = 0.8f;
        
        private readonly WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _decorator;
        private float _attackTimer;

        public Transform BulletSpawnPoint { get; }
        public BulletFactory BulletFactory { get; }
        
        public WispModeStateA(Bullet bullet, Transform container, Transform bulletSpawnPoint, DiContainer diContainer, 
            WispDecoratorFactory factory)
        {
            BulletSpawnPoint = bulletSpawnPoint;
            _wispDecoratorFactory = factory;
            BulletFactory = new BulletFactory(bullet, container, diContainer, bulletSpawnPoint);
            _decorator = new WispDecoratorStandard(new WispComponentStateA(BulletFactory, bulletSpawnPoint), BulletFactory, bulletSpawnPoint);
        }
        
        public void Update()
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= AttackSpeed)
            {
                _decorator.Shoot();
                _attackTimer = 0;
            }
        }

        public void AddDecorator<T>() where T : WispDecorator
        {
            DebugSystem.Instance.Log(LogType.Wisp, "StateA decorator set!");
            _decorator = _wispDecoratorFactory.CreateDecorator<T>(this, _decorator);
        }
    }
    
    public class WispModeStateB: IWispModeState, IWisp
    {
        //Add damage, but decrease attack speed

        private const float AttackSpeed = 0.4f;
        
        private readonly WispDecoratorFactory _wispDecoratorFactory;
        private WispDecorator _decorator;
        private float _attackTimer;

        public Transform BulletSpawnPoint { get; }
        public BulletFactory BulletFactory { get; }
        
        public WispModeStateB(Bullet bullet, Transform container, Transform bulletSpawnPoint, DiContainer diContainer, 
            WispDecoratorFactory factory)
        {
            BulletSpawnPoint = bulletSpawnPoint;
            _wispDecoratorFactory = factory;
            BulletFactory = new BulletFactory(bullet, container, diContainer, bulletSpawnPoint);
            _decorator = new WispDecoratorStandard(new WispComponentStateB(BulletFactory, bulletSpawnPoint), BulletFactory, bulletSpawnPoint);
        }
        
        public void Update()
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= AttackSpeed)
            {
                _decorator.Shoot();
                _attackTimer = 0;
            }
        }

        public void AddDecorator<T>() where T : WispDecorator
        {
            DebugSystem.Instance.Log(LogType.Wisp, "StateB decorator set!");
            _decorator = _wispDecoratorFactory.CreateDecorator<T>(this, _decorator);
        }
    }
}