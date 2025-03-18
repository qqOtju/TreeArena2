using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public abstract class UIPanel: MonoBehaviour
    {
        private const float AnimationDuration = 0.5f;
        
        private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        
        private Coroutine _openCoroutine;
        private Coroutine _closeCoroutine;
        
        protected CanvasGroup CanvasGroup;
        protected Canvas Canvas;

        protected virtual void Start()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            Canvas = GetComponent<Canvas>();
        }

        protected virtual void OnDestroy()
        {
            if (_openCoroutine != null)
                StopCoroutine(_openCoroutine);
            if (_closeCoroutine != null)
                StopCoroutine(_closeCoroutine);
        }

        private IEnumerator Fade(float start, float end, float duration, Action onComplete = null)
        {
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                CanvasGroup.alpha = Mathf.Lerp(start, end, time / duration);
                yield return _waitForFixedUpdate;
            }
            CanvasGroup.alpha = end;
            onComplete?.Invoke();
        }

        public void Open()
        {
            Canvas.enabled = true;
            if (_closeCoroutine != null)
            {
                StopCoroutine(_closeCoroutine);
            }
            if(_openCoroutine != null)
            {
                StopCoroutine(_openCoroutine);
            }
            _openCoroutine = StartCoroutine(Fade(0, 1, AnimationDuration));
        }

        public void Close()
        {
            if (_openCoroutine != null)
                StopCoroutine(_openCoroutine);
            if(_closeCoroutine != null)
                StopCoroutine(_closeCoroutine);
            _closeCoroutine = StartCoroutine(Fade(1, 0, AnimationDuration, () => Canvas.enabled = false));
        }
    }
}