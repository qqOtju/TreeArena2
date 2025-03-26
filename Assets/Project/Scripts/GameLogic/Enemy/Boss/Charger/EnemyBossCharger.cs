using Project.Scripts.Config.Enemy;
using Project.Scripts.DesignPattern.FSM;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Enemy.Charger;
using Project.Scripts.UI.Game;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Boss.Charger
{
    public class EnemyBossCharger: EnemyBase
    {
        [SerializeField] private UIBoss _uiBoss;
        
        private EnemyChargerStateAttack _stateAttack;
        private EnemyChargerStateWalk _stateWalk;
        private IStateMachine _stateMachine;

        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            _uiBoss.Initialize(this);
        }
        
        protected override void Start()
        {
            base.Start();
            _stateMachine = new StateMachine();
            _stateAttack = new EnemyBossChargerStateAttack(transform, TargetTree, BasicMovement,
                _stateMachine, Attack, RotateView, Rb, this);
            _stateWalk = new EnemyChargerStateWalk(transform, TargetTree, BasicMovement,
                EnemyValue, _stateMachine, RotateView);
            _stateMachine.AddState(typeof(EnemyChargerStateAttack), _stateAttack);
            _stateMachine.AddState(typeof(EnemyChargerStateWalk), _stateWalk);
            _stateMachine.Initialize(_stateWalk);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _stateAttack.OnDestroy();
            _stateWalk.OnDestroy();
        }

        protected override void Update()
        {
            _stateMachine.CurrentState.Update();
        }
        
        protected override void FixedUpdate()
        {
            _stateMachine.CurrentState.FixedUpdate();
        }
    }
}