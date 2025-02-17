using System;
using Project.Scripts.Config.Enemy;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.Enemy
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EnemyBase: EntityBase, IEnemyHealth
    {
        [SerializeField] private EnemyType _type;
        
        private Vector3 _targetPosition;
        private EnemyValue _enemyValue;
        
        public EnemyType Type => _type;
        
        public event Action<EnemyBase> OnDeath;

        public void Initialize(EnemyValue enemyStat, Transform target)
        {
            _enemyValue = enemyStat;
            var range = 0.8f;
            _targetPosition = target.position + new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
            SetInitialHealth(_enemyValue.MaxHealth);
            #if (DEVELOPMENT_BUILD || UNITY_EDITOR)
            DebugSystem.Instance.Log(LogType.Enemy, $"Enemy {gameObject.name} initialized with \n" +
                                                   $"MaxHealth: {_enemyValue.MaxHealth} \n" +
                                                   $"Speed: {_enemyValue.MoveSpeed} \n" +
                                                   $"Damage: {_enemyValue.Damage} \n" +
                                                   $"AttackSpeed: {_enemyValue.AttackSpeed} \n" +
                                                   $"AttackRange: {_enemyValue.AttackRange} \n");
            #endif
        }

        private void Awake()
        {
            OnHealthChange += CheckDeath;
        }
        
        private void CheckDeath(OnHealthChangeArgs obj)
        {
            if (obj.Type != HeathChangeType.Death) return;
            OnDeath?.Invoke(this);
        }
    }
}