using UnityEngine;
using UnityEngine.UI;

public class EnergyBarSegment : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public void UpdateSegment(float fillAmount, float alpha)
    {
        barImage.fillAmount = fillAmount;

        Color barColor = barImage.color;
        barColor.a = alpha;
        barImage.color = barColor;
    }
}
