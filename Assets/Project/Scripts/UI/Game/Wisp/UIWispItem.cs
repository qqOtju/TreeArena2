using System;
using Project.Scripts.Config.Item.WispShop;
using Project.Scripts.DesignPattern.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game.Wisp
{
    public class UIWispItem: MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemDescription;
        [SerializeField] private TMP_Text _itemCost;
        [SerializeField] private TMP_Text _itemRarity;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Transform _container;
        [SerializeField] private UIWispItemStat _statsPrefab;
        
        private MonoBehaviourPool<UIWispItemStat> _pool;
        private WispItem _wispItem;

        public event Action<WispItem> OnItemBuy;

        private void Start()
        {
            _pool = new MonoBehaviourPool<UIWispItemStat>(_statsPrefab, _container);
            _pool.Initialize(5);
        }

        private void SetStats(WispItem wispItem)
        {
            if(wispItem.Damage != 0)
                SetStat(WispItemStatType.Damage, $"{wispItem.Damage:F0}");
            if(wispItem.AttackSpeed != 0)
                SetStat(WispItemStatType.AttackSpeed, $"{wispItem.AttackSpeed:F0}");
            if(wispItem.CriticalChance != 0)
                SetStat(WispItemStatType.CriticalChance, $"{wispItem.CriticalChance:F0}");
            if(wispItem.CriticalDamage != 0)
                SetStat(WispItemStatType.CriticalDamage, $"{wispItem.CriticalDamage:F0}");
            if(wispItem.Piercing != 0)
                SetStat(WispItemStatType.Piercing, $"{wispItem.Piercing:F0}"); 
            if(wispItem.BonusEliteDamage != 0)
                SetStat(WispItemStatType.BonusEliteDamage, $"{wispItem.BonusEliteDamage:F0}");
            if(wispItem.BonusBossDamage != 0)
                SetStat(WispItemStatType.BonusBossDamage, $"{wispItem.BonusBossDamage:F0}");
            if(wispItem.EnemyDamage != 0)
                SetStat(WispItemStatType.EnemyDamage, $"{wispItem.EnemyDamage:F0}");
            if(wispItem.EnemyMaxHealth != 0)
                SetStat(WispItemStatType.EnemyMaxHealth, $"{wispItem.EnemyMaxHealth:F0}");
            if(wispItem.EnemyMoveSpeed != 0)
                SetStat(WispItemStatType.EnemyMoveSpeed, $"{wispItem.EnemyMoveSpeed:F0}");
            if(wispItem.EnemyAttackSpeed != 0)
                SetStat(WispItemStatType.EnemyAttackSpeed, $"{wispItem.EnemyAttackSpeed:F0}");
            if(wispItem.EnemyAttackRange != 0)
                SetStat(WispItemStatType.EnemyAttackRange, $"{wispItem.EnemyAttackRange:F0}");
        }

        private void SetStat(WispItemStatType type, string value)
        {
            var stat = _pool.Get();
            stat.Initialize(type, value);
        }

        public void SetItem(WispItem wispItem)
        {
            _wispItem = wispItem;
            _itemName.text = wispItem.ItemName;
            _itemDescription.text = wispItem.Description;
            _itemCost.text = wispItem.Price.ToString();
            _itemRarity.text = wispItem.Rarity.ToString();
            _icon.sprite = wispItem.Icon;
            SetStats(wispItem);
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(BuyItem);
            Canvas.ForceUpdateCanvases();
        }

        private void BuyItem()
        {
            OnItemBuy?.Invoke(_wispItem);
        }
    }
}