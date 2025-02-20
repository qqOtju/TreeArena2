using Project.Scripts.Config.Wisp;
using Project.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.MainMenu
{
    public class UIMainMenu: MonoBehaviour
    {
        [SerializeField] private WispData[] _wispData;
        [SerializeField] private Button[] _wispButtons;

        private GameData _gameData;
        
        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
        }
        
        private void Start()
        {
            for (int i = 0; i < _wispButtons.Length; i++)
            {
                var index = i;
                _wispButtons[i].GetComponentInChildren<TMP_Text>().text = _wispData[i].Name;
                _wispButtons[i].onClick.AddListener(() => OnWispButtonClicked(index));
            }
        }

        private void OnWispButtonClicked(int i)
        {
            _gameData.ChosenWisp = _wispData[i];
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}