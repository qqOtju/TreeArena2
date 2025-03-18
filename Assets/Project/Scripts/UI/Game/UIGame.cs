
using System;
using Project.Scripts.Entity;
using Project.Scripts.Module.Stats.Tree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Game
{
    public class UIGame: MonoBehaviour
    {
        [SerializeField] private GameLogic.Character.Tree _tree;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _armorSlider;
        [SerializeField] private TMP_Text _healthText;

        private TreeStats _treeStats;
        
        [Inject]
        private void Construct(TreeStats treeStats)
        {
            _treeStats = treeStats;
        }
        
        private void Awake()
        {
            _tree.OnHealthChange += UpdateHealthSlider;
            _tree.OnArmorChange += UpdateArmorSlider;
        }

        private void OnDestroy()
        {
            _tree.OnHealthChange -= UpdateHealthSlider;
            _tree.OnArmorChange -= UpdateArmorSlider;
        }

        private void UpdateHealthSlider(OnHealthChangeArgs obj)
        {
            _healthSlider.value = _tree.CurrentHealth / _tree.MaxHealth;
            _healthText.text = $"{_tree.CurrentHealth:F0}/{_tree.MaxHealth:F0}";
        }

        private void UpdateArmorSlider(float obj)
        {
            _armorSlider.value = _tree.CurrentArmor / _treeStats.Armor;
        }
    }
}