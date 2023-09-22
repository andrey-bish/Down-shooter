using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

using static Common.Constant.AssetPath;

namespace Common.ObjectPool
{
	public class Pool : MonoBehaviour
	{
		[SerializeField] private bool _prewarm;

		[SerializeField, TableList, ShowIf(nameof(_prewarm))]
		private List<PrewarmElement> _prewarmElements;

		[SerializeField, ShowIf(nameof(_prewarm))]
		private bool _smartSpawn;

		[SerializeField, Indent, ShowIf(nameof(SmartSpawn)), Min(0)]
		private float _targetDeltaTime = 0.016f;

		[SerializeField, Indent, ShowIf(nameof(SmartSpawn)), Min(1)]
		private int _smartStep = 1;

		[SerializeField, ShowIf(nameof(_prewarm)), Min(1)]
		private int _spawnPerFrame = 10;

		private static GameObject PoolGameObject { get; set; }
		private static Pool _instance;

		[GroupView] private static readonly Dictionary<int, Queue<IPoolable>> poolItems = new();
		[GroupView] private static readonly Dictionary<int, Transform> containers = new();
		[GroupView] private static readonly HashSet<IPoolable> usedItems = new();

		private bool SmartSpawn => _prewarm && _smartSpawn;

		private Transform CachedTransform =>
			_cachedTransform == default ? _cachedTransform = transform : _cachedTransform;

		private Transform _cachedTransform;

		private void Awake()
		{
			if (_instance != default) Destroy(this);

			_instance = this;
			PoolGameObject = gameObject;
			StartCoroutine(PrewarmCor());
		}

		private void OnDestroy()
		{
			poolItems.Clear();
			containers.Clear();
			usedItems.Clear();
		}

		private IEnumerator PrewarmCor()
		{
			if (!_prewarm) yield break;

			yield return null;

			foreach (var element in _prewarmElements)
			{
				if (element.Prefab == default || element.Count == 0) continue;

				var prefab = element.Prefab;
				var spawnPerFrame = _spawnPerFrame;
				var id = prefab.GetInstanceID();
				var container = GetContainer(id);

				for (var i = 0; i < element.Count; i++)
				{
					var newItem = InstantiateObject(prefab, Vector3.zero, container, id, false);
					newItem.Release();
					spawnPerFrame--;

					if (spawnPerFrame <= 0)
					{
						if (_smartSpawn)
						{
							if (Time.deltaTime < _targetDeltaTime)
								_spawnPerFrame += _smartStep;
							else
								_spawnPerFrame -= _smartStep;

							_spawnPerFrame = Mathf.Max(1, _spawnPerFrame);
						}

						spawnPerFrame = _spawnPerFrame;
						yield return null;
					}
				}

				yield return null;
			}
		}

		public static Pool Instance
		{
			get
			{
				if (_instance != default) return _instance;

				PoolGameObject = new GameObject("###_MAIN_POOL_###");
				_instance = PoolGameObject.AddComponent<Pool>();

				return _instance;
			}
		}

		public static T Get<T>(T prefab, Vector3 position = default, Transform parent = default)
			where T : UnityEngine.Object, IPoolable
		{
			T pooledItem;
			var id = prefab.GetInstanceID();
			var queue = GetQueue(id);
			var container = GetContainer(id);
			if (queue.Count > 0)
			{
				pooledItem = (T) queue.Dequeue();
				var pooledItemTransform = pooledItem.MyTransform();
				if (parent != default) pooledItemTransform.parent = parent;

				pooledItemTransform.position = position;
				//pooledItemTransform.localScale = Vector3.one;
				pooledItem.MyTransform().gameObject.SetActive(true);
				pooledItem.Restart();
				usedItems.Add(pooledItem);

				UpdateContainerName(container, queue.Count, prefab.name);
				return pooledItem;
			}

			var newParent = parent == default ? container : parent;
			pooledItem = InstantiateObject(prefab, position, newParent, id);

			UpdateContainerName(container, 0, prefab.name);
			return pooledItem;
		}

		private static T InstantiateObject<T>(T prefab, Vector3 position, Transform newParent, int id,
			bool activate = true)
			where T : UnityEngine.Object, IPoolable
		{
			var instance = Instantiate(prefab, position, prefab.MyTransform().rotation, newParent);
			//instance.transform.localScale = Vector3.one;
			instance.name = prefab.name;
			instance.Retain(id, prefab.name);
			instance.SetActive(activate);
			usedItems.Add(instance);

			return instance;
		}

		public static void Release<T>(int id, T poolItem, bool disableObject = true)
			where T : UnityEngine.Object, IPoolable
		{
			//if (!usedItems.Contains(poolItem)) return;

			var queue = GetQueue(id);
			if (!queue.Contains(poolItem)) queue.Enqueue(poolItem);
			usedItems.Remove(poolItem);

			var container = GetContainer(id);
			poolItem.SetParent(container);
			UpdateContainerName(container, queue.Count, poolItem.ContainerName);

			if (disableObject)
			{
				poolItem.SetActive(false);
			}

			//Destroy(poolItem.gameObject);
		}

		public static void ReleaseAll()
		{
			foreach (var item in usedItems.ToList())
			{
				item.Release();
			}
		}

		public static void ReleaseAll(PoolItem item) => ReleaseAll(item.GetInstanceID());

		public static void ReleaseAll(int id)
		{
			foreach (var item in usedItems.Where(x => x.ID == id).ToList())
			{
				item.Release();
			}
		}

		private static Queue<IPoolable> GetQueue(int id)
		{
			if (poolItems.TryGetValue(id, out var queue)) return queue;

			queue = new Queue<IPoolable>();
			poolItems.Add(id, queue);

			return queue;
		}

		public static Transform GetContainer(int id)
		{
			if (containers.TryGetValue(id, out var container))
			{
				return container;
			}

			container = new GameObject().transform;
			container.parent = Instance.CachedTransform;
			containers.Add(id, container);

			return container;
		}

		private static void UpdateContainerName(Transform container, int pooled, string name = default)
		{
#if UNITY_EDITOR
			var newName = name ?? container.name;
			if (name != default) container.name = $"{newName}\t{pooled}/{container.childCount}";
#endif
		}
	}

	public interface IPoolable
	{
		public int ID { get; }
		public string ContainerName { get; }
		public event Action OnRestart;
		public event Action OnRelease;
		Transform MyTransform();
		public void Restart();
		public void Retain(int id, string containerName);
		public void Release(bool disableObject = true);
		public void SetParent(Transform parent);
		public void SetActive(bool active);
	}

	[Serializable]
	public class PrewarmElement
	{
		[SerializeField, AssetSelector(Paths = POOL_ITEM_PATH)]
		private PoolItem _prefab;

		[SerializeField] private int _count;

		public PoolItem Prefab => _prefab;
		public int Count => _count;
	}
}