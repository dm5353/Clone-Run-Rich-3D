using UnityEngine;

[ExecuteAlways]
public class PulseEffect : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    private Vector3 baseScale;

    private void Start() => baseScale = transform.localScale;

    private void Update()
    {
        float wave = Mathf.PingPong(Time.time * speed, 1f);
        float currentScale = Mathf.Lerp(minScale, maxScale, wave);

        transform.localScale = baseScale * currentScale;
    }
}
