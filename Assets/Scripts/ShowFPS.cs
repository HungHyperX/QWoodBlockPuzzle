using UnityEngine;
using System;
using System.Collections;

public class ShowFps : MonoBehaviour
{
    private float count;
    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnGUI()
    {
        // Set the style for the label
        GUIStyle textStyle = new GUIStyle();
        textStyle.fontSize = 40;
        textStyle.normal.textColor = Color.black;

        // Create a label with the current FPS
        GUI.Label(new Rect(10, 10, 100, 40), count.ToString("F1") + " FPS", textStyle);
    }
}
