using System.Collections;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Game.Scripts.UI.Windows
{
	public class CanvasBasedWindow : MonoBehaviour
	{
		public event System.Action<bool> OnEnabled;

		[Header("Transition values")] [SerializeField, Min(0)]
		protected float _showDelay;

		[SerializeField, Min(0)] protected float _fadeInDuration;
		[SerializeField, Min(0)] protected float _fadeOutDuration;

		[Header("References")] [SerializeField]
		protected Canvas _canvas;

		[SerializeField] protected CanvasGroup _canvasGroup;

		protected WaitForSeconds ShowDelayWps;
		protected WaitForSeconds FadeInDurationWps;
		protected WaitForSeconds FadeOutDurationWps;
		private Coroutine _coroutine;

		public bool IsShown { get; protected set; }

		protected virtual void Awake()
		{
			if (_canvas == null) _canvas = GetComponent<Canvas>();
			if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();

			ShowDelayWps = new WaitForSeconds(_showDelay);
			FadeInDurationWps = new WaitForSeconds(_fadeInDuration);
			FadeOutDurationWps = _fadeInDuration == _fadeOutDuration
				? FadeInDurationWps
				: new WaitForSeconds(_fadeOutDuration);
		}

		/// <summary>
		/// </summary>
		/// <param name="value">d</param>
		/// <param name="delay">Delay before window appears, if less than 0 will be used showDelay value, used when value == true</param>
		/// <param name="duration">Duration of fading in or out, if less than 0 will be used fadeDuration value</param>
		public virtual void Enable(bool value, float delay = -1, float duration = -1)
		{
			if (value) _canvas.enabled = true;

			if (delay < 0) delay = _showDelay;
			if (duration < 0) duration = value ? _fadeInDuration : _fadeOutDuration;

			_coroutine.Stop(this);
			if (value && delay > 0f)
			{
				_coroutine = StartCoroutine(WithDelay());
			}
			else
			{
				Enable();
			}

			IEnumerator WithDelay()
			{
				if (delay == _showDelay)
					yield return ShowDelayWps;
				else
					yield return new WaitForSeconds(delay);

				Enable();
			}

			void Enable()
			{
				IsShown = value;

				if (duration > 0)
				{
					_canvasGroup.DOFade(value ? 1f : 0f, duration).OnComplete(() =>
					{
						if (!IsShown)
						{
							_canvas.enabled = false;
							EnableInteractable(false, 0);
						}
					});
				}
				else
				{
					_canvasGroup.alpha = value ? 1f : 0f;
					if (!value) _canvas.enabled = false;
				}

				OnEnabled?.Invoke(value);
				EnableInteractable(value, duration);
			}
		}

		public void EnableInteractable(bool enabled, float duration)
		{
			_coroutine.Stop(this);
			if (duration <= 0)
			{
				_canvasGroup.blocksRaycasts = enabled;
				_canvasGroup.interactable = enabled;
			}
			else
			{
				_coroutine = StartCoroutine(WithDelay());
			}

			IEnumerator WithDelay()
			{
				if (duration == _fadeInDuration)
					yield return FadeInDurationWps;
				else if (duration == _fadeOutDuration)
					yield return FadeOutDurationWps;
				else
					yield return new WaitForSeconds(duration);

				EnableInteractable(enabled, 0);
			}
		}
	}
}