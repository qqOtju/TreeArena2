using System;
using Project.Scripts.Config.Coin;
using Project.Scripts.Debug;
using Project.Scripts.Module.System;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Dropdown
{
    public class Coin: MonoBehaviour
    {
        private const int MaxHits = 20;
        private const string PlayerLayer = "Player";
        
        private readonly Collider2D[] _results = new Collider2D[MaxHits];
        private ContactFilter2D _filter;
        private CoinSystem _coinSystem;
        private Transform _transform;
        private float _pickupRange;
        private Transform _target;
        private int _value;
        
        public event Action<Coin> OnPlayerHit;

        [Inject]
        private void Construct(CoinSystem coinSystem)
        {
            _coinSystem = coinSystem;
        }
        
        public void Initialize(CoinValue config, Transform target)
        {
            _pickupRange = .5f;
            _value = config.Value;
            _target = target;
        }
        
        private void Start()
        {
            _transform = transform;
            _filter = new ContactFilter2D
            {
                layerMask = LayerMask.GetMask(PlayerLayer),
                useLayerMask = true,
            };
        }

        private void Update()
        {
            var size = Physics2D.OverlapCircle(_transform.position,
                _pickupRange, _filter, _results);
            foreach (var coll in _results.AsSpan(0,size))
            {
                if(coll == null || coll.gameObject == gameObject ||  !coll.CompareTag("Player")) continue;   
                Collect();
                return;
            }
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            _transform.position = Vector2.MoveTowards(_transform.position, _target.position, 5 * Time.deltaTime);
        }
        
        private void Collect()
        {
            OnPlayerHit?.Invoke(this);
            _coinSystem.CurrentGold += _value;
            DebugSystem.Instance.Log(LogType.Coin, $"Player picked up coin. Current gold: {_coinSystem.CurrentGold}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _pickupRange);
        }
    }
}