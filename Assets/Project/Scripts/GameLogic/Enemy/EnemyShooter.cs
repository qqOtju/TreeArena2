using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.Module.Factory;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameLogic.Enemy
{
    public class EnemyShooter: EnemyBase
    {
        [SerializeField] private EnemyBullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;
        
        private BulletFactory _bulletFactory;
        private DiContainer _diContainer;
        private BulletEnemyActionsArgs _args;
            
        [Inject]
        private void Construct(DiContainer container)
        {
            _diContainer = container;
        }
        
        protected override void Start()
        {
            base.Start();
            var container = new GameObject("BulletContainer").transform;
            container.SetParent(null);
            TargetPosition = TargetTree.transform.position;
            _bulletFactory = new BulletFactory(_bulletPrefab, container, _diContainer, _bulletSpawnPoint);
            _args = new BulletEnemyActionsArgs(OnTreeHit, OnWallHit, MoveForward);
            _bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        protected override void Attack(IHealth health)
        {
            _bulletFactory.Get();
        }

        private Bullet ConfigBullet(Bullet bullet)
        {
            if (bullet is EnemyBullet enemyBullet)
                enemyBullet.Init(_args);
            var direction = transform.position - TargetPosition;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.localRotation = Quaternion.Euler(0, 0, targetAngle - 180);
            return bullet;
        }
        
        private void OnTreeHit(Bullet bullet, IHealth health)
        {
            health.TakeDamage(EnemyValue.Damage);
            RaiseOnDealDamage(health);
            _bulletFactory.Release(bullet);
        }
        
        private void OnWallHit(Bullet bullet)
        {
            _bulletFactory.Release(bullet);
        }

        private void MoveForward(Bullet bullet)
        {
            var tr = bullet.transform;
            var rb = bullet.Rb;
            var moveForce = tr.position + tr.right / 3;
            rb.MovePosition(moveForce);
        }
    }
}