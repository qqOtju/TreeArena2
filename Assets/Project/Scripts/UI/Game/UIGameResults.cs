using System;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Wave;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(Canvas))]
    public class UIGameResults: MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _infinityButton;
        [SerializeField] private WaveController _waveController;
        [SerializeField] private Tree _tree;

        private Canvas _canvas;
        
        private void Awake()
        {
            Time.timeScale = 1;
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            _infinityButton.onClick.AddListener(OnInfinityButtonClick);
            _waveController.OnAllWavesEnd += OnAllWavesEndHandler;
            _tree.OnHealthChange += OnTreeHealthChange;
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
            _infinityButton.onClick.RemoveListener(OnInfinityButtonClick);
            _waveController.OnAllWavesEnd -= OnAllWavesEndHandler;
        }

        private void OnRestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
        private void OnMainMenuButtonClick()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        private void OnInfinityButtonClick()
        {
            _canvas.enabled = false;
            _waveController.StartInfinityWaves();
        }

        private void OnAllWavesEndHandler()
        {
            _canvas.enabled = true;
        }

        private void OnTreeHealthChange(OnHealthChangeArgs obj)
        {
            if(obj.Type == HeathChangeType.Death)
            {
                Time.timeScale = 0;
                _canvas.enabled = true;
            }
        }
    }
}