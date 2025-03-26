using Project.Scripts.Config.Enemy;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.UI.Game;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Boss.Around
{
    public class EnemyBossAround: EnemyShooter
    {
        [SerializeField] private Transform _pointsParent;
        [SerializeField] private UIBoss _uiBoss;

        private const float RotationSpeed = 1f;
        
        private float _damageMultiplier = 0f;

        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            var points = _pointsParent.GetComponentsInChildren<EnemyBossAroundPoint>();
            foreach (var point in points)
            {
                point.gameObject.SetActive(true);
                point.Initialize(enemyStat, enemyType, tree);
            }
            _uiBoss.Initialize(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            RotatePoints();
        }

        private void RotatePoints()
        {
            _pointsParent.Rotate(Vector3.forward, RotationSpeed);
        }

        public override void TakeDamage(float dmg)
        {
            dmg *= _damageMultiplier;
            base.TakeDamage(dmg);
        }
        
        public void AddMultiplier(float multiplier)
        {
            _damageMultiplier += multiplier;
            DebugSystem.Instance.Log(LogType.Boss ,$"Multiplier added: {_damageMultiplier}");
        }
    }
}