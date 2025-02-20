using UnityEngine;

namespace Project.Scripts.GameLogic.Enemy
{
    public class EnemyInteresting: EnemyBase
    {
        private Transform _transform;
        private float _time;

        protected override void Start()
        {
            base.Start();
            _transform = transform;
        }

        protected override void Move()
        {
            if(MoveInput == Vector2.zero) return;
            _time += Time.fixedDeltaTime;
            Vector2 direction = (TargetPosition - _transform.position).normalized;
            var perpendicularDirection = new Vector2(-direction.y, direction.x);
            const float waveAmplitude = 1f; // Висота хвилі
            const float waveFrequency = 2f;  // Швидкість хвилі
            var waveOffset = Mathf.Cos(_time * waveFrequency) * waveAmplitude;
            var moveDirection = direction + perpendicularDirection * waveOffset;
            BasicMovement.Move(moveDirection, EnemyValue.MoveSpeed);
        }
    }
}