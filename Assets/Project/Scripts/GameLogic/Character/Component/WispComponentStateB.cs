using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using Project.Scripts.Module.Stats.Wisp;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Component
{
    public class WispComponentStateB: WispComponentStandard
    {
        public WispComponentStateB(BulletFactory bulletFactory, Transform bulletSpawnPoint, WispStats wispStats) : base(bulletFactory, bulletSpawnPoint, wispStats)
        {
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, WispStats.Piercing);
            BulletFactory.SetActions(args);
            BulletFactory.SetConfigBulletFunc(ConfigBullet);
        }
    }
}