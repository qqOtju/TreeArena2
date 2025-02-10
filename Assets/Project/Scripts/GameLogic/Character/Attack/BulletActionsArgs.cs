using System;
using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    public struct BulletActionsArgs
    {
        public Action<Bullet, IHealth> OnHealthHit { get; }
        public Action<Bullet> OnWallHit { get; }
        public Action<Bullet> MoveForward { get; }
        public int Piercing { get; }

        public BulletActionsArgs(Action<Bullet, IHealth> onHealthHit, 
            Action<Bullet> onWallHit, Action<Bullet> moveForward, int piercing)
        {
            OnHealthHit = onHealthHit;
            OnWallHit = onWallHit;
            MoveForward = moveForward;
            Piercing = piercing;
        }
    }
}