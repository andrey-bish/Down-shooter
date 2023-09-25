using System;
using System.Collections;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.ProgressBars
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField, GroupComponent] private Image _fill;

		[SerializeField, GroupComponent(false)] private RectTransform _icon;

		[SerializeField, GroupComponent] private CanvasGroup     _canvasGroup;
		[SerializeField, GroupComponent] private TextMeshProUGUI _progress;

		[SerializeField, GroupSetting] private bool _hideIdle;
		[SerializeField, GroupSetting] private bool _isTower;

		[SerializeField, GroupSetting, Indent, ShowIf(nameof(_hideIdle))] private float _hideDelay;
		[SerializeField, GroupSetting, HideIf(nameof(_hideIdle))]         private bool  _hideEmpty;

		[SerializeField, GroupSetting] private float _switchDuration = 0.2f;
		[SerializeField, GroupSetting] private float _fillDuration   = 0.2f;

		[SerializeField, GroupSetting, OnValueChanged(nameof(SetColor))] private Color _fillColor = Color.white;

		private float     _maxValue = 1;
		private float     _currentValue;
		private Coroutine _showCor;
		private Tweener   _fillTweener;
		public void Show(float duration) => _canvasGroup.DOFade(1, duration);
		public void Hide(float duration) => _canvasGroup.DOFade(0, duration);
		private float _halfWidth;
		private float _minIconX;
		private float _maxIconX;

		private void Awake()
		{
			SetColor();
			_fill.fillAmount = 0;
			if (_progress != default) _progress.text = "";

			InitWidth();
		}

		private void LateUpdate()
		{
			if (_icon == default) return;

			var xPos = Mathf.Lerp(-_halfWidth, _halfWidth, _fill.fillAmount);
			_icon.anchoredPosition = Vector2.right * xPos;
		}

		private void InitWidth()
		{
			_halfWidth = _fill.rectTransform.rect.width / 2;
		}

		public void Switch(bool enable)
		{
			if (enable)
				Show(_switchDuration);
			else
				Hide(_switchDuration);
		}

		public void SetMaxValue(float maxValue, bool force, bool isInit = false)
		{
			_maxValue = maxValue;
			if (isInit) return;
			UpdateBar(force);
		}

		public void SetValue(float value, bool force = false, bool isInit = false)
		{
			_currentValue = value;
			if (isInit)
			{
				_fill.fillAmount = _currentValue / _maxValue;
				Hide(0.0f);
				return;
			}
			UpdateBar(force);
		}

		protected void ResetHp()
		{
			if (_currentValue < _maxValue)
			{
				_currentValue = _maxValue;
				UpdateBar();
			}
		}

		public void DoFill(float duration, bool autoHide = false, Action onComplete = null)
		{
			SetMaxValue(duration, true);
			Switch(true);
			_fillTweener = DOTween.To(x => SetValue(x, true), 0, duration, duration)
														.OnComplete(() =>
														{
															if (autoHide) Switch(false);
															onComplete?.Invoke();
														});
		}

		private void UpdateBar(bool force = false)
		{
			var percent = _currentValue / _maxValue;
			_fill.DOKill();
			if (force)
				_fill.fillAmount = percent;
			else
				_fill.DOFillAmount(percent, _fillDuration);

			if (_progress != default) _progress.text = percent <= 0 ? "" : $"{(int) (percent * 100)}%";

			if (!_hideIdle && _hideEmpty) Switch(percent > 0);
			if (!_hideIdle) return;

			ShowAndHide();
		}

		private void SetColor()
		{
			_fill.color = _fillColor;
		}

		private void ShowAndHide()
		{
			Show(_switchDuration);
			_showCor.Stop(this);
			_showCor = StartCoroutine(ShowCor());
		}

		private IEnumerator ShowCor()
		{
			yield return new WaitForSeconds(_hideDelay);

			Hide(_switchDuration);
		}

		private void OnDestroy()
		{
			_fillTweener.Kill();
			_showCor.Stop(this);
		}
    }
}