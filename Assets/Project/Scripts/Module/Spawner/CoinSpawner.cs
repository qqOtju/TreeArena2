using System;
using System.Collections.Generic;
using Project.Scripts.Config.Coin;
using Project.Scripts.GameLogic.Character;
using Project.Scripts.GameLogic.Dropdown;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.Factory;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.Module.Spawner
{
    public class CoinSpawner: MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private WaveController _waveController;
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private CoinConfig _coinConfig;
        [SerializeField] private Transform _coinContainer;
        [SerializeField] private Player _player;
        
        private const float RandomSpawnRange = 0.5f;

        private List<Coin> _activeCoins = new ();
        private CoinFactory _coinFactory;
        private DiContainer _diContainer;
        
        public event Action<Coin> OnCoinSpawn;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        private void Awake()
        {
            _enemySpawner.OnEnemyDeath += OnEnemyDeath;
        }

        private void Start()
        {
            _coinFactory = new CoinFactory(_coinPrefab, _coinContainer, 
                _diContainer, _coinConfig, _player.transform);  
        } 

        private void OnDestroy()
        {
            _enemySpawner.OnEnemyDeath -= OnEnemyDeath;
            foreach (var coin in _activeCoins)
                coin.OnPlayerHit -= OnPlayerHit;
        }

        private void OnEnemyDeath(EnemyBase obj)
        {
            for (var i = 0; i < obj.Value.CoinsDropCount; i++)
            {
                var coin = _coinFactory.Get();
                coin.transform.position = GetRandomSpawnPos(obj.transform.position, RandomSpawnRange);
                coin.OnPlayerHit += OnPlayerHit;
                _activeCoins.Add(coin);
                OnCoinSpawn?.Invoke(coin);
            }
        }

        private void OnPlayerHit(Coin obj)
        {
            obj.OnPlayerHit -= OnPlayerHit;
            _activeCoins.Remove(obj);
        }

        private Vector3 GetRandomSpawnPos(Vector3 position, float range)
        {
            var randomPos = new Vector3(Random.Range(-range, range), 
                Random.Range(-range, range), 0);
            return position + randomPos;
        }
    }
}