using UI.Buttons;
using UnityEngine;

namespace Game.Scripts.UI.Windows
{
    public class VictoryWindow: CanvasBasedWindow
    {
        [Space]
        [SerializeField] private MyButton _continueButton;

        public MyButton ContinueButton => _continueButton;

        public sealed override void Enable(bool value, float delay = -1, float duration = -1)
        {
            base.Enable(value, delay, duration);

            if (value)
            {
                _continueButton.Show();
            }
            else
            {
                _continueButton.Hide();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            ShowDelayWps = new WaitForSeconds(_showDelay);
        }
    }
}