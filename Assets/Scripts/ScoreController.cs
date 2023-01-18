using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score = 0;
    private int buildIndex;

    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + score;
        SetHighScore();
        buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;  
    }
    void Update()
    {
        if (buildIndex != 1)
        {
            SetHighScore();
        }
    }
    
    public void ChangeScore(int increment)
    {
        if(score < 1 && increment < 0) { return;  }
        score += increment;
        scoreText.text = "Score: " + score;
    }

    public void SetHighScore()
    {
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
        }
    }

}
