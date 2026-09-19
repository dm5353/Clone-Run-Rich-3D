using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; }

    [Header("Компоненты")]
    [SerializeField] private Animator animator;
    [SerializeField] private Avatar[] avatars;
    [SerializeField] private GameObject[] models;

    [Header("Настройки порогов богатства")]
    [SerializeField] private int middleClassThreshold = 100;
    [SerializeField] private int richClassThreshold = 180;

    private static readonly int StateHash = Animator.StringToHash("State");

    private int currentAnimationState = -1;
    private bool isGameActive = false;

    private void Awake()
    {
        Instance = this;

        if (animator == null) animator = GetComponent<Animator>();
    }

    public void StartGame()
    {
        isGameActive = true;
        UpdateMovementAnimation(40);
    }

    public void UpdateMovementAnimation(float currentScore)
    {
        if (!isGameActive) return;

        int targetState;

        if (currentScore >= richClassThreshold)
        {
            targetState = 3; // WalkCocktail
        }
        else if (currentScore >= middleClassThreshold)
        {
            targetState = 2; // WalkMiddle
        }
        else
        {
            targetState = 1; // WalkPoor
        }

        SetState(targetState);
    }

    public void PlayDanceAnimation()
    {
        isGameActive = false;
        SetState(4); // Dance
    }

    public void PlayDefeatAnimation()
    {
        isGameActive = false;
        SetState(5);
    }

    public void ResetToIdle()
    {
        isGameActive = false;
        SetState(0); // Idle
    }

    private void SetState(int newState)
    {
        if (currentAnimationState == newState) return;

        animator.avatar = avatars[newState];

        currentAnimationState = newState;
        animator.SetInteger(StateHash, newState);

        if (newState >= 1 && newState <= 3)
        {
            for (int i = 0; i < models.Length; i++)
                models[i].SetActive(i + 1 == newState);
        }
    }
}
