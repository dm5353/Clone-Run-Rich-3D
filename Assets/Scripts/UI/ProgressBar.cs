using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar Instance { get; private set; }

    [Header("Компоненты")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text stateText, countText;

    [Header("Настройки цветов")]
    [SerializeField] private Color minColor = Color.red;
    [SerializeField] private Color midColor = Color.yellow;
    [SerializeField] private Color maxColor = Color.green;

    [Header("Настройки надписи")]
    [SerializeField] private string minText = "БЕДНЫЙ";
    [SerializeField] private string midText = "СОСТОЯТЕЛЬНЫЙ";
    [SerializeField] private string maxText = "БОГАТЫЙ";

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateProgress(float value)
    {
        countText.text = value.ToString();

        value = value / 200;

        fillImage.fillAmount = value;

        Color targetColor;
        string targetText;

        targetColor = value <= 0.5f ? minColor : midColor;
        targetText = value <= 0.5 ? minText : midText;

        if (value >= 0.9f)
        {
            targetColor = maxColor;
            targetText = maxText;
        }

        fillImage.color = targetColor;
        stateText.text = targetText;
        stateText.color = targetColor;
    }
}
