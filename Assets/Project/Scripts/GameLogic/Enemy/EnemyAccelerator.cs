using Project.Scripts.Config.Enemy;
using Project.Scripts.Entity;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy
{
    public class EnemyAccelerator: EnemyBase
    {
        private const float MaxAttackBonus = 2f;
        private const float Acceleration = 0.1f;
        private const float TimePerBonus = 1f;
        
        private float _attackBonus;
        private float _bonusTimer;

        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            _attackBonus = 0;    
        }

        protected override void Update()
        {
            base.Update();
            _bonusTimer += Time.deltaTime;
            if (_bonusTimer >= TimePerBonus)
            {
                _bonusTimer = 0;
                if(_attackBonus < MaxAttackBonus)
                    _attackBonus += Acceleration;
            }
        }

        protected override void Attack(IHealth health)
        {
            var damage = EnemyValue.Damage * (1 + _attackBonus);
            health.TakeDamage(damage);
            RaiseOnDealDamage(health);
        }
    }
}