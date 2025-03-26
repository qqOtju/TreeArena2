using Project.Scripts.Config.Enemy;
using Project.Scripts.Entity;
using Project.Scripts.UI.Game;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy.Boss.Weak
{
    public class EnemyBossWeak: EnemyBase
    {
        [SerializeField] private EnemyBossWeakPoint[] _weakPoints;
        [SerializeField] private UIBoss _uiBoss;

        private const float ChangeTime = 6f;
        
        private EnemyBossWeakPoint _currentWeakPoint;
        private Collider2D _collider;
        private float _changeTimer;
        
        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            _uiBoss.Initialize(this);
        }

        protected override void Start()
        {
            base.Start();
            _collider = GetComponent<Collider2D>();
            foreach (var weakPoint in _weakPoints)
            {
                weakPoint.gameObject.SetActive(false);
            }
            _currentWeakPoint = _weakPoints[Random.Range(0, _weakPoints.Length)];
            _currentWeakPoint.gameObject.SetActive(true);
            var weakPointCollider = _currentWeakPoint.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_collider, weakPointCollider, true);
        }

        protected override void Update()
        {
            base.Update();
            _changeTimer += Time.deltaTime;
            if (_changeTimer >= ChangeTime)
            {
                _changeTimer = 0;
                ChangeWeakPoint();
            }
        }

        private void ChangeWeakPoint()
        {
            var index = 0;
            while (_weakPoints[index] == _currentWeakPoint)
            {
                index = Random.Range(0, _weakPoints.Length);
            }
            _currentWeakPoint.gameObject.SetActive(false);
            _currentWeakPoint = _weakPoints[index];
            _currentWeakPoint.gameObject.SetActive(true);
            var weakPointCollider = _currentWeakPoint.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_collider, weakPointCollider, true);
        }
    }
}