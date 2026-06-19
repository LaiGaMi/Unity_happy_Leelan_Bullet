using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_P2Att : MonoBehaviour
{
	// =========================
    // ⭐ 旋轉設定
    // =========================

    [Header("最大偏轉角度（±）")]
    public float maxAngle = 45f;

    [Header("旋轉速度")]
    public float rotateSpeed = 2f;

    private float baseAngle = 180f; // 面向左邊
    private float timeCounter;

    // =========================
    // ⭐ 射擊設定
    // =========================

    [Header("子彈物件")]
    public GameObject bulletPrefab;

    [Header("射擊間隔")]
    public float fireInterval = 0.1f;

    [Header("子彈生成點")]
    public Transform firePoint;

    private Coroutine fireRoutine;
	
    void Start()
    {
        // 初始化面向左邊
        transform.rotation = Quaternion.Euler(0, 0, baseAngle);

        fireRoutine = StartCoroutine(FireLoop());
    }

    void Update()
    {
        HandleRotation();
    }

    // =========================
    // ⭐ 曲線旋轉
    // =========================
    void HandleRotation()
    {
        timeCounter += Time.deltaTime * rotateSpeed;

        float angleOffset = Mathf.Sin(timeCounter) * maxAngle;

        float finalAngle = baseAngle + angleOffset;

        transform.rotation = Quaternion.Euler(0, 0, finalAngle);
    }

    // =========================
    // ⭐ 射擊
    // =========================
    IEnumerator FireLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);

            Fire();
        }
    }

    void Fire()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = firePoint != null
            ? firePoint.position
            : transform.position;

        Instantiate(
            bulletPrefab,
            spawnPos,
            transform.rotation
        );
    }
}
