using Project.Scripts.Config.Enemy;
using Project.Scripts.Entity;
using UnityEngine;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy
{
    public class EnemyWalkShooter: EnemyShooter
    {
        private const float LeftAngle = 200f;
        private const float RightAngle = 340f;
        
        private Vector2 _targetPosition;
        private float _angleSpeed;
        private float _angle;
        private bool _isRight;

        public override void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            base.Initialize(enemyStat, enemyType, tree);
            _angle = Random.Range(LeftAngle, RightAngle);
            _angleSpeed = enemyStat.MoveSpeed * 10;
            _isRight = Random.Range(0, 2) == 1;
        }

        protected override void Update()
        {
            base.Update();
            SetTargetPosition();
        }

        private void SetTargetPosition()
        {
            if (_isRight)
            {
                _angle += _angleSpeed * Time.deltaTime;
                if (_angle >= RightAngle)
                    _isRight = false;
            }
            else
            {
                _angle -= _angleSpeed * Time.deltaTime;
                if (_angle <= LeftAngle)
                    _isRight = true;
            }

            var treePosition = TargetTree.transform.position;
            _targetPosition = GetPointOnCircle(treePosition,
                EnemyValue.AttackRange, _angle);
        }

        protected override void CalculateMoveInput()
        {
            if(Vector2.Distance(transform.position, _targetPosition) < 0.1f)
                MoveInput = Vector2.zero;
            else
                MoveInput = (_targetPosition - (Vector2)transform.position).normalized;
        }

        private Vector2 GetPointOnCircle(Vector2 center, float radius, float angle)
        {
            var angleRad = angle * Mathf.Deg2Rad;
            var x = center.x + radius * Mathf.Cos(angleRad);
            var y = center.y + radius * Mathf.Sin(angleRad);
            return new Vector2(x, y);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(TargetTree.transform.position, _targetPosition);
            Gizmos.DrawWireSphere(_targetPosition, 0.5f);
        }
    }
}