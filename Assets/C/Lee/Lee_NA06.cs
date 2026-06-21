using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_NA06 : MonoBehaviour
{
    [Header("初始速度")]
    public float speed = 10f;

    [Header("持續時間")]
    public float lifeTime = 3f;

    [Header("速度衰減曲線（0~1）")]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("爆炸物件")]
    public GameObject explosionPrefab;

    private Transform player;
    private float timer;

    void Start()
    {
        GameObject p = GameObject.Find("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        RotateToPlayer();
        MoveForward();
        HandleLifeTime();
    }

    void RotateToPlayer()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void MoveForward()
    {
        float t = Mathf.Clamp01(timer / lifeTime);
        float currentSpeed = speed * speedCurve.Evaluate(t);

        transform.position += transform.right * currentSpeed * Time.deltaTime;
    }

    void HandleLifeTime()
    {
        if (timer < lifeTime) return;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}