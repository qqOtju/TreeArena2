using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;

namespace Project.Scripts.GameLogic.Character.Component
{
    public abstract class WispComponent
    {
        public abstract void OnHealthHit(Bullet bullet, IHealth health);
        public abstract void OnWallHit(Bullet bullet);
        public abstract void MoveForward(Bullet bullet);
        public abstract void OnTargetDeath(Bullet bullet, IHealth health);
        public abstract Bullet CreateBullet();
        public abstract void Shoot();
    }
}