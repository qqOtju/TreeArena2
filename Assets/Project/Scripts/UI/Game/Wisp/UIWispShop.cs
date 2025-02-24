using System;
using Project.Scripts.Config.Item.WispShop;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.GameLogic.Character.Wisp;
using Project.Scripts.Module.ItemManager;
using Project.Scripts.Module.Stats.Enemy;
using Project.Scripts.Module.Stats.Wisp;
using Project.Scripts.UI.Game.Tree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Game.Wisp
{
    [RequireComponent(typeof(Canvas))]
    public class UIWispShop: MonoBehaviour
    {
        [SerializeField] private UITreeUpgrade _uiTreeUpgrade;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TMP_Text _coins;
        [SerializeField] private TMP_Text _resetCounterText;
        [SerializeField] private UIWispItem[] _uiWispItems;
        [SerializeField] private TMP_Text[] _currentWispStats;

        private const int ResetCount = 1;
        
        private WispItemManager _wispItemManager;
        private EnemyBonuses _enemyBonuses;
        private WispBonuses _wispBonuses;
        private WispStats _wispStats;
        private int _resetCounter;
        private Canvas _canvas;
        private IWisp _wisp;
        
        public event Action OnClose;
        
        [Inject]
        private void Construct(WispItemManager wispItemManager, WispStats wispStats, 
            WispBonuses wispBonuses, EnemyBonuses enemyBonuses, IWisp wisp)
        {
            _wispItemManager = wispItemManager;
            _enemyBonuses = enemyBonuses;
            _wispBonuses = wispBonuses;
            _wispStats = wispStats;
            _wisp = wisp;
        }
        
        private void Awake()
        {
            _uiTreeUpgrade.OnClose += Open;
            _closeButton.onClick.AddListener(Close);
            foreach (var item in _uiWispItems)
                item.OnItemBuy += (wispItem) => OnItemBuyHandler(wispItem, item);
            _resetButton.onClick.AddListener(UseReset);
        }

        private void Start()
        {
            _resetCounter = ResetCount;
            _canvas = GetComponent<Canvas>();
            SetCurrentWispStats();
        }
        
        private void OnDestroy()
        {
            _uiTreeUpgrade.OnClose -= Open;
            _closeButton.onClick.RemoveListener(Close);
            _resetButton.onClick.RemoveListener(UseReset);
        }

        private void UseReset()
        {
            if (_resetCounter <= 0) return;
            _resetCounter--;
            _resetCounterText.text = _resetCounter.ToString();
            foreach (var item in _uiWispItems)
                item.SetItem(_wispItemManager.GetRandomItem());
            SetCurrentWispStats();
        }

        private void OnItemBuyHandler(WispItem item, UIWispItem wispItem)
        {
            if(item.Rarity == WispShopItemRarity.Unique)
                _wispItemManager.RemoveItem(item);
            _wispBonuses.ApplyItemBonuses(item);
            //ToDo: Change maybe
            wispItem.SetItem(_wispItemManager.GetRandomItem());
            if(item.WispDecoratorType != null)
            {
                _wisp.AddDecorator(item.WispDecoratorType);
            }
        }

        private void Open()
        {
            _canvas.enabled = true;
            _resetCounter = ResetCount;
            SetCurrentWispStats();
            foreach (var item in _uiWispItems)
                item.SetItem(_wispItemManager.GetRandomItem());
        }

        private void Close()
        {
            _canvas.enabled = false;
            OnClose?.Invoke();
        }

        private void SetCurrentWispStats()
        {
            _currentWispStats[0].text = $"{_wispStats.Damage}";
            _currentWispStats[1].text = $"{_wispStats.AttackSpeed}";
            _currentWispStats[2].text = $"{_wispStats.CriticalChance}";
            _currentWispStats[3].text = $"{_wispStats.CriticalDamage}";
            _currentWispStats[4].text = $"{_wispStats.Piercing}";
            _currentWispStats[5].text = $"{_wispStats.BonusEliteDamage}";
            _currentWispStats[6].text = $"{_wispStats.BonusBossDamage}";
        }
    }
}