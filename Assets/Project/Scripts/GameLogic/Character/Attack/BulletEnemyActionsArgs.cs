using System;
using Project.Scripts.Entity;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public struct BulletEnemyActionsArgs
    {
        public Action<Bullet, IHealth> OnTreeHit { get; }
        public Action<Bullet> OnWallHit { get; }
        public Action<Bullet> MoveForward { get; }
        
        public BulletEnemyActionsArgs(Action<Bullet, IHealth> onTreeHit, 
            Action<Bullet> onWallHit, Action<Bullet> moveForward)
        {
            OnTreeHit = onTreeHit;
            OnWallHit = onWallHit;
            MoveForward = moveForward;
        }
    }
}