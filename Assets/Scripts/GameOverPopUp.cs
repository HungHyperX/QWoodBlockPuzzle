using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject darkScreen;
    public GameObject losePopup;
    public GameObject newHighScorePopup;
    // Start is called before the first frame update

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.Log("AUDIOMANAGER NOT FOUND");
        }
    }
    void Start()
    {
        gameOverPopup.SetActive(false);
        darkScreen.SetActive(false);
    }
    // in the beginning, if the gameObject that this script is attached to is not active, the OnEnable and OnDisable will not be called
    private void OnDisable()
    {
        Event.GameOver -= OnGameOver;
    }

    private void OnEnable()
    {
        Event.GameOver += OnGameOver;
    }

    private void OnGameOver(bool newHighScore)
    {
        gameOverPopup.SetActive(true);
        darkScreen.SetActive(true);
        losePopup.SetActive(true);
        newHighScorePopup.SetActive(false);
        audioManager.PlaySFX(audioManager.gameOverMusic);
    }
}
