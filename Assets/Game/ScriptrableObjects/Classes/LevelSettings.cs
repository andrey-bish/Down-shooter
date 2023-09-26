using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.ScriptrableObjects.Classes
{
    [CreateAssetMenu(fileName = "LevelSetting", menuName = "Data/LevelSetting")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private List<int> _awardPoints;
        [SerializeField, PreviewField(200, ObjectFieldAlignment.Left)] private List<Sprite> _spritesAward;

        public List<int> AwardPoints => _awardPoints;
        public List<Sprite> SpritesAward => _spritesAward;
    }
}