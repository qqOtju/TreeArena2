using System;
using System.Collections.Generic;
using Project.Scripts.Config.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.MainMenu.LogBook
{
    public class UILogBook: UIPanel
    {
        [Header("Log")]
        [SerializeField] private Transform _logContainer;
        [SerializeField] private UILog _logPrefab;
        [SerializeField] private EnemyData[] _enemyData;
        [Header("Description")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _type;
        [SerializeField] private TMP_Text _description;
        [Header("Description/Stats")]
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _moveSpeed;
        [SerializeField] private TMP_Text _damage;
        [SerializeField] private TMP_Text _attackSpeed;
        [SerializeField] private TMP_Text _attackRange;
        [SerializeField] private TMP_Text _coinsDropCount;
        [Header("Description/Bonuses")]
        [SerializeField] private TMP_Text _healthPerWave;
        [SerializeField] private TMP_Text _moveSpeedPerWave;
        [SerializeField] private TMP_Text _damagePerWave;
        [SerializeField] private TMP_Text _attackSpeedPerWave;
        [SerializeField] private TMP_Text _attackRangePerWave;
        [Header("Other")]
        [SerializeField] private Button _closeButton;

        private List<UILog> _logs = new ();
        
        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
        }

        protected override void Start()
        {
            base.Start();
            foreach (var enemyData in _enemyData)
            {
                var log = Instantiate(_logPrefab, _logContainer);
                log.Initialize(enemyData.Icon, enemyData.Name, enemyData.EnemyType.ToString());
                log.ChooseButton.onClick.AddListener( () => SetDescription(enemyData));
                _logs.Add(log);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var log in _logs)
            {
                log.ChooseButton.onClick.RemoveAllListeners();
            }
        }

        private void SetDescription(EnemyData enemyData)
        {
            _icon.sprite = enemyData.Icon;
            _name.text = enemyData.Name;
            _type.text = enemyData.EnemyType.ToString();
            _description.text = enemyData.Description;
            var enemyValue = enemyData.EnemyConfig.Value;
            _health.text = enemyValue.MaxHealth.ToString();
            _moveSpeed.text = enemyValue.MoveSpeed.ToString();
            _damage.text = enemyValue.Damage.ToString();
            _attackSpeed.text = enemyValue.AttackSpeed.ToString();
            _attackRange.text = enemyValue.AttackRange.ToString();
            _coinsDropCount.text = enemyValue.CoinsDropCount.ToString();
            _healthPerWave.text = enemyValue.MaxHealthPerWave.ToString();
            _moveSpeedPerWave.text = enemyValue.MoveSpeedPerWave.ToString();
            _damagePerWave.text = enemyValue.DamagePerWave.ToString();
            _attackSpeedPerWave.text = enemyValue.AttackSpeedPerWave.ToString();
            _attackRangePerWave.text = enemyValue.AttackRangePerWave.ToString();
        }
    }
}