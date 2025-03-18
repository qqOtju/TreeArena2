using Project.Scripts.Config.Wisp;
using Project.Scripts.GameLogic.GameCycle;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.MainMenu
{
    public class UIWispMenu: UIPanel
    {
        [Title("Wisp Data")]
        [SerializeField] private WispData[] _wispData;
        [SerializeField] private Button[] _wispButtons;
        [Title("Button")]
        [SerializeField] private Button _chooseButton;
        [SerializeField] private Button _closeButton;
        [Title("Wisp Info")]
        [SerializeField] private Image _wispIcon;
        [SerializeField] private TMP_Text _wispName;
        [SerializeField] private TMP_Text _wispDescription;
        [Title("Wisp Stat")]
        [SerializeField] private TMP_Text _wispDamage;
        [SerializeField] private TMP_Text _wispAttackSpeed;
        [SerializeField] private TMP_Text _wispCriticalChance;
        [SerializeField] private TMP_Text _wispCriticalDamage;
        [SerializeField] private TMP_Text _wispPiercing;
        [SerializeField] private TMP_Text _wispBonusEliteDamage;
        [SerializeField] private TMP_Text _wispBonusBossDamage;
        
        private GameData _gameData;
        private Button _chosenWispButton;
        
        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        private void Awake()
        {
            _chooseButton.onClick.AddListener(Choose);
            _closeButton.onClick.AddListener(Close);
        }

        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < _wispData.Length; i++)
            {
                var index = i;
                _wispButtons[i].GetComponentInChildren<TMP_Text>().text = _wispData[i].Name;
                _wispButtons[i].onClick.AddListener(() => OnWispButtonClicked(index));
            }
            OnWispButtonClicked(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(Close);
            _chooseButton.onClick.RemoveListener(Choose);
        }

        private void Choose()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        private void OnWispButtonClicked(int i)
        {
            if(_chosenWispButton != null)
                _chosenWispButton.interactable = true;
            _chosenWispButton = _wispButtons[i];
            _chosenWispButton.interactable = false;
            _gameData.ChosenWisp = _wispData[i];
            _wispIcon.sprite = _wispData[i].Icon;
            _wispName.text = _wispData[i].Name;
            _wispDescription.text = _wispData[i].Description;
            InitStats(_wispData[i]);
        }

        private void InitStats(WispData wispData)
        {
            _wispDamage.text = $"{wispData.WispConfig.Damage}";
            _wispAttackSpeed.text = $"{wispData.WispConfig.AttackSpeed}";
            _wispCriticalChance.text = $"{wispData.WispConfig.CriticalChance}";
            _wispCriticalDamage.text = $"{wispData.WispConfig.CriticalDamage}";
            _wispPiercing.text = $"{wispData.WispConfig.Piercing}";
            _wispBonusEliteDamage.text = $"{wispData.WispConfig.BonusEliteDamage}";
            _wispBonusBossDamage.text = $"{wispData.WispConfig.BonusBossDamage}";
        }
    }
}