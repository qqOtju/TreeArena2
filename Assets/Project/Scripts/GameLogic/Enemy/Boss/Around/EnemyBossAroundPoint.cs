using Project.Scripts.Config.Enemy;
using Project.Scripts.Entity;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Boss.Around
{
    public class EnemyBossAroundPoint: EnemyBase
    {
        [SerializeField] private EnemyBossAround _boss;
        [SerializeField] private EnemyConfig _config;

        private const float Multiplier = 0.3f;

        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            var layer = gameObject.layer;
            EnemyValue = _config.Value;
            SetInitialHealth(EnemyValue.MaxHealth);
            Physics2D.IgnoreLayerCollision(layer, layer, true);
        }
        
        protected override void Awake()
        {
            base.Awake();
            OnDeath += AddMultiplier;
        }

        protected override void OnDestroy()
        {
            OnDeath -= AddMultiplier;
        }

        protected override void Update() { }

        protected override void FixedUpdate() { }
        
        private void AddMultiplier(EnemyBase obj)
        {
            _boss.AddMultiplier(Multiplier);
            gameObject.SetActive(false);
        }
    }
}