using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Egg : MonoBehaviour
{
    [Header("前進速度")]
    public float speed = 10f;

    [Header("存在時間（0 = 永久）")]
    public float lifeTime = 0f;

    [Header("左右擺動幅度（0 = 無）")]
    public float waveAmount = 0f;

    [Header("擺動頻率")]
    public float waveFrequency = 5f;

    [Header("停止時生成物件")]
    public GameObject spawnOnEndPrefab;

    [Header("顏色切換目標")]
    public Color flashColor = Color.red;

    [Header("圖像物件（SpriteRenderer）")]
    public SpriteRenderer targetRenderer;

    // =========================
    // ⭐ 修改：減速曲線控制
    // =========================
    [Header("速度衰減曲線（0~1）")]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private float timer;
    private bool ended;
    private bool colorToggle;

    private Color originalColor;

    void Start()
    {
        if (targetRenderer != null)
            originalColor = targetRenderer.color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        MoveForward();
        ApplyWave();

        HandleLifeTime();
        HandleColorFlash();
    }

    void MoveForward()
    {
        float currentSpeed = speed;

        // =========================
        // ⭐ 修改：生命週期內曲線減速
        // =========================
        if (lifeTime > 0f && spawnOnEndPrefab != null)
        {
            float t = Mathf.Clamp01(timer / lifeTime);
            currentSpeed = speed * speedCurve.Evaluate(t);
        }

        transform.position += transform.right * currentSpeed * Time.deltaTime;
    }

    void ApplyWave()
    {
        if (waveAmount == 0f) return;

        float wave = Mathf.Sin(timer * waveFrequency) * waveAmount;
        transform.position += transform.up * wave * Time.deltaTime;
    }

    void HandleLifeTime()
    {
        if (lifeTime <= 0f || ended) return;

        if (timer >= lifeTime)
        {
            ended = true;

            if (spawnOnEndPrefab != null)
            {
                Instantiate(spawnOnEndPrefab, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    void HandleColorFlash()
    {
        if (lifeTime <= 0f || targetRenderer == null) return;

        float cycle = Mathf.Floor(timer / 0.2f);
        bool state = cycle % 2 == 0;

        targetRenderer.color = state ? flashColor : originalColor;
    }
}