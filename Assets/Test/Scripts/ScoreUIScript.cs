using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        EventHolder.OnScoreUpdate += UpdateScore;
    }

    private void OnDestroy()
    {
        EventHolder.OnScoreUpdate += UpdateScore;
    }

    // Update is called once per frame
    void UpdateScore(int playerScore, int enemyScore)
    {
        if(isPlayer)
        {
            scoreText.text = "Player Score:  " + playerScore.ToString();
            return;
        }

        scoreText.text = "Enemy Score:  " + enemyScore.ToString();
    }
}
