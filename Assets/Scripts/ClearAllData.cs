#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class ClearAllSavedData
{
    [MenuItem("Tools/Clear All Saved Data")]
    public static void ClearAllData()
    {
        ClearPlayerPrefs();
        ClearPersistentData();
        Debug.Log("All saved data cleared.");
    }

    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared.");
    }

    private static void ClearPersistentData()
    {
        string persistentDataPath = Application.persistentDataPath;

        if (Directory.Exists(persistentDataPath))
        {
            DirectoryInfo directory = new DirectoryInfo(persistentDataPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
            Debug.Log($"All data in {persistentDataPath} cleared.");
        }
        else
        {
            Debug.LogWarning($"Directory {persistentDataPath} does not exist.");
        }
    }
}

#endif