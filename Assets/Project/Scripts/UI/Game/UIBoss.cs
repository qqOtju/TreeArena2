using System;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game
{
    public class UIBoss: MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private TMP_Text _healthText;

        private EnemyBase _enemyBase;
        
        public void Initialize(EnemyBase enemyBase)
        {
            _enemyBase = enemyBase;
            _enemyBase.OnHealthChange += UpdateHealthBar;
            _healthText.text = $"{_enemyBase.CurrentHealth:F0}/{_enemyBase.MaxHealth:F0}";
            _healthBar.value = _enemyBase.CurrentHealth / _enemyBase.MaxHealth;
        }

        private void OnDestroy()
        {
            _enemyBase.OnHealthChange -= UpdateHealthBar;
        }

        private void UpdateHealthBar(OnHealthChangeArgs obj)
        {
            _healthBar.value = _enemyBase.CurrentHealth / _enemyBase.MaxHealth;
            _healthText.text = $"{_enemyBase.CurrentHealth:F0}/{_enemyBase.MaxHealth:F0}";
        }
    }
}