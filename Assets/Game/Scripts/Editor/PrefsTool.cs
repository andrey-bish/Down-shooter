using UnityEditor;
using UnityEngine;

public static class PrefsTool
{
    [MenuItem("PlayerPrefs/ClearAll", priority = 1010)]
    public static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}