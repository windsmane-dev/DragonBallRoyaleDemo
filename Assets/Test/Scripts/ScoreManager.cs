using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]protected int playerScore;
    [SerializeField]protected int enemyScore;
    [SerializeField]protected RoundResult result;
    private void Start()
    {
        EventHolder.OnRoundEnd += OnRoundEnd;
        EventHolder.OnGameEndReached += GameEndCheck;
        EventHolder.TriggerScoreUpdate(playerScore, enemyScore);
    }

    private void OnDestroy()
    {
        EventHolder.OnRoundEnd -= OnRoundEnd;
        EventHolder.OnGameEndReached -= GameEndCheck;
    }

    void GameEndCheck()
    {
        if(playerScore > enemyScore)
        {
            //trigger game win
        }
        else if(playerScore == enemyScore)
        {
            //trigger game draw, enable maze
            EventHolder.TriggerMazeSpawnOnDraw();
        }
        else
        {
            //trigger Game Loss;
        }
    }
    void OnRoundEnd(RoundResult in_Result)
    {

        result = in_Result;
        EventHolder.TriggerRequestTurnInfo(CalculateScore);
        
    }

    void CalculateScore(int currentTurnInfo)
    {
        switch (result)
        {
            case RoundResult.AttackerPoint:
                if(currentTurnInfo == 1)
                {
                    playerScore++;
                }
                else
                {
                    enemyScore++;
                }
                break;
            case RoundResult.DefenderPoint:
                if(currentTurnInfo == 2)
                {
                    playerScore++;
                }
                else
                {
                    enemyScore++;
                }
                break;
            case RoundResult.Draw:
                break;
        }

        Debug.Log("Should send trigger to update score");


        EventHolder.TriggerScoreUpdate(playerScore, enemyScore);
        EventHolder.TriggerEndTurn();
    }
}
