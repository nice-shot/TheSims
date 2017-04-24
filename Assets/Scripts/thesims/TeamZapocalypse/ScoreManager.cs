using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Text scoreText;
    private int score;
    private int increaseScoreEvery = 1;
    private float previousTime = 0;

	void Start () {
        score = 0;
	}

    void UpdateScore () {
        scoreText.text = "Survival time: " + score.ToString();
    }
	
	void Update () {
        if (Time.time - previousTime > increaseScoreEvery) {
            score++;
            UpdateScore();
            previousTime = Time.time;
        }
	}
}
