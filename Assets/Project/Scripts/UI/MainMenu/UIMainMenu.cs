using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.MainMenu
{
    public class UIMainMenu: MonoBehaviour
    {
        [Title("Button")]
        [SerializeField] private Button _wispsButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _exitButton;
        [Title("Menu")]
        [SerializeField] private UIPanel _wispMenu;
        [SerializeField] private UIPanel _optionsMenu;

        private void Awake()
        {
            _wispsButton.onClick.AddListener(OnWispsButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDestroy()
        {
            _wispsButton.onClick.RemoveListener(OnWispsButtonClicked);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnWispsButtonClicked()
        {
            _wispMenu.Open();
        }

        private void OnOptionsButtonClicked()
        {
            // _optionsMenu.Open();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
#if Unity_Editor
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}