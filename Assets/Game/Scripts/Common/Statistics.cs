using UnityEngine;
using static Common.Constant.PlayerPrefsKeyNames;

namespace Common
{
    public static class Statistics
    {
        public static int CurrentLevelIndex
        {
            get { return PlayerPrefs.GetInt(CURRENT_LEVEL_NUMBER, 0); }
            set
            {
                PlayerPrefs.SetInt(CURRENT_LEVEL_NUMBER, value);
                PlayerPrefs.Save();
            }
        }
        
        public static bool AllLevelsCompleted
        {
            get { return PlayerPrefs.GetInt(ALL_LEVELS_COMPLETED, 0) == 1; }
            set
            {
                PlayerPrefs.SetInt(ALL_LEVELS_COMPLETED, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
    }
}