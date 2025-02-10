using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public class BasicBullet: Bullet
    {
        private Vector3 _pastPosition;
        private bool _outOfRange;

        public override void Init(BulletActionsArgs args)
        {
            base.Init(args);
            _pastPosition = transform.position;
            _outOfRange = false;
        }
        
        private void FixedUpdate()
        {
            if(_outOfRange) return;
            var currentPos = BulletTr.position;
            var distance = Vector3.Distance(currentPos, _pastPosition);
            CurrenDistance -= distance;
            _pastPosition = currentPos;
            if (CurrenDistance <= 0 && !_outOfRange)
                _outOfRange = true;
            Args.MoveForward?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(gameObject.activeSelf == false)
                return;
            if (other.gameObject.CompareTag("Health"))
            {
                CurrentPiercing--;
                Args.OnHealthHit?.Invoke(this, other.gameObject.GetComponent<IHealth>());
            }
            else if(other.gameObject.CompareTag("Wall"))
                Args.OnWallHit?.Invoke(this);
        }
    }
}