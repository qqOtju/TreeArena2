using System;
using Project.Scripts.Config.Enemy;
using Project.Scripts.DesignPattern.FSM;
using Project.Scripts.GameLogic.Movement;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Charger
{
    public class EnemyChargerStateWalk : State
    {
        private readonly BasicMovement _basicMovement;
        private readonly IStateMachine _stateMachine;
        private readonly Action<Vector2> _rotateView;
        private readonly Transform _transform;
        private readonly Tree _tree;
        
        private Vector3 _targetPosition;
        private EnemyValue _enemyValue;
        private Vector2 _moveInput;

        public EnemyChargerStateWalk(Transform transform, Tree tree, 
            BasicMovement basicMovement, EnemyValue enemyValue, 
            IStateMachine stateMachine, Action<Vector2> rotateView)
        {
            _tree = tree;
            _transform = transform;
            _basicMovement = basicMovement;
            _enemyValue = enemyValue;
            _stateMachine = stateMachine;
            _rotateView = rotateView;
        }
        
        public static Vector2 GetPointOnVector(Vector2 a, Vector2 b, float distance)
        {
            var direction = b - a; 
            var length = direction.magnitude;
            var unitDirection = direction / length;

            return b + unitDirection * distance;
        }

        public override void Enter()
        {
            _targetPosition = GetPointOnVector(_transform.position,
                _tree.transform.position, -_enemyValue.AttackRange);
        }

        public override void Update()
        {
            _moveInput = (_targetPosition - _transform.position).normalized;
            _rotateView(_moveInput);
            if (Vector2.Distance(_transform.position, _targetPosition) < 0.1f)
            {
                _moveInput = (_tree.transform.position - _transform.position).normalized;
                _rotateView(_moveInput);
                _stateMachine.ChangeState(_stateMachine.States[typeof(EnemyChargerStateAttack)]);
            }
        }

        public override void FixedUpdate()
        {
            _basicMovement.Move(_moveInput, _enemyValue.MoveSpeed);
        }

        public override void Exit() { }

        public override void OnDestroy() { }
    }
}