using System;
using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public struct BulletActionsArgs
    {
        public Action<Bullet, IEnemyHealth> OnEnemyHit { get; }
        public Action<Bullet> OnWallHit { get; }
        public Action<Bullet> MoveForward { get; }
        public int Piercing { get; }

        public BulletActionsArgs(Action<Bullet, IEnemyHealth> onEnemyHit, 
            Action<Bullet> onWallHit, Action<Bullet> moveForward, int piercing)
        {
            OnEnemyHit = onEnemyHit;
            OnWallHit = onWallHit;
            MoveForward = moveForward;
            Piercing = piercing;
        }
    }
}