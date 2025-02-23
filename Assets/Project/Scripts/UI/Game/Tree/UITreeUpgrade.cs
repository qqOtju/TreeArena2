using System;
using Project.Scripts.Config.Item.Tree;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.ItemManager;
using Project.Scripts.Module.Stats.Tree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.Game.Tree
{
    [RequireComponent(typeof(Canvas))]
    public class UITreeUpgrade: MonoBehaviour
    {
        [SerializeField] private WaveController _waveController;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TMP_Text _resetCounterText;
        [SerializeField] private UITreeItem[] _uiTreeItems;
        [SerializeField] private TMP_Text[] _currentTreeStats;
        
        //ToDo: make tmp_text counter
        private const int ResetCount = 1;
        
        private TreeItemManager _treeItemManager;
        private TreeBonuses _treeBonuses;
        private TreeStats _treeStats;
        private int _resetCounter;
        private Canvas _canvas;
        
        public event Action OnClose;

        [Inject]
        private void Construct(TreeItemManager treeItemManager, TreeBonuses treeBonuses, TreeStats treeStats)
        {
            _treeItemManager = treeItemManager;
            _treeBonuses = treeBonuses;
            _treeStats = treeStats;
        }
        
        private void Awake()
        {
            _resetButton.onClick.AddListener(UseReset);
            _waveController.OnWaveEnd += OnWaveEndHandler;
            foreach (var uiTreeItem in _uiTreeItems)
                uiTreeItem.OnItemChoose += OnItemChooseHandler;
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            for (int i = 0; i < _uiTreeItems.Length; i++)
            {
                var treeItem = _treeItemManager.TreeItems[i];
                _uiTreeItems[i].SetItem(treeItem);
            }
        }

        private void OnDestroy()
        {
            _resetButton.onClick.RemoveListener(UseReset);
            _waveController.OnWaveEnd -= OnWaveEndHandler;
            foreach (var uiTreeItem in _uiTreeItems)
                uiTreeItem.OnItemChoose -= OnItemChooseHandler;
        }

        private void UseReset()
        {
            if (_resetCounter > 0)
            {
                _resetCounter--;
                _resetCounterText.text = $"{_resetCounter}/{ResetCount}";
                ResetItems();
            }
        }

        private void OnWaveEndHandler(int obj)
        {
            _canvas.enabled = true;
            _resetCounter = ResetCount;
            _resetCounterText.text = $"{_resetCounter}/{ResetCount}";
            _currentTreeStats[0].text = $"{_treeStats.MaxHealth}";
            _currentTreeStats[1].text = $"{_treeStats.Regen}";
            _currentTreeStats[2].text = $"{_treeStats.Armor}";
            _currentTreeStats[3].text = $"{_treeStats.Absorption}";
            ResetItems();
        }

        private void ResetItems()
        {
            foreach (var uiTreeItem in _uiTreeItems)
                uiTreeItem.SetItem(_treeItemManager.GetRandomItem());
        }

        private void OnItemChooseHandler(TreeItem obj)
        {
            //ToDo: Check
            if(obj.Rarity == TreeItemRarity.Unique)
                _treeItemManager.RemoveItem(obj);
            _treeBonuses.ApplyItemBonuses(obj);
            _canvas.enabled = false;
            OnClose?.Invoke();
        }
    }
}