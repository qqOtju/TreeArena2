using System;
using Project.Scripts.DesignPattern.FSM;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Movement;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Charger
{
    public class EnemyChargerStateAttack : State
    {
        private const float BaseTimeToAttack = 1f;
        private const float BaseChargeSpeed = 14f;
        private const float BaseStunTime = 2f;

        private readonly BasicMovement _basicMovement;
        private readonly IStateMachine _stateMachine;
        private readonly Action<Vector2> _rotateView;
        private readonly Action<IHealth> _attack;
        private readonly Transform _transform;
        private readonly Rigidbody2D _rb;
        private readonly Tree _tree;
        
        private Vector2 _moveInput;
        private float _attackTimer;
        private float _stunTimer;
        private bool _canAttack;
        private bool _isStunned;
        
        protected float TimeToAttack;
        protected float ChargeSpeed;
        protected float StunTime;
        protected float AttackRange = 1f;
        
        public EnemyChargerStateAttack(Transform transform, Tree tree, 
            BasicMovement basicMovement, IStateMachine stateMachine,
            Action<IHealth> attack, Action<Vector2> rotateView, Rigidbody2D rb)
        {
            _basicMovement = basicMovement;
            _stateMachine = stateMachine;
            _rotateView = rotateView;
            _transform = transform;
            _attack = attack;
            _tree = tree;
            _rb = rb;
        }
        
        public override void Enter()
        {
            _canAttack = false;
            _isStunned = false;
            TimeToAttack = BaseTimeToAttack;
            ChargeSpeed = BaseChargeSpeed;
            StunTime = BaseStunTime;
            _attackTimer = 0;
            _stunTimer = 0;
            _rb.linearVelocity = Vector2.zero;
        }

        public override void Update()
        {
            if(!_canAttack)
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= TimeToAttack)
                {
                    _canAttack = true;
                }
                return;
            }
            if (_isStunned)
            {
                _stunTimer += Time.deltaTime;
                if (_stunTimer >= StunTime)
                {
                    _rb.linearVelocity = Vector2.zero;
                    _stateMachine.ChangeState(_stateMachine.States[typeof(EnemyChargerStateWalk)]);
                }
                return;
            }
            _moveInput = (_tree.transform.position - _transform.position).normalized;
            _rotateView(_moveInput);
            if (Vector2.Distance(_transform.position, _tree.transform.position) < AttackRange)
            {
                _attack(_tree);
                _isStunned = true;
            }
        }

        public override void FixedUpdate()
        {
            if(!_canAttack || _isStunned) return;
            _basicMovement.Move(_moveInput, ChargeSpeed);
        }

        public override void Exit() { }

        public override void OnDestroy() { }
    }
}