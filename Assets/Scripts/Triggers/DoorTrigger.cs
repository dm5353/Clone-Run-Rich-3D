using UnityEngine;

public enum DoorType { Door, Flag }

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private int cost = 0;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip doorSound;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            AudioSource.PlayClipAtPoint(doorSound, transform.position);

            if (cost <= GameManager.Instance.CurrentGlobalScore) doorAnimator.SetTrigger("Open");
            else SplinePlayerMovement.Instance.StopRunning();
        }
    }

    public void ResetTrigger()
    {
        isTriggered = false;
    }
}
