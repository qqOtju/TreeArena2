using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;

namespace Project.Scripts.GameLogic.Character.Component
{
    public abstract class WispComponent
    {
        public abstract void OnEnemyHit(Bullet bullet, IEnemyHealth health);
        public abstract void OnWallHit(Bullet bullet);
        public abstract void MoveForward(Bullet bullet);
        public abstract void OnEnemyDeath(Bullet bullet, IEnemyHealth health);
        public abstract Bullet ConfigBullet(Bullet bullet);
        public abstract void Shoot();
        //ToDo: somehow add event OnShoot with args
    } 
}