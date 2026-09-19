using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public static PlayerEffect Instance { get; private set; }

    [SerializeField] private GameObject effects;
    [SerializeField] private SpriteRenderer haloSlash;

    [SerializeField] private Renderer targetRenderer;

    private Material dynamicMaterial;

    void Start()
    {
        Instance = this;

        if (targetRenderer == null) targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null) dynamicMaterial = targetRenderer.material;
    }

    public void PlayEffect(bool isMoney)
    {
        haloSlash.color = isMoney ? Color.green : Color.red;

        if (dynamicMaterial.HasProperty("_BaseColor"))
            dynamicMaterial.SetColor("_BaseColor", isMoney ? Color.white : Color.red);

        CancelInvoke(nameof(DisableEffect));
        effects.SetActive(true);
        Invoke(nameof(DisableEffect), 0.5f);
    }

    private void DisableEffect()
    {
        if (effects != null) effects.SetActive(false);
    }
}
