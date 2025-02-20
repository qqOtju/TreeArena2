using System;
using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public class EnemyBullet: Bullet
    {
        private BulletEnemyActionsArgs _args;

        public override void Init(BulletActionsArgs bulletActionsArgs) { }

        public void Init(BulletEnemyActionsArgs args)
        {
            _args = args;
        }

        private void FixedUpdate()
        {
            _args.MoveForward?.Invoke(this);
        }
        
        //ToDo: Не забути всюди зробити стіни або зробити, щось щоб куля з часом викликала _args.OnWallHit, бо інакше вона буде літати вічно 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(gameObject.activeSelf == false)
                return;
            if (other.gameObject.CompareTag("Tree"))
            {
                _args.OnTreeHit?.Invoke(this, other.gameObject.GetComponent<IHealth>());
            }
            else if(other.gameObject.CompareTag("Wall"))
                _args.OnWallHit?.Invoke(this);
        }
    }
}