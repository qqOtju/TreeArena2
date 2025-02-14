using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorUniqueBullet: WispDecorator
    {
        private const int ShotsToActivate = 3;
        private const int UniqueDamage = 100;
        
        private readonly BulletFactory _uniqueFactory;
        private readonly Transform _spawnPoint;
        
        private int _currentShot;
        
        public WispDecoratorUniqueBullet(WispComponent component, 
            BulletFactory baseFactory, Transform spawnPoint, BulletFactory uniqueFactory, WispStats wispStats) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, $"<color=yellow>Unique bullet decorator added!</color>");
            _uniqueFactory = uniqueFactory;
            _spawnPoint = spawnPoint;
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, wispStats.Piercing);
            baseFactory.SetActions(args);
            var uniqueArgs = new BulletActionsArgs(UniqueOnHealthHit, OnWallHit, UniqueMoveForward, 0);
            _uniqueFactory.SetActions(uniqueArgs);
        }

        //ToDo: made damage in this method with WispStats and critical
        private void UniqueOnHealthHit(Bullet bullet, IHealth health)
        {
            health.TakeDamage(UniqueDamage);
            _uniqueFactory.Release(bullet);
        }

        private void UniqueMoveForward(Bullet bullet)
        {
            if (bullet.CurrenDistance <= 0)
            {
                _uniqueFactory.Release(bullet);
                return;
            }
            var tr = bullet.gameObject.transform;
            var rb = bullet.Rb;
            var forwardDirection = tr.right * 0.2f; // Швидкість руху вперед
            const float waveAmplitude = 0.1f; // Висота хвилі
            const float waveFrequency = 0.5f; // Частота хвилі
            var waveOffset = Mathf.Cos(bullet.CurrenDistance * waveFrequency) * waveAmplitude;
            var perpendicularDirection = tr.up * waveOffset;
            var moveForce = tr.position + forwardDirection + perpendicularDirection;
            rb.MovePosition(moveForce);
        }

        public override void Shoot()
        {
            base.Shoot();
            _currentShot++;
            if (_currentShot >= ShotsToActivate)
            {
                DebugSystem.Instance.Log(LogType.WispComponent, $"<color=yellow>Unique attack!</color>");
                _currentShot = 0;
                var bullet = _uniqueFactory.Get();
                var randomAngle = Random.Range(-7, 7);
                bullet.transform.rotation = Quaternion.Euler(
                    new Vector3(0,0, _spawnPoint.localRotation.eulerAngles.z + randomAngle));
            }
        }
    }
}