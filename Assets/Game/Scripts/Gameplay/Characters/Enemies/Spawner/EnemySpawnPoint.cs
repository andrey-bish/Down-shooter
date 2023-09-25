using UnityEngine;

namespace Characters.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        public Bounds Bounds => _collider.bounds;
    }
}