using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsBox;
    [SerializeField]
    private GameObject darkScreen;
    [SerializeField]
    private GameObject notification;
    public void OpenSettingsBox()
    {
        darkScreen.gameObject.SetActive(true);
        settingsBox.gameObject.SetActive(true);
    }

    public void CloseSettingsBox()
    {
        darkScreen.gameObject.SetActive(false);
        //settingsBox.gameObject.SetActive(false);
    }   

    public void ActiveNotification()
    {
        notification.gameObject.SetActive(true);
    }
}
