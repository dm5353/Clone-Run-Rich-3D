using UnityEngine;

public class PlayerAudioFootsteps : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private AudioSource audioSource;

    [Header("Массив звуков шагов")]
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Настройки времени")]
    [SerializeField] private float stepInterval = 0.35f;

    private float timer = 0f;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!SplinePlayerMovement.Instance.IsRunning) return;

        if (footstepClips == null || footstepClips.Length == 0 || audioSource == null) return;

        timer += Time.deltaTime;

        if (timer >= stepInterval)
        {
            timer = 0f;
            PlayStep();
        }
    }

    private void PlayStep()
    {
        int randomIndex = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[randomIndex]);
    }
}
