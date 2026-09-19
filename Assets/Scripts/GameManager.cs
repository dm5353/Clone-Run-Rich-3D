using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public float CurrentGlobalScore = 40f;

    [SerializeField] private GameObject mainCanvas, playerCanvas, textInfo, buttonSettings, buttonExit;
    [SerializeField] private GameObject losePanel, winPanel;

    [Header("Компоненты PlayerCanvas")]
    [SerializeField] private TMP_Text countBills;

    [Header("Компоненты MainCanvas")]
    [SerializeField] private GameObject[] levelBar;

    [Header("Компоненты WinPanel")]
    [SerializeField] private TMP_Text levelText, headerText, countText;

    private static float globalScore = 0;
    private static int gameCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < gameCount; i++) levelBar[i].SetActive(true);
        levelText.text = $"Уровень {gameCount+1}";
        countBills.text = globalScore.ToString();
    }

    public void StartRun()
    {
        ChangeButton();
        PlayerAnimation.Instance.StartGame();
        CameraFollow.Instance.ChangeOffset(true);
        SplinePlayerMovement.Instance.StartRunning();
    }

    public void StopRun()
    {
        ChangeButton(false);
        CameraFollow.Instance.ChangeOffset();
        SplinePlayerMovement.Instance.StopRunning();
    }

    public void LevelOver()
    {
        bool isWin = CurrentGlobalScore > 0;

        CameraFollow.Instance.ChangeOffset();

        losePanel.SetActive(!isWin);
        winPanel.SetActive(isWin);

        if (!isWin)
        {
            PlayerAnimation.Instance.PlayDefeatAnimation();
        }
        else
        {
            PlayerAnimation.Instance.PlayDanceAnimation();
            headerText.text = "ЗАВЕРШЕНО";
            countText.text = CurrentGlobalScore.ToString();
            gameCount++;
        }
    }

    public void GetMoney()
    {
        globalScore += CurrentGlobalScore;
        countBills.text = globalScore.ToString();

        UIMoneyFlyEffect.Instance.PlayFlyEffect();
    }

    public void ResetScene()
    {
        CurrentGlobalScore = 40f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void HidePlayerCanvas() => playerCanvas.SetActive(false);

    private void ChangeButton(bool isRun = true)
    {
        mainCanvas.SetActive(!isRun);
        buttonSettings.SetActive(!isRun);
        textInfo.SetActive(isRun);
        buttonExit.SetActive(isRun);
        playerCanvas.SetActive(isRun);
    }
}
