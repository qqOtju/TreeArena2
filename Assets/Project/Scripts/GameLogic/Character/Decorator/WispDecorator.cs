using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecorator: WispComponent
    {
        private WispComponent _component;
        
        public WispDecorator(WispComponent component)
        {
            _component = component;
        }
        
        public override void OnHealthHit(Bullet bullet, IHealth health)
        {
            _component.OnHealthHit(bullet, health);
        }

        public override void OnWallHit(Bullet bullet)
        {
            _component.OnWallHit(bullet);
        }

        public override void MoveForward(Bullet bullet)
        {
            _component.MoveForward(bullet);
        }

        public override void OnTargetDeath(Bullet bullet, IHealth health)
        {
            _component.OnTargetDeath(bullet, health);
        }

        public override Bullet CreateBullet()
        {
            return _component.CreateBullet();
        }

        public override void Shoot()
        {
            _component.Shoot();
        }
    }
}