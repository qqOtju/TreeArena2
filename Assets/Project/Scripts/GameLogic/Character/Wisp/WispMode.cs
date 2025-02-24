using System;
using Project.Scripts.Debug;
using Project.Scripts.DesignPattern.FSM;
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
    public class WispMode: WispBase
    {
        [SerializeField] private Bullet _bulletAPrefab;
        [SerializeField] private Bullet _bulletBPrefab;

        private WispDecoratorFactory _wispDecoratorFactory;
        private IStateMachine _stateMachine;
        private DiContainer _diContainer;
        private WispBaseModeStateA _stateA;
        private WispBaseModeStateB _stateB;
        private Transform _bulletAContainer;
        private Transform _bulletBContainer;
        private WispBonuses _wispBonuses;
        private WispStats _wispStats;
        
        [Inject]
        private void Construct(DiContainer diContainer, WispDecoratorFactory wispDecoratorFactory, WispStats wispStats, WispBonuses wispBonuses)
        {
            _diContainer = diContainer;
            _wispDecoratorFactory = wispDecoratorFactory;
            _wispStats = wispStats;
            _wispBonuses = wispBonuses;
        }
        
        protected override void Start()
        {
            base.Start();
            _stateMachine = new StateMachine();
            _bulletAContainer = new GameObject("BulletAContainer").transform;
            _bulletBContainer = new GameObject("BulletBContainer").transform;
            _bulletAContainer.SetParent(BulletContainer);
            _bulletBContainer.SetParent(BulletContainer);
            _stateA = new WispBaseModeStateA(_bulletAPrefab, _bulletAContainer, _bulletSpawnPoint, _diContainer, _wispDecoratorFactory, _wispStats, _wispBonuses);
            _stateB = new WispBaseModeStateB(_bulletBPrefab, _bulletBContainer, _bulletSpawnPoint, _diContainer, _wispDecoratorFactory, _wispStats, _wispBonuses);
            _stateMachine.AddState(typeof(WispBaseModeStateA), _stateA);
            _stateMachine.AddState(typeof(WispBaseModeStateB), _stateB);
            _stateMachine.Initialize(_stateA);
        }
        
        protected override void Update()
        {
            base.Update();
            _stateMachine.CurrentState.Update();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DebugSystem.Instance.Log(LogType.Wisp, "<color=red>StateA selected!</color>");
                _stateMachine.ChangeState(_stateA);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                DebugSystem.Instance.Log(LogType.Wisp, "<color=blue>StateB selected!</color>");
                _stateMachine.ChangeState(_stateB);
            }
        }
        
        public override void AddDecorator(Type wispDecoratorType)
        {
            DebugSystem.Instance.Log(LogType.Wisp, "Decorator added!");
            _stateA.AddDecorator(wispDecoratorType);
            _stateB.AddDecorator(wispDecoratorType);
        }
    }
    
    public class WispBaseModeStateA: State, IWisp
    {
        private readonly WispDecoratorFactory _wispDecoratorFactory;
        private readonly WispStats _wispStats;
        private readonly WispBonuses _wispBonuses;
        
        private WispDecorator _decorator;
        private float _attackTimer;

        public Transform BulletSpawnPoint { get; }
        public BulletFactory BulletFactory { get; }
        
        public WispBaseModeStateA(Bullet bullet, Transform container, Transform bulletSpawnPoint, DiContainer diContainer, 
            WispDecoratorFactory factory, WispStats wispStats, WispBonuses wispBonuses)
        {
            BulletSpawnPoint = bulletSpawnPoint;
            _wispDecoratorFactory = factory;
            _wispStats = wispStats;
            _wispBonuses = wispBonuses;
            BulletFactory = new BulletFactory(bullet, container, diContainer, bulletSpawnPoint);
            _decorator = new WispDecoratorStandard(new WispComponentStateA(BulletFactory, bulletSpawnPoint, _wispStats), BulletFactory, bulletSpawnPoint);
        }
        
        public override void Enter()
        {
            _wispBonuses.Damage += 30;
        }
        
        public override void Exit()
        {
            _wispBonuses.Damage -= 30;
        }
        
        public override void Update()
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= _wispStats.AttackSpeed)
            {
                _decorator.Shoot();
                _attackTimer = 0;
            }
        }

        public override void FixedUpdate() { }

        public override void OnDestroy() { }

        public void AddDecorator(Type wispDecoratorType)
        {
            DebugSystem.Instance.Log(LogType.Wisp, "StateA decorator set!");
            _decorator = _wispDecoratorFactory.CreateDecorator(wispDecoratorType, this, _decorator);
        }
    }
    
    public class WispBaseModeStateB: State, IWisp
    {
        private readonly WispDecoratorFactory _wispDecoratorFactory;
        private readonly WispStats _wispStats;
        private readonly WispBonuses _wispBonuses;
        
        private WispDecorator _decorator;
        private float _attackTimer;

        public Transform BulletSpawnPoint { get; }
        public BulletFactory BulletFactory { get; }
        
        public WispBaseModeStateB(Bullet bullet, Transform container, Transform bulletSpawnPoint, DiContainer diContainer, 
            WispDecoratorFactory factory, WispStats wispStats, WispBonuses wispBonuses)
        {
            BulletSpawnPoint = bulletSpawnPoint;
            _wispDecoratorFactory = factory;
            _wispStats = wispStats;
            _wispBonuses = wispBonuses;
            BulletFactory = new BulletFactory(bullet, container, diContainer, bulletSpawnPoint);
            _decorator = new WispDecoratorStandard(new WispComponentStateB(BulletFactory, bulletSpawnPoint, _wispStats), BulletFactory, bulletSpawnPoint);
        }
        
        public override void Enter()
        {
            _wispBonuses.AttackSpeed += 30;
        }
        
        public override void Exit()
        {
            _wispBonuses.AttackSpeed -= 30;
        }
        
        public override void Update()
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= _wispStats.AttackSpeed)
            {
                _decorator.Shoot();
                _attackTimer = 0;
            }
        }

        public override void FixedUpdate() { }

        public override void OnDestroy() { }

        public void AddDecorator(Type wispDecoratorType)
        {
            DebugSystem.Instance.Log(LogType.Wisp, "StateB decorator set!");
            _decorator = _wispDecoratorFactory.CreateDecorator(wispDecoratorType, this, _decorator);
        }
    }
}