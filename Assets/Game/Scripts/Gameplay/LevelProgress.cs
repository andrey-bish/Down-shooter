using Extensions;
using Game.ScriptrableObjects.Classes;
using UI.ProgressBars;
using UnityEngine;

namespace Gameplay
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField, GroupComponent] private ProgressBar _levelProgressBar;

        public void Init(LevelSettings levelSettings)
        {
            _levelProgressBar.SetMaxValue(levelSettings.AwardPoints[levelSettings.AwardPoints.Count-1], true, true);
            _levelProgressBar.SetValue(0, true);
            _levelProgressBar.SetSprites(levelSettings.AwardPoints, levelSettings.SpritesAward);
        }

        public void UpgradeProgress(int currentPoint) => _levelProgressBar.SetValue(currentPoint);

        public void HideProgressBar() => _levelProgressBar.Hide(0.25f);
    }
}