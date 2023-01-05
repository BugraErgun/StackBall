using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;

    
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        
        MakeSingleton();
        
    }

    private void Start()
    {
        AddScore(0);
    }
    private void Update()
    {
        if (scoreText==null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            scoreText.text = score.ToString();
        }
        
    }

    void MakeSingleton()
    {
        if (instance !=null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score>PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        scoreText.text = score.ToString();

    }

    public void ResetScore()
    {
        score = 0;
    }
}
