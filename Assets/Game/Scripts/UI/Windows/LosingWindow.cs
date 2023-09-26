using System.Collections.Generic;
using Extensions;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows
{
    public class LosingWindow: CanvasBasedWindow
    {
        [Space]
        [SerializeField] private MyButton _restartButton;

        [SerializeField] private List<Image> _stars;
        
        public MyButton RestartButton => _restartButton;

        public sealed override void Enable(bool value, float delay = -1, float duration = -1)
        {
            base.Enable(value, delay, duration);

            if (value)
            {
                _restartButton.Show();
            }
            else
            {
                _restartButton.Hide();
            }
        }

        public void SetActiveStars(int value)
        {
            for (int i = 0; i < _stars.Count; i++)
            {
                if(i <= value) _stars[i].Activate();
                else _stars[i].Deactivate();
            }
        }
    }
}