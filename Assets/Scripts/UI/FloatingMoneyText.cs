using UnityEngine;
using TMPro;

public class FloatingMoneyText : MonoBehaviour
{
    public static FloatingMoneyText Instance { get; private set; }

    [Header("Компоненты")]
    [SerializeField] private TextMeshProUGUI textComponent;

    [Header("Позиции на экране (Canvas Local)")]
    [SerializeField] private Vector3 moneyStartLocalPos = new Vector3(200f, 0f, 0f);
    [SerializeField] private Vector3 alcoholStartLocalPos = new Vector3(-200f, 0f, 0f);

    [Header("Цвета")]
    [SerializeField] private Color moneyColor = Color.green;
    [SerializeField] private Color alcoholColor = Color.red;

    [Header("Настройки анимации")]
    [SerializeField] private float floatSpeed = 150f;
    [SerializeField] private float fadeSpeed = 2.5f;

    private int currentAmount = 0;
    private float alpha = 0f;
    private bool isActive = false;
    private float currentDirection = 1f;
    private Vector3 activeStartLocalPos;

    private void Awake()
    {
        Instance = this;
        if (textComponent == null) textComponent = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void AddPopup(int amount, bool isMoney)
    {
        float targetDirection = isMoney ? 1f : -1f;
        Vector3 targetStartPos = isMoney ? moneyStartLocalPos : alcoholStartLocalPos;

        if (!isActive || currentDirection != targetDirection)
        {
            currentAmount = amount;
            currentDirection = targetDirection;
            activeStartLocalPos = targetStartPos;
            transform.localPosition = activeStartLocalPos;
            alpha = 1f;
            isActive = true;
            gameObject.SetActive(true);
        }
        else
        {
            currentAmount += amount;
            transform.localPosition = Vector3.Lerp(transform.localPosition, activeStartLocalPos, 0.5f);
            alpha = 1f;
        }

        if (isMoney)
        {
            textComponent.text = $"+ {currentAmount} $";
            textComponent.color = moneyColor;
        }
        else
        {
            textComponent.text = $"- {currentAmount} $";
            textComponent.color = alcoholColor;
        }

        SetTextAlpha(alpha);
    }

    private void Update()
    {
        if (!isActive) return;

        transform.localPosition += Vector3.up * (floatSpeed * currentDirection) * Time.deltaTime;

        alpha -= fadeSpeed * Time.deltaTime;
        SetTextAlpha(Mathf.Max(0f, alpha));

        if (alpha <= 0f)
        {
            isActive = false;
            currentAmount = 0;
            gameObject.SetActive(false);
        }
    }

    private void SetTextAlpha(float targetAlpha)
    {
        if (textComponent == null) return;
        Color color = textComponent.color;
        color.a = targetAlpha;
        textComponent.color = color;
    }
}
