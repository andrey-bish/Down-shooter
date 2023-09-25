using System.Collections.Generic;
using Gameplay;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.ScriptrableObjects.Classes
{
    public class PrefabsContainer: GlobalConfig<PrefabsContainer>
    {
        [SerializeField] private List<Level> levels;
        
        public List<Level> Levels => levels;
    }
}