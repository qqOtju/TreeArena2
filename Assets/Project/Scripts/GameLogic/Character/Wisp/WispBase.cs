using System;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Wisp
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class WispBase : MonoBehaviour, IWisp
    {
        [SerializeField] protected Transform _bulletSpawnPoint;
        [SerializeField] protected Transform _view;
        [SerializeField] protected float _moveSpeed;
        
        private const float AvoidanceDistance = 1.5f;
        private const float RotationSpeed = 2f;
        
        protected Transform FollowTarget;
        protected Transform Transform;
        protected Vector2 MoveDirection;
        protected Rigidbody2D Rb;
        protected Camera MainCamera;
        protected Transform BulletContainer;
        
        public Transform BulletSpawnPoint { get; protected set; }
        public BulletFactory BulletFactory { get; protected set; }
        
        protected virtual void Start()
        {
            Transform = transform;
            Rb = GetComponent<Rigidbody2D>();
            MainCamera = Camera.main;
            BulletSpawnPoint = _bulletSpawnPoint;
        }

        protected virtual void Update()
        {
            RotateBulletSpawnPoint();
            RotateView();
        }

        protected virtual void FixedUpdate()
        {
            Move();
            Rotate();
        }

        protected virtual void Move()
        {
            if (FollowTarget == null) return;
            var startPosition = transform.position;
            var endPosition = FollowTarget.position;
            var distance = Vector3.Distance(startPosition, endPosition);
            if (distance <= AvoidanceDistance) return;
            var speedFactor = Mathf.Clamp((distance - 1.5f), 0.2f, 1.5f); // Налаштуй 10f і межі за потребою
            var dynamicSpeed = _moveSpeed * speedFactor;
            var direction = (endPosition - startPosition).normalized;
            var moveForce = (Vector2)(startPosition + direction * (dynamicSpeed * Time.fixedDeltaTime));
            MoveDirection = moveForce - Rb.position;
            Rb.MovePosition(moveForce);
        }
        
        protected void Rotate()
        {
            var direction = (Vector2)FollowTarget.position - Rb.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var currentAngle = Rb.rotation;
            Rb.rotation = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed * Time.fixedDeltaTime);
        }

        protected void RotateBulletSpawnPoint()
        {
            var mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - _bulletSpawnPoint.position;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _bulletSpawnPoint.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }

        protected void RotateView()
        {
            _view.rotation = Quaternion.identity;
            if(MoveDirection.x > 0)
                _view.localScale = new Vector3(1, 1, 1);
            else if(MoveDirection.x < 0)
                _view.localScale = new Vector3(-1, 1, 1);
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AvoidanceDistance);
        }

        public virtual void Init(Transform followTarget, Transform bulletContainer)
        {
            FollowTarget = followTarget;
            BulletContainer = bulletContainer;
        }
        
        public abstract void AddDecorator(Type wispDecoratorType);
    }
}