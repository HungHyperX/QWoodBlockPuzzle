using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{

    private void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }

    public void LoadScene(string name)
    {
        PlayerPrefsHelper.SaveEmptyList("myNumbers");
        SceneManager.LoadScene(name);
    }

    public void LoadScene1(string name)
    {
        SceneManager.LoadScene(name);
    }

}
