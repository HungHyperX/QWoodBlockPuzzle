using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Scores score;
    public void LoadGame(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadGame1(int sceneIndex)
    {
        PlayerPrefsHelper.SaveEmptyList("myNumbers");
        //if (Event.emptyGrids)
        //{
        //    Event._currentScores = 0;
        //    score.UpdateScoreText();
            
        //}
        score.UpdateScoreText();
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(progress);
            slider.value = progress;

            yield return null;
        }
    }
}
