using System;
using Project.Scripts.Module.Stats.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config.Enemy
{
    [CreateAssetMenu(menuName = "Config/Enemy", fileName = "Enemy Config")]
    public class EnemyConfig: ScriptableObject
    {
        [SerializeField] private EnemyValue _value;
        
        public EnemyValue Value => _value;
    }

    [Serializable]
    public struct EnemyValue
    {
        [HorizontalGroup("Split", Width = 0.5f)]

        [BoxGroup("Split/Values")]
        [SerializeField] private int _maxHealth;
        [BoxGroup("Split/Values")]
        [SerializeField] private float _moveSpeed;
        [BoxGroup("Split/Values")]
        [SerializeField] private float _damage;
        [BoxGroup("Split/Values")]
        [SerializeField] private float _attackSpeed;
        [BoxGroup("Split/Values")]
        [SerializeField] private float _attackRange;
        [BoxGroup("Split/Values")]
        [SerializeField] private int _coinsDropCount;
        
        [BoxGroup("Split/Bonuses")]
        [SerializeField] private int _maxHealthPerWave;
        [BoxGroup("Split/Bonuses")]
        [SerializeField] private float _moveSpeedPerWave;
        [BoxGroup("Split/Bonuses")]
        [SerializeField] private float _damagePerWave;
        [BoxGroup("Split/Bonuses")]
        [SerializeField] private float _attackSpeedPerWave;
        [BoxGroup("Split/Bonuses")]
        [SerializeField] private float _attackRangePerWave;
        
        public int MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public float AttackRange => _attackRange;
        public int MaxHealthPerWave => _maxHealthPerWave;
        public float MoveSpeedPerWave => _moveSpeedPerWave;
        public float DamagePerWave => _damagePerWave;
        public float AttackSpeedPerWave => _attackSpeedPerWave;
        public float AttackRangePerWave => _attackRangePerWave;
        //ToDo make unique item that increases drop count
        public int CoinsDropCount => _coinsDropCount;
        
        public EnemyValue(int maxHealth, float moveSpeed, 
            float damage, float attackSpeed, float attackRange, 
            int maxHealthPerWave, float moveSpeedPerWave, 
            float damagePerWave, float attackSpeedPerWave, 
            float attackRangePerWave, int coinsDropCount)
        {
            _maxHealth = maxHealth;
            _moveSpeed = moveSpeed;
            _damage = damage;
            _attackSpeed = attackSpeed;
            _attackRange = attackRange;
            _maxHealthPerWave = maxHealthPerWave;
            _moveSpeedPerWave = moveSpeedPerWave;
            _damagePerWave = damagePerWave;
            _attackSpeedPerWave = attackSpeedPerWave;
            _attackRangePerWave = attackRangePerWave;
            _coinsDropCount = coinsDropCount;
        }

        public static EnemyValue GetMultipliedValue(EnemyValue value, int wave)
        {
            var health = value.MaxHealth + value.MaxHealthPerWave * wave;
            var moveSpeed = value.MoveSpeed + value.MoveSpeedPerWave * wave;
            var damage = value.Damage + value.DamagePerWave * wave;
            var attackSpeed = value.AttackSpeed + value.AttackSpeedPerWave * wave;
            var attackRange = value.AttackRange + value.AttackRangePerWave * wave;
            var coinsDropCount = value.CoinsDropCount;
            return new EnemyValue
            {
                _maxHealth = health,
                _moveSpeed = moveSpeed,
                _damage = damage,
                _attackSpeed = attackSpeed,
                _attackRange = attackRange,
                _coinsDropCount = coinsDropCount
            };
        }

        public static EnemyValue operator +(EnemyValue a, EnemyValue b)
        {
            return new EnemyValue
            {
                _maxHealth = a._maxHealth + b._maxHealth,
                _moveSpeed = a._moveSpeed + b._moveSpeed,
                _damage = a._damage + b._damage,
                _attackSpeed = a._attackSpeed + b._attackSpeed,
                _attackRange = a._attackRange + b._attackRange,
                _coinsDropCount = a._coinsDropCount + b._coinsDropCount
            };
        }
        
        public static EnemyValue operator -(EnemyValue a, EnemyValue b)
        {
            return new EnemyValue
            {
                _maxHealth = a._maxHealth - b._maxHealth,
                _moveSpeed = a._moveSpeed - b._moveSpeed,
                _damage = a._damage - b._damage,
                _attackSpeed = a._attackSpeed - b._attackSpeed,
                _attackRange = a._attackRange - b._attackRange,
                _coinsDropCount = a._coinsDropCount - b._coinsDropCount
            };
        }

        public static EnemyValue GetUpgraded(EnemyValue value, EnemyBonuses bonuses)
        {
            var maxHealth = (int)(value.MaxHealth * (1 + bonuses.MaxHealth / 100));
            var moveSpeed = value.MoveSpeed * (1 + bonuses.MoveSpeed / 100);
            var damage = value.Damage * (1 + bonuses.Damage / 100);
            var attackSpeed = value.AttackSpeed * (1 + bonuses.AttackSpeed / 100);
            var attackRange = value.AttackRange * (1 + bonuses.AttackRange / 100);
            var coinsDropCount = value.CoinsDropCount;
            return new EnemyValue
            {
                _maxHealth = maxHealth,
                _moveSpeed = moveSpeed,
                _damage = damage,
                _attackSpeed = attackSpeed,
                _attackRange = attackRange,
                _coinsDropCount = coinsDropCount
            };
        }
    }
}