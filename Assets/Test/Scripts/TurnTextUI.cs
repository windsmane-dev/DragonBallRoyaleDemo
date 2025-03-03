using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnTextUI : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    // Start is called before the first frame update
    void Start()
    {
        EventHolder.OnTotalTurnCountUpdated += UpdateTurnText;
    }

    private void OnDestroy()
    {
        EventHolder.OnTotalTurnCountUpdated -= UpdateTurnText;
    }
    // Update is called once per frame
    void UpdateTurnText(int val)
    {
        turnText.text = "Turn: " + val.ToString();
    }
}
