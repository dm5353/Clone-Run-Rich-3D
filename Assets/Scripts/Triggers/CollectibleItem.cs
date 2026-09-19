using UnityEngine;

public enum PickupType { Bills, Alcohol, GoodChoice, BadChoice }

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private PickupType itemType;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private GameObject rotationObject;

    private void Update()
    {
        if (rotationObject == null) return;
        rotationObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPickupImpact();
        }
    }

    private void ApplyPickupImpact()
    {
        bool isGood = true;

        float scoreChange;

        if (itemType == PickupType.Bills) scoreChange = 2f;
        else if (itemType == PickupType.GoodChoice) scoreChange = 20f;
        else { scoreChange = -20f; isGood = false; }

        FloatingMoneyText.Instance.AddPopup(Mathf.Abs((int)scoreChange), isGood);

        AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        float currentScore = GameManager.Instance.CurrentGlobalScore;
        currentScore += scoreChange;

        GameManager.Instance.CurrentGlobalScore = currentScore;
        ProgressBar.Instance.UpdateProgress(currentScore);
        PlayerAnimation.Instance.UpdateMovementAnimation(currentScore);
        PlayerEffect.Instance.PlayEffect(isGood);

        if (currentScore <= 0f)
        {
            PlayerAnimation.Instance.PlayDefeatAnimation();
            SplinePlayerMovement.Instance.StopRunning();
        }

        gameObject.SetActive(false);
    }
}
