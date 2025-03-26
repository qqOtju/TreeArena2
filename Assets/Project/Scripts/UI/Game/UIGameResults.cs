using System;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Wave;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LogType = Project.Scripts.Debug.LogType;

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
        //ToDo Remove GameLogic.Character.Tree i need just Tree
        [SerializeField] private GameLogic.Character.Tree _tree;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _canvas.enabled = !_canvas.enabled;
                Time.timeScale = _canvas.enabled ? 0 : 1;
            }
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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
                DebugSystem.Instance.Log(LogType.Wave, "Tree is <color=red>dead</color>! Game over!");
                Time.timeScale = 0;
                _canvas.enabled = true;
            }
        }
    }
}