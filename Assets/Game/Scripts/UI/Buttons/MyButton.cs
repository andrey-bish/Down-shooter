using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Buttons
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class MyButton : MonoBehaviour
    {
        public event System.Action OnClick; 

        [SerializeField, GroupSetting] private float _scalerDuration = 0.25f;

        private Button   _button;
        private bool     _interactable = true;
        private Image    _image;
        private Sequence _sequence;

        public void Show(bool immediate = false, bool isImage = false) => Toggle(true, immediate, isImage);
        public void Hide(bool immediate = false, bool isImage = false) => Toggle(false, immediate, isImage);

        public void SetInteractable(bool value, bool isImage = false)
        {

            _interactable = value;
            if (isImage) return;
            _image ??= GetComponent<Image>();

            if (_image != default)
            {
                _image.color = _interactable ? Color.white : Color.gray;
            }
        }

        public void SetInteractable(bool value) => _interactable = value;

        protected virtual void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(ClickButton);
        }

        protected virtual void ClickButton()
        {
            if (!_interactable) return;

            OnClick?.Invoke();
        }
        
        private void Toggle(bool value, bool immediate, bool isImage = false)
        {
            _sequence?.Kill();
            SetInteractable(value, isImage);
            if (immediate)
            {
                transform.localScale = value ? Vector3.one : Vector3.zero;
            }
            else
            {
                _sequence = DOTween.Sequence();
                _sequence.Append(transform.DOScale(1.25f, _scalerDuration));
                _sequence.Append(transform.DOScale(value ? 1f : 0f, _scalerDuration));
            }
        }
    }
}