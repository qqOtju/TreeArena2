using System;
using Project.Scripts.Config.Item.Tree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game.Tree
{
    public class UITreeItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _chooseButton;
        [SerializeField] private GameObject[] _stats;
        [SerializeField] private Image[] _statsIcons;
        [SerializeField] private TMP_Text[] _statsTexts;
        [SerializeField] private Sprite _healthIcon;
        [SerializeField] private Sprite _regenIcon;
        [SerializeField] private Sprite _armorIcon;
        [SerializeField] private Sprite _absorptionIcon;
        
        private TreeItem _treeItem;
        
        public event Action<TreeItem> OnItemChoose;

        private void Start()
        {
            foreach (var stat in _stats)
                stat.SetActive(false);
        }

        private void SetStats(TreeItem treeItem)
        {
            foreach (var stat in _stats)
                stat.SetActive(false);
            int index = 0;
            if (treeItem.MaxHealth != 0)
                SetMaxHealth(treeItem, index++);
            if (treeItem.Regen != 0)
                SetRegen(treeItem, index++);
            if (treeItem.Armor != 0)
                SetArmor(treeItem, index++);
            if (treeItem.Absorption != 0)
                SetAbsorption(treeItem, index);
        }
        
        private void SetMaxHealth(TreeItem treeItem, int index)
        {
            _stats[index].SetActive(true);
            _statsIcons[index].sprite = _healthIcon;
            var text = treeItem.MaxHealth > 0 ? $"+{treeItem.MaxHealth} HP" : $"{treeItem.MaxHealth} HP";
            _statsTexts[index].text = text;
        }
        
        private void SetRegen(TreeItem treeItem, int index)
        {
            _stats[index].SetActive(true);
            _statsIcons[index].sprite = _regenIcon;
            var text = treeItem.Regen > 0 ? $"+{treeItem.Regen} Regen" : $"{treeItem.Regen} Regen";
            _statsTexts[index].text = text;
        }
        
        private void SetArmor(TreeItem treeItem, int index)
        {
            _stats[index].SetActive(true);
            _statsIcons[index].sprite = _armorIcon;
            var text = treeItem.Armor > 0 ? $"+{treeItem.Armor} Armor" : $"{treeItem.Armor}Armor";
            _statsTexts[index].text = text;
        }
        
        private void SetAbsorption(TreeItem treeItem, int index)
        {
            _stats[index].SetActive(true);
            _statsIcons[index].sprite = _absorptionIcon;
            var text = treeItem.Absorption > 0 ? $"+{treeItem.Absorption} Abs" : $"{treeItem.Absorption} Abs";
            _statsTexts[index].text = text;
        }
        
        private void OnChooseButtonClick()
        {
            OnItemChoose?.Invoke(_treeItem);
        }

        public void SetItem(TreeItem treeItem)
        {
            _treeItem = treeItem;
            _itemName.text = treeItem.ItemName;
            _icon.sprite = treeItem.Icon;
            SetStats(treeItem);
            _chooseButton.onClick.RemoveAllListeners();
            _chooseButton.onClick.AddListener(OnChooseButtonClick);
        }
    }
}