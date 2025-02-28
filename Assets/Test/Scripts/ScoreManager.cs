using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]protected int playerScore;
    [SerializeField]protected int enemyScore;
    [SerializeField]protected RoundResult result;
    private void Start()
    {
        EventHolder.OnRoundEnd += OnRoundEnd;
    }

    private void OnDestroy()
    {
        EventHolder.OnRoundEnd -= OnRoundEnd;
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

        EventHolder.TriggerEndTurn();
    }
}
