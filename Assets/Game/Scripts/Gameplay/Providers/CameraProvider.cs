using Cinemachine;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Utils;

using static Providers.CameraProvider.CameraType;

namespace Providers
{
    public class CameraProvider: Singleton<CameraProvider>
    {
        [SerializeField, GroupComponent] private Camera                   _mainCamera;
		[SerializeField, GroupComponent] private CinemachineVirtualCamera _target;
		[SerializeField, GroupComponent] private CinemachineVirtualCamera _player;
		[SerializeField, GroupComponent] private CinemachineBrain         _brain;

		private static CinemachineVirtualCamera Target => Instance._target;
		public static CinemachineVirtualCamera Player => Instance._player;
		private static CinemachineBrain Brain => Instance._brain;
		private static CinemachineBasicMultiChannelPerlin _playerNoise;
		private static Tweener                            _shakeTween;

		protected override void Awake()
		{
			base.Awake();
			UpdatePriority(PlayerCamera);
			_playerNoise = Player.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}

		public static Camera MainCamera => Instance._mainCamera;
		public static void SetBlendTime(float time) => Brain.m_DefaultBlend.m_Time = time;
		public static void SetTarget(Transform target) => SetupCamera(Target, target);
		public static void SetPlayerTarget(Transform target) => SetupCamera(Player, target);
		public static void ShowPlayer() => UpdatePriority(PlayerCamera);
		public static void ShowTarget() => UpdatePriority(TargetCamera);

		private static void UpdatePriority(CameraType type)
		{
			if (Instance == default) return;

			if (Player != default) Player.Priority = Equal(type, PlayerCamera);
			if (Target != default) Target.Priority = Equal(type, TargetCamera);
		}

		private static int Equal(CameraType type1, CameraType type2) => type1 == type2 ? 1 : 0;

		private static void SetupCamera(CinemachineVirtualCamera virtualCamera, Transform target)
		{
			if (virtualCamera == default) return;

			virtualCamera.Follow = target;
			virtualCamera.LookAt = target;
		}

		public enum CameraType
		{
			PlayerCamera,
			TargetCamera
		}
    }
}