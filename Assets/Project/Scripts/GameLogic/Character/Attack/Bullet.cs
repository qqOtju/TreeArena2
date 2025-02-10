using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Attack
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class Bullet: MonoBehaviour
    {
        protected const float BaseDistance = 50f;

        protected BulletActionsArgs Args;
        protected Transform BulletTr;

        public int CurrentPiercing { get; protected set; }
        public float CurrenDistance { get; protected set; }
        public Rigidbody2D Rb { get; private set; }

        protected virtual void Awake()
        {
            BulletTr = transform;
        }
        
        protected virtual void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        public virtual void Init(BulletActionsArgs bulletActionsArgs)
        {
            Args = bulletActionsArgs;
            CurrentPiercing = Args.Piercing;
            CurrenDistance = BaseDistance;
        }
    }
}