using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HighScoreData
{
    public int score = 0;
}
public class Scores : MonoBehaviour
{
    public Text scoreText;
    public Text scoreEndText;
    public Text highScoreText;

    private bool newHighScore = false;
    private HighScoreData _highScore = new HighScoreData();
    //public int _currentScores { get; set; }

    private string _highScoreKey = "hsdat";

    private void Awake()
    {
        if (BinaryDataStream.Exist(_highScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    private IEnumerator ReadDataFile()
    {
        _highScore = BinaryDataStream.Read<HighScoreData>(_highScoreKey);
        yield return new WaitForEndOfFrame();
        //Debug.Log("Read High Scores = " + _highScore.score);
        Event.UpdateHighScoreText(Event._currentScores, _highScore.score); // When Restart the game, highscore is still remain
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Event.emptyGrids)
        {
            Event._currentScores = 0;
        }
        else
        {
            Event._currentScores = PlayerPrefs.GetInt("CurrentScore");
        }
        newHighScore = false;
        Event.UpdateHighScoreText(Event._currentScores, _highScore.score);
        UpdateHighScoreText(Event._currentScores, _highScore.score);
        UpdateScoreText();

    }

    private void OnDisable()
    {
        Event.AddScores -= AddScores;
        Event.GameOver -= SaveHighScore;
        Event.UpdateHighScoreText -= UpdateHighScoreText;
    }

    private void OnEnable()
    {
        Event.AddScores += AddScores;
        Event.UpdateHighScoreText += UpdateHighScoreText;
    }

    public void SaveHighScore(bool newHighScores)
    {
        BinaryDataStream.Save<HighScoreData>(_highScore, _highScoreKey);
    }

    private void AddScores(int scores)
    {
        Event._currentScores += scores;
        if (Event._currentScores > _highScore.score)
        {
            newHighScore = true;
            _highScore.score = Event._currentScores;
            SaveHighScore(true);
        }

        Event.UpdateHighScoreText(Event._currentScores, _highScore.score);
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = Event._currentScores.ToString();
        scoreEndText.text = Event._currentScores.ToString();
    }

    void UpdateHighScoreText(int currentScore, int highScore)
    {
        highScoreText.text = highScore.ToString();
    }
}
