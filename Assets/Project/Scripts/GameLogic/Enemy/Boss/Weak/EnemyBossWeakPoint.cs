using Project.Scripts.Config.Enemy;
using UnityEngine;

namespace Project.Scripts.GameLogic.Enemy.Boss.Weak
{
    public class EnemyBossWeakPoint: EnemyBase
    {
        [SerializeField] private EnemyBossWeak _boss;
        [SerializeField] private EnemyConfig _config;

        private const float DamageMultiplier = 3f;

        protected override void Awake()
        {
            base.Awake();
            _boss.OnDeath += BossOnOnDeath;
        }

        protected override void Start()
        {
            EnemyValue = _config.Value;
            SetInitialHealth(EnemyValue.MaxHealth);
        }

        protected override void Update() { }
        protected override void FixedUpdate() { }

        private void BossOnOnDeath(EnemyBase obj)
        {
            // TakeDamage(CurrentHealth);
        }

        public override void TakeDamage(float dmg)
        {
            //base.TakeDamage(dmg);
            _boss.TakeDamage(dmg * DamageMultiplier);
        }
    }
}