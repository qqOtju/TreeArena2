using System;
using Project.Scripts.DesignPattern.FSM;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Enemy.Charger;
using Project.Scripts.GameLogic.Movement;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Boss.Charger
{
    public class EnemyBossChargerStateAttack: EnemyChargerStateAttack
    {
        private const float RageTimeToAttack = 0.5f;
        private const float RageChargeSpeed = 20f;
        private const float RageStunTime = 1f;
        
        private readonly EnemyBase _enemyBase;
        
        public EnemyBossChargerStateAttack(Transform transform, 
            Tree tree, BasicMovement basicMovement, IStateMachine stateMachine,
            Action<IHealth> attack, Action<Vector2> rotateView, Rigidbody2D rb, 
            EnemyBase enemyBase) : 
            base(transform, tree, basicMovement, stateMachine, attack, rotateView, rb)
        {
            _enemyBase = enemyBase;
        }

        public override void Enter()
        {
            base.Enter();
            AttackRange = 1.5f;
            if(_enemyBase.CurrentHealth < _enemyBase.MaxHealth / 2)
            {
                TimeToAttack = RageTimeToAttack;
                ChargeSpeed = RageChargeSpeed;
                StunTime = RageStunTime;
            }
        }
    }
}