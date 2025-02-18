using System;
using System.Collections;
using Project.Scripts.Config.Wave;
using Project.Scripts.Debug;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Spawner;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.Wave
{
    public class WaveController: MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private WaveConfig[] _waveConfigs;
        
        private EnemyFactory _enemyFactory;
        private int _currentWaveIndex;
        private bool _allEnemiesSpawned;
        private bool _infinityWaves;

        public event Action<int> OnWaveStart;
        public event Action<int> OnWaveEnd;
        public event Action OnAllWavesEnd;

        [Inject]
        private void Construct(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        private void Awake()
        {
            _enemySpawner.OnAllEnemiesDead += OnAllEnemiesDead;
            OnWaveEnd += OnWaveEndHandler;
        }

        private void Start()
        {
            StartNextWave();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                StartNextWave();
        }

        private void OnDestroy()
        {
            _enemySpawner.OnAllEnemiesDead -= OnAllEnemiesDead;
            OnWaveEnd -= OnWaveEndHandler;
        }

        private void StartNextWave()
        {
            WaveConfig waveConfig = null;
            if (_currentWaveIndex < _waveConfigs.Length)
                waveConfig = _waveConfigs[_currentWaveIndex];
            else if(_infinityWaves)
                waveConfig = _waveConfigs[Random.Range(0, _waveConfigs.Length)];
            else
                return;
            OnWaveStart?.Invoke(_currentWaveIndex);
            DebugSystem.Instance.Log(LogType.Wave, $"Wave {_currentWaveIndex} started");
            _currentWaveIndex++;
            StartWave(waveConfig);
        }

        private void StartWave(WaveConfig waveConfig)
        {
            _allEnemiesSpawned = false;
            foreach (var spawn in waveConfig.Spawns)
            {
                StartCoroutine(StartWave(spawn));
            }
            //ToDo: check if all enemies spawned, because coroutine is async
            _allEnemiesSpawned = true;
        }
        
        private IEnumerator StartWave(WaveContent wave)
        {
            yield return new WaitForSeconds(wave.WaveDelay);
            for (var i = 0; i < wave.EnemyCount; i++)
            {
                yield return new WaitForSeconds(wave.SpawnDelay);
                _enemySpawner.SpawnEnemy(wave.SpawnPoint, wave.EnemyData);
            }
        }

        private void OnAllEnemiesDead()
        {
            if (!_allEnemiesSpawned) return;
            if (_currentWaveIndex >= _waveConfigs.Length && !_infinityWaves)
            {
                OnAllWavesEnd?.Invoke();
                DebugSystem.Instance.Log(LogType.Wave, "<color=green>All waves ended!</color>");
            }
            else
            {
                OnWaveEnd?.Invoke(_currentWaveIndex - 1);
                DebugSystem.Instance.Log(LogType.Wave, $"Wave <color=red>{_currentWaveIndex - 1}</color> ended");
            }
        }

        private void OnWaveEndHandler(int obj)
        {
            _enemyFactory.SetWaveIndex(obj);
        }
        
        public void StartInfinityWaves()
        {
            _infinityWaves = true;
            OnWaveEnd?.Invoke(_currentWaveIndex - 1);
            DebugSystem.Instance.Log(LogType.Wave, "<color=yellow>Infinity waves started</color>");
        }
    }
}