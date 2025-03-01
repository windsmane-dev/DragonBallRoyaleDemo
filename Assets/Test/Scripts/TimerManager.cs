using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float maxTime;
    protected float currentTime;

    public Image timerImage;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        EventHolder.OnTurnReset += ResetTimer;
    }

    private void OnDestroy()
    {
        EventHolder.OnTurnReset -= ResetTimer;
    }

    void ResetTimer()
    {
        currentTime = maxTime;
        StopAllCoroutines();
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        float temp = 0;

        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            temp = Mathf.Round(currentTime * 10f) * 0.1f;
            timerImage.fillAmount = 1- currentTime / maxTime;
            timerText.text = temp.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        EventHolder.TriggerRoundEnd(RoundResult.Draw);
    }
    
}
