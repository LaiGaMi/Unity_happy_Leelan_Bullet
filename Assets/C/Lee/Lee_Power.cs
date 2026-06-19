using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_Power : MonoBehaviour
{
    // =========================
    // ⭐ 能量點
    // =========================

    [Header("能量點1")]
    public Transform energyPoint1;

    [Header("能量點2")]
    public Transform energyPoint2;

    [Header("額外能量點(可不設)")]
    public Transform extraEnergyPoint;

    [Header("匯合點")]
    public Transform mergePoint;

    // =========================
    // ⭐ 特效與雷射
    // =========================

    [Header("蓄力特效")]
    public GameObject chargeEffectPrefab;

    [Header("雷射物件")]
    public GameObject laserPrefab;

    // =========================
    // ⭐ 時間設定
    // =========================

    [Header("蓄力移動時間")]
    public float chargeMoveTime = 3f;

    [Header("匯合後等待時間")]
    public float laserDelay = 1f;

    // =========================
    // ⭐ 雷射角度
    // =========================

    [Header("主雷射角度")]
    public float laserAngle = 0f;

    [Header("額外雷射角度")]
    public float extraLaserAngle = 0f;

    // =========================

    private void Start()
    {
        StartCoroutine(EnergyAttack());
    }

    IEnumerator EnergyAttack()
    {
        // =========================
        // ⭐ 生成蓄力特效
        // =========================

        GameObject effect1 = null;
        GameObject effect2 = null;
        GameObject effectExtra = null;

        if (chargeEffectPrefab != null)
        {
            if (energyPoint1 != null)
            {
                effect1 = Instantiate(
                    chargeEffectPrefab,
                    energyPoint1.position,
                    Quaternion.identity,
                    transform
                );
            }

            if (energyPoint2 != null)
            {
                effect2 = Instantiate(
                    chargeEffectPrefab,
                    energyPoint2.position,
                    Quaternion.identity,
                    transform
                );
            }

            if (extraEnergyPoint != null)
            {
                effectExtra = Instantiate(
                    chargeEffectPrefab,
                    extraEnergyPoint.position,
                    Quaternion.identity,
                    transform
                );
            }
        }

        // =========================
        // ⭐ 特效移動到匯合點
        // =========================

        float timer = 0f;

        Vector3 start1 = effect1 != null ? effect1.transform.position : Vector3.zero;
        Vector3 start2 = effect2 != null ? effect2.transform.position : Vector3.zero;

        while (timer < chargeMoveTime)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / chargeMoveTime);

            if (effect1 != null)
            {
                effect1.transform.position =
                    Vector3.Lerp(start1, mergePoint.position, t);
            }

            if (effect2 != null)
            {
                effect2.transform.position =
                    Vector3.Lerp(start2, mergePoint.position, t);
            }

            yield return null;
        }

        // =========================
        // ⭐ 等待
        // =========================

        yield return new WaitForSeconds(laserDelay);

        // =========================
        // ⭐ 刪除蓄力特效
        // =========================

        if (effect1 != null) Destroy(effect1);
        if (effect2 != null) Destroy(effect2);
        if (effectExtra != null) Destroy(effectExtra);

        // =========================
        // ⭐ 生成主雷射（修正：保留原始縮放）
        // =========================

        if (laserPrefab != null && mergePoint != null)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                mergePoint.position,
                Quaternion.Euler(0, 0, laserAngle),
                transform
            );

            laser.transform.localScale = laserPrefab.transform.localScale;
        }

        // =========================
        // ⭐ 額外雷射（修正：保留原始縮放）
        // =========================

        if (laserPrefab != null && extraEnergyPoint != null)
        {
            GameObject extraLaser = Instantiate(
                laserPrefab,
                extraEnergyPoint.position,
                Quaternion.Euler(0, 0, extraLaserAngle),
                transform
            );

            extraLaser.transform.localScale = laserPrefab.transform.localScale;
        }
    }
}