using UnityEngine;

namespace Game.Scripts.UI.Windows
{
    public class GameWindow: CanvasBasedWindow
    {
	    [SerializeField] private float _hideLevelTextInterval;
		
		private WaitForSeconds _hideLevelTextIntervalWfs;
		private Coroutine      _hideLevelTextCoroutine;

		public sealed override void Enable(bool value, float delay = -1, float duration = -1)
		{
			base.Enable(value, delay, duration);

			if (!enabled) return;
			
		}

		protected override void Awake()
		{
			base.Awake();

			_hideLevelTextIntervalWfs = new WaitForSeconds(_hideLevelTextInterval);
		}

		
		

		

	
		
    }
}