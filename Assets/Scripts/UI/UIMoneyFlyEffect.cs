using UnityEngine;
using System.Collections.Generic;

public class UIMoneyFlyEffect : MonoBehaviour
{
    public static UIMoneyFlyEffect Instance { get; private set; }

    [Header("Префабы и компоненты")]
    [SerializeField] private GameObject dollarPrefab;
    [SerializeField] private RectTransform targetHUDPoint;
    [SerializeField] private RectTransform buttonSpawnPoint;
    [SerializeField] private AudioClip pickupSound;

    [Header("Настройки анимации")]
    [SerializeField] private int coinsCount = 10;
    [SerializeField] private float minSpread = 40f;
    [SerializeField] private float maxSpread = 120f;
    [SerializeField] private float flySpeed = 8f;

    private List<RectTransform> dollarPool = new List<RectTransform>();
    private List<ActiveDollar> activeDollars = new List<ActiveDollar>();

    private struct ActiveDollar
    {
        public RectTransform rect;
        public Vector3 startPos;
        public Vector3 controlPos;
        public float time;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayFlyEffect()
    {
        if (dollarPrefab == null || targetHUDPoint == null) return;

        Vector3 spawnWorldPos = buttonSpawnPoint.position;

        for (int i = 0; i < coinsCount; i++)
        {
            RectTransform coin = GetPooledDollar();
            coin.position = spawnWorldPos;
            coin.gameObject.SetActive(true);

            float angle = Random.Range(90f, 180f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpread, maxSpread);
            Vector3 burstOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
            Vector3 peakPos = spawnWorldPos + burstOffset;

            activeDollars.Add(new ActiveDollar
            {
                rect = coin,
                startPos = spawnWorldPos,
                controlPos = peakPos,
                time = 0f
            });
        }
    }

    private void Update()
    {
        Vector3 targetPos = targetHUDPoint.position;

        for (int i = activeDollars.Count - 1; i >= 0; i--)
        {
            ActiveDollar dollar = activeDollars[i];
            dollar.time += Time.deltaTime * flySpeed;

            Vector3 m1 = Vector3.Lerp(dollar.startPos, dollar.controlPos, dollar.time);
            Vector3 m2 = Vector3.Lerp(dollar.controlPos, targetPos, dollar.time);
            dollar.rect.position = Vector3.Lerp(m1, m2, dollar.time);

            activeDollars[i] = dollar;

            if (dollar.time >= 1f)
            {
                dollar.rect.gameObject.SetActive(false);
                activeDollars.RemoveAt(i);

                if (activeDollars.Count == 0)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    AudioSource.PlayClipAtPoint(pickupSound, player.transform.position);
                }
            }
        }
    }

    private RectTransform GetPooledDollar()
    {
        foreach (var coin in dollarPool)
        {
            if (coin != null && !coin.gameObject.activeSelf)
            {
                return coin;
            }
        }

        GameObject newCoin = Instantiate(dollarPrefab, transform);
        RectTransform rect = newCoin.GetComponent<RectTransform>();
        dollarPool.Add(rect);
        return rect;
    }
}
