using UnityEngine;
using System.Collections.Generic;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private List<EnergyBarSegment> energyBars;
    [SerializeField] private int playerID;

    private void OnEnable()
    {
        EventHolder.OnEnergyUpdated += UpdateEnergyUI;
    }

    private void OnDisable()
    {
        EventHolder.OnEnergyUpdated -= UpdateEnergyUI;
    }

    private void UpdateEnergyUI(int updatedPlayerID, float currentEnergy, float maxEnergy)
    {
        if (updatedPlayerID != playerID) return;

        int wholeEnergy = Mathf.FloorToInt(currentEnergy);
        float fractionalEnergy = currentEnergy - wholeEnergy;

        for (int i = 0; i < energyBars.Count; i++)
        {
            float fillAmount = 0f;
            float alpha = 0f;

            if (i < wholeEnergy)
            {
                fillAmount = 1f;
                alpha = 1f;
            }
            else if (i == wholeEnergy)
            {
                fillAmount = fractionalEnergy;
                alpha = fractionalEnergy;
            }

            energyBars[i].UpdateSegment(fillAmount, alpha);
        }
    }
}
