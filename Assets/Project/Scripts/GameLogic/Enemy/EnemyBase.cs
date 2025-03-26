using System;
using Project.Scripts.Config.Enemy;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Movement;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;
using Random = UnityEngine.Random;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Enemy
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EnemyBase: EntityBase, IEnemyHealth
    {
        [SerializeField] private Transform _view;
        
        private const string TreeTagLayer = "Tree";
        
        private Collider2D[] _nearbyObjects = new Collider2D[20];
        private ContactFilter2D _contactFilter;
        private EnemyType _type;
        
        protected BasicMovement BasicMovement;
        protected Vector3 TargetPosition;
        protected EnemyValue EnemyValue;
        protected Vector2 MoveInput;
        protected float AttackTimer;
        protected Tree TargetTree;
        protected Rigidbody2D Rb;
        
        public EnemyType Type => _type;
        public EnemyValue Value => EnemyValue;
        
        public event Action<EnemyBase> OnDeath;
        public event Action<EnemyBase, IHealth> OnDealDamage;

        public virtual void Initialize(EnemyValue enemyStat, EnemyType enemyType, Tree tree)
        {
            EnemyValue = enemyStat;
            TargetTree = tree;
            _type = enemyType;
            var range = 0.8f;
            TargetPosition = tree.transform.position + new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
            SetInitialHealth(EnemyValue.MaxHealth);
            #if (DEVELOPMENT_BUILD || UNITY_EDITOR)
            DebugSystem.Instance.Log(LogType.Enemy, $"Enemy {gameObject.name} initialized with \n" +
                                                   $"MaxHealth: {EnemyValue.MaxHealth} \n" +
                                                   $"Speed: {EnemyValue.MoveSpeed} \n" +
                                                   $"Damage: {EnemyValue.Damage} \n" +
                                                   $"AttackSpeed: {EnemyValue.AttackSpeed} \n" +
                                                   $"AttackRange: {EnemyValue.AttackRange} \n");
            #endif
        }

        protected virtual void Awake()
        {
            OnHealthChange += CheckDeath;
        }

        protected virtual void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            BasicMovement = new BasicMovement(Rb);
            _contactFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = LayerMask.GetMask(TreeTagLayer)
            };
        }

        protected virtual void OnDestroy()
        {
            OnHealthChange -= CheckDeath;
        }

        protected virtual void Update()
        {
            CalculateMoveInput();
            AttackCycle();
            RotateView(MoveInput);
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            BasicMovement.Move(MoveInput, EnemyValue.MoveSpeed);
        }
        
        protected virtual void CalculateMoveInput()
        {
             if(IsNearbyTree())
                MoveInput = Vector2.zero;
             else
                MoveInput = (TargetPosition - transform.position).normalized;
        }

        protected virtual void AttackCycle()
        {
            if(!IsNearbyTree()) return;
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= EnemyValue.AttackSpeed)
            {
                Attack(TargetTree);
                AttackTimer = 0;
            }
        }

        protected bool IsNearbyTree()
        {
            //ToDo: Maybe change to the way it realized in EnemyChargerStateAttack.
            //if (Vector2.Distance(_transform.position, _tree.transform.position) < Range)
            //    Attack...
            Array.Clear(_nearbyObjects, 0, _nearbyObjects.Length);
            Physics2D.OverlapCircle(transform.position, 
                EnemyValue.AttackRange, _contactFilter, _nearbyObjects);
            foreach (var hit in _nearbyObjects)
                if (hit != null && hit.transform.CompareTag(TreeTagLayer))
                    return true;
            return false;
        }

        protected virtual void Attack(IHealth health)
        {
            health.TakeDamage(EnemyValue.Damage);
            OnDealDamage?.Invoke(this, health);
        }
        
        protected void RaiseOnDealDamage(IHealth health)
        {
            OnDealDamage?.Invoke(this, health);
        }

        private void CheckDeath(OnHealthChangeArgs obj)
        {
            if (obj.Type != HeathChangeType.Death) return;
            OnDeath?.Invoke(this);
        }

        protected void RotateView(Vector2 moveInput)
        {
            if(moveInput == Vector2.zero) return;
            if(moveInput.x > 0)
                _view.localScale = new Vector3(1, 1, 1);
            else if(moveInput.x < 0)
                _view.localScale = new Vector3(-1, 1, 1);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnemyValue.AttackRange);
        }
    }
}