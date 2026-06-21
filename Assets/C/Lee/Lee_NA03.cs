using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_NA03 : MonoBehaviour
{
    [Header("前進速度")]
    public float speed = 10f;

    [Header("存在時間（0 = 永久）")]
    public float lifeTime = 0f;

    [Header("左右擺動幅度（0 = 無）")]
    public float waveAmount = 0f;

    [Header("擺動頻率")]
    public float waveFrequency = 5f;

    [Header("速度衰減曲線（0~1）")]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private float timer;
    private bool reverseMove;
    private bool decayPhase;

    void Update()
    {
        timer += Time.deltaTime;

        HandleLifeState();
        MoveForward();
        ApplyWave();
    }

    void HandleLifeState()
    {
        if (lifeTime <= 0f) return;

        // 正向時間結束 → 進入衰減反轉階段
        if (!reverseMove && !decayPhase && timer >= lifeTime)
        {
            decayPhase = true;
            timer = 0f; // 重置時間用於曲線反轉計算
        }
    }

    void MoveForward()
    {
        float currentSpeed = speed;

        // =========================
        // 正常階段（前進減速）
        // =========================
        if (!decayPhase && !reverseMove && lifeTime > 0f)
        {
            float t = Mathf.Clamp01(timer / lifeTime);
            currentSpeed = speed * speedCurve.Evaluate(t);
        }

        // =========================
        // 反轉衰減階段（0 → -speed）
        // =========================
        if (decayPhase)
        {
            float t = Mathf.Clamp01(timer / lifeTime);
            float curve = speedCurve.Evaluate(t);

            currentSpeed = Mathf.Lerp(0f, -speed, curve);

            // 當速度變為負 → 進入持續回退模式
            if (currentSpeed <= 0f)
            {
                decayPhase = false;
                reverseMove = true;
            }
        }

        // =========================
        // 反向持續移動
        // =========================
        if (reverseMove)
        {
            currentSpeed = -speed;
        }

        transform.position += transform.right * currentSpeed * Time.deltaTime;
    }

    void ApplyWave()
    {
        if (waveAmount == 0f) return;

        float wave = Mathf.Sin(Time.time * waveFrequency) * waveAmount;
        transform.position += transform.up * wave * Time.deltaTime;
    }
}