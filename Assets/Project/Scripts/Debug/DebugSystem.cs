using System;
using System.Collections.Generic;
using Project.Scripts.DesignPattern.Pool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Debug
{
    //ToDo: замінити спавн окремих повідомлень на один текст з усіма повідомленнями
    public class DebugSystem: MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private bool _logToConsole;
        [SerializeField] private bool _scrollToBottom;
        [Title("Window")]
        [SerializeField] private RectTransform _windowRect;
        [Title("Type")]
        [SerializeField] private Button _typeButtonPrefab;
        [SerializeField] private Transform _typeButtonParent;
        [Title("Log")]
        [SerializeField] private TMP_Text _logTextPrefab;
        [SerializeField] private Transform _logTextParent;
        [SerializeField] private ScrollRect _scrollRect;
        [Title("Buttons")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _logToFileButton;
        [SerializeField] private Button _toggleWindowSizeButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _openButton;
        
        private const string LogFileName = "log.txt";
        
        private Vector2 _originalAnchoredPosition;
        private Vector2 _originalSizeDelta;
        private Vector2 _originalMinAnchor;
        private Vector2 _originalMaxAnchor;
        private bool _isMaximized = false;
        private Dictionary<LogType, Log> _logs = new ();
        private List<TMP_Text> _logTexts = new ();
        private MonoBehaviourPool<TMP_Text> _logTextPool;
        private bool _isPaused;
        private LogType _currentLogType;

        public static DebugSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateLogTypes();
            OnLogButtonClicked(LogType.All);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _logToFileButton.onClick.AddListener(LogToFile);
            _toggleWindowSizeButton.onClick.AddListener(ToggleWindowSize);
            _closeButton.onClick.AddListener(Close);
            _openButton.onClick.AddListener(Open);
            
            _logTextPool = new MonoBehaviourPool<TMP_Text>(_logTextPrefab, _logTextParent);
            _logTextPool.Initialize(20);
            _originalAnchoredPosition = _windowRect.anchoredPosition;
            _originalSizeDelta = _windowRect.sizeDelta;
            _originalMinAnchor = _windowRect.anchorMin;
            _originalMaxAnchor = _windowRect.anchorMax;
        }

        private void OnDestroy()
        {
            Instance = null;
            _pauseButton.onClick.RemoveAllListeners();
            _logToFileButton.onClick.RemoveAllListeners();
            _toggleWindowSizeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            _openButton.onClick.RemoveAllListeners();
        }

        private void Close()
        {
            _openButton.gameObject.SetActive(true);
            _windowRect.gameObject.SetActive(false);
        }

        private void Open()
        {
            _openButton.gameObject.SetActive(false);
            _windowRect.gameObject.SetActive(true);
        }

        private void ToggleWindowSize()
        {
            if (_isMaximized)
                MinimizeWindow();
            else
                MaximizeWindow();
            _isMaximized = !_isMaximized;
        }

        private void MinimizeWindow()
        {
            _windowRect.anchorMin = _originalMinAnchor;
            _windowRect.anchorMax = _originalMaxAnchor;
            _windowRect.anchoredPosition = _originalAnchoredPosition;
            _windowRect.sizeDelta = _originalSizeDelta;
        }

        private void MaximizeWindow()
        {
            _windowRect.anchorMin = new Vector2(0, 0);
            _windowRect.anchorMax = new Vector2(1, 1);
            _windowRect.anchoredPosition = Vector2.zero;
            _windowRect.sizeDelta = Vector2.zero;
        }

        private void OnPauseButtonClicked()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
        }

        private void CreateLogTypes()
        {
            foreach (var logType in Enum.GetValues(typeof(LogType)))
            {
                _logs.Add((LogType)logType, new Log());
                var button = Instantiate(_typeButtonPrefab, _typeButtonParent);
                button.GetComponentInChildren<TMP_Text>().text = logType.ToString();
                button.onClick.AddListener(() => OnLogButtonClicked((LogType)logType));
            }
        }

        [Title("Actions")] [Button(ButtonSizes.Medium)]
        private void LogToFile()
        {
            var lines = new List<string>();
            foreach (var message in _logs[LogType.All].Messages)
            {
                var text = $"[{message.LogType}] [{message.Time:HH:mm:ss}] {message.Message}";
                lines.Add(text);
            }
            System.IO.File.WriteAllLines(LogFileName, lines);
        }

        private void OnLogButtonClicked(LogType logType)
        {
            _currentLogType = logType;
            _logTextPrefab.text = "";
            foreach (var text in _logTexts)
                _logTextPool.Release(text);
            _logTexts.Clear();
            foreach (var message in _logs[logType].Messages)
            {
                var logMessage = message;
                var text = CreateLogText(logMessage);
                _logTexts.Add(text);
            }
            if(_scrollToBottom)
                ScrollToBottom();
        }
        
        private TMP_Text CreateLogText(LogMessage message)
        {
            var text = _logTextPool.Get();
            text.text = $"[{message.Time:HH:mm:ss}] {message.Message}";
            text.transform.SetAsLastSibling();
            return text;
        }
        
        private void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();
            _scrollRect.verticalNormalizedPosition = 0f;
        }
        
        [Button(ButtonStyle.CompactBox, Expanded = true)]//
        public void Log(LogType logType, string message)
        {
            if(_logToConsole)
                UnityEngine.Debug.Log(message);
            _logs[logType].AddMessage(logType, message);
            if(logType != LogType.All)
                _logs[LogType.All].AddMessage(logType, message);
            if(logType == _currentLogType || _currentLogType == LogType.All)
            {
                var logMessage = _logs[logType].Messages[^1];
                var text = CreateLogText(logMessage);
                _logTexts.Add(text);
            }
            if(_scrollToBottom)
                ScrollToBottom();
        }
    }
}