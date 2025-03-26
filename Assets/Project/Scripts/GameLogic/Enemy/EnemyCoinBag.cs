using System.Collections;
using Project.Scripts.Config.Enemy;
using Project.Scripts.Entity;
using UnityEngine;
using Random = UnityEngine.Random;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy
{
    public class EnemyCoinBag: EnemyBase
    {
        private const float MaxYPosition = -6f;
        private const float MinYPosition = -3.5f;
        private const float XPosition = 16f;

        private Vector2 _positionA;
        private Vector2 _positionB;
        private Vector2 _targetPosition;
        private bool _isMovingToA;
        private WaitForSeconds _wait = new (.3f);
        
        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            StartCoroutine(SetPosition());
        }

        protected override void Update()
        {
            base.Update();
            if(_isMovingToA)
            {
                _targetPosition = Vector2.MoveTowards(_targetPosition,_positionA,
                    (EnemyValue.MoveSpeed * Time.deltaTime));
                if(Vector2.Distance(transform.position, _positionA) < 0.1f)
                    Attack(this);
            }
            else
            {
                _targetPosition = Vector2.MoveTowards(_targetPosition,_positionB,
                    (EnemyValue.MoveSpeed * Time.deltaTime));
                if(Vector2.Distance(transform.position, _positionB) < 0.1f)
                    Attack(this);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllCoroutines();
        }

        private IEnumerator SetPosition()
        {
            yield return _wait;
            _positionA = new Vector2(XPosition, Random.Range(MinYPosition, MaxYPosition));
            _positionB = new Vector2(-XPosition, Random.Range(MinYPosition, MaxYPosition));
            var position = transform.position;
            var distanceToA = Vector2.Distance(_positionA, position);
            var distanceToB = Vector2.Distance(_positionB, position);
            if (distanceToA < distanceToB)
            {
                _isMovingToA = false;
                _targetPosition = _positionA;
            }
            else
            {
                _isMovingToA = true;
                _targetPosition = _positionB;
            }
        }

        protected override void CalculateMoveInput()
        {
            MoveInput = (_targetPosition - (Vector2)transform.position).normalized;
        }

        protected override void AttackCycle() { }

        protected override void Attack(IHealth health)
        {
            var newValue = new EnemyValue(0,0,0,0,0,0,0,0,0,0,EnemyValue.CoinsDropCount);
            EnemyValue -= newValue; 
            TakeDamage(EnemyValue.MaxHealth);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_positionA, 0.5f);
            Gizmos.DrawWireSphere(_positionB, 0.5f);
            Gizmos.DrawLine(_positionA, _positionB);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_targetPosition, 0.25f);
        }
    }
}