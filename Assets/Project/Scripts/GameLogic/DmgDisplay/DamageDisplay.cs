using System;
using System.Collections;
using Project.Scripts.DesignPattern.Pool;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.Module.Spawner;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.DmgDisplay
{
    public class DamageDisplay: MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private TMP_Text _textPrefab;
        [SerializeField] private float _time = 0.7f;
        [SerializeField] private Color _damageFromEnemy;
        [SerializeField] private Color _damageToEnemy;
        [SerializeField] private Color _damageToEnemyCritical;
        [SerializeField] private Color _heal;

        private ObjectPool<TMP_Text> _textPool;
        private Transform _container;
        
        private void Awake()
        {
            _enemySpawner.OnEnemySpawn += OnEnemySpawn;
            _enemySpawner.OnEnemyDeath += OnEnemyDeath;
        }

        private void Start()
        {
            _container = transform;
            _textPool = new ObjectPool<TMP_Text>(CreateText, OnGetText, OnReleaseText);
            _textPool.InitializePool(10);
        }

        private void OnDestroy()
        {
            _enemySpawner.OnEnemySpawn -= OnEnemySpawn;
            _enemySpawner.OnEnemyDeath -= OnEnemyDeath;
        }

        private void OnEnemySpawn(EnemyBase obj)
        {
            obj.OnDealDamage += OnDamageDealt;
            obj.OnHealthChange += OnDamageDealt;
        }

        private void OnEnemyDeath(EnemyBase obj)
        {
            obj.OnDealDamage -= OnDamageDealt;
            obj.OnHealthChange -= OnDamageDealt;
        }

        private TMP_Text CreateText()
        {
            var text = Instantiate(_textPrefab, _container);
            text.gameObject.SetActive(false);
            return text;
        }
        
        private void OnGetText(TMP_Text text)
        {
            text.gameObject.SetActive(true);
        }
        
        private void OnReleaseText(TMP_Text text)
        {
            text.gameObject.SetActive(false);
            // LeanTween.cancel(text.gameObject);
        }

        private void OnDamageDealt(EnemyBase arg1, IHealth arg2)
        {
            var damage = arg2.LastHealthChangeArgs.CurrentHealth - arg2.LastHealthChangeArgs.PreviousHealth;
            damage = Mathf.Abs(damage);
            OnDamageDealt(arg2.GO.transform.position, damage, DamageType.DamageToTree);
        }

        private void OnDamageDealt(OnHealthChangeArgs obj)
        {
            var damage = obj.CurrentHealth - obj.PreviousHealth;
            if(damage > 0)
                OnDamageDealt(obj.Object.transform.position, damage, DamageType.Heal);
            else
                OnDamageDealt(obj.Object.transform.position, Mathf.Abs(damage), DamageType.Damage);
        }

        private void OnDamageDealt(Vector3 pos, float value, DamageType damageType)
        {
            var range = 0.55f;
            var randomX = Random.Range(-range, range);
            var randomY = Random.Range(-range, range);
            var random = new Vector3(randomX, randomY, 0);
            var newPos = pos + random;
            var text = _textPool.Get();
            text.transform.position = newPos;
            text.text = $"{value:F0}";
            switch (damageType)
            {
                case DamageType.Damage:
                    text.color = _damageToEnemy;
                    break;;
                case DamageType.CriticalDamage:
                    text.color = _damageToEnemyCritical;
                    break;
                case DamageType.DamageToTree:
                    text.color = _damageFromEnemy;
                    break;
                case DamageType.Heal:
                    text.color = _heal;        
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
            }
            StartCoroutine(ReleaseText(text, random.normalized));
        }

        private IEnumerator ReleaseText(TMP_Text text, Vector3 direction)
        {
            var currentTime = 0f;
            var tr = text.gameObject.transform;
            while (currentTime < _time/2)
            {
                currentTime += Time.deltaTime;
                tr.position += direction * Time.deltaTime;
                yield return null;
            }
            while (currentTime < _time)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            _textPool.Release(text);
        }

        private enum DamageType
        {
            Damage,
            CriticalDamage,
            //ToDo: Its not used right
            DamageToTree,
            Heal
        }
    }
    
}