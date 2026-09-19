using UnityEngine;

public class FiinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.HidePlayerCanvas();
        }
    }
}
