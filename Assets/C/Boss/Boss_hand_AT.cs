using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_hand_AT : MonoBehaviour
{
    [Header("攻擊1物件")]
    public GameObject attack1Prefab;

    [Header("攻擊2物件")]
    public GameObject attack2Prefab;

    [Header("攻擊3物件")]
    public GameObject attack3Prefab;

    [Header("等待時間")]
    public float attackWaitTime = 3f;

    [Header("第三攻擊生成間隔")]
    public float attack3Interval = 0.1f;

    [Header("移動速度")]
    public float moveSpeed = 5f;

    [Header("Boss周圍範圍")]
    public float bossRadius = 2f;

    [Header("Attack1內圈半徑(禁止區)")]
    public float attack1MinRadius = 1f;

    [Header("Attack1外圈半徑(最大範圍)")]
    public float attack1MaxRadius = 4f;

    [Header("攝影機內縮")]
    public float cameraInset = 1f;

    [Header("Attack1旋轉時間(秒)")]
    public float attack1RotateTime = 1f;

    [Header("Attack1旋轉圈數")]
    public float attack1RotateTurns = 1f;

    [Header("Attack1發射數量")]
    public int attack1FireCount = 10;

    private Camera cam;
    private Transform player;
    private GameObject boss;

    private int lastAttack = -1;
    private Coroutine attackRoutine;

    void Awake()
    {
        boss = GameObject.Find("Boss_B");
        cam = Camera.main;

        GameObject p = GameObject.Find("Player");
        if (p != null)
            player = p.transform;
    }

    void OnEnable()
    {
        StartAttack();
    }

    void OnDisable()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
    }

    public void StartAttack()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            int index = GetRandomAttack();
            yield return ExecuteAttack(index);
        }
    }

    int GetRandomAttack()
    {
        int r;

        do
        {
            r = Random.Range(0, 3);
        }
        while (r == lastAttack);

        lastAttack = r;
        return r;
    }

    IEnumerator ExecuteAttack(int type)
    {
        switch (type)
        {
            case 0: yield return AttackType1(); break;
            case 1: yield return AttackType2(); break;
            case 2: yield return AttackType3(); break;
        }
    }

    IEnumerator AttackType1()
    {
        if (player == null)
            yield break;

        Vector2 dir = Random.insideUnitCircle.normalized;

        float radius =
            Random.Range(attack1MinRadius, attack1MaxRadius);

        Vector2 target =
            (Vector2)player.position + dir * radius;

        yield return MoveTo(target);

        transform.rotation = Quaternion.identity;

        float timer = 0f;
        float interval =
            attack1RotateTime / attack1FireCount;

        float totalRotation =
            360f * attack1RotateTurns;

        int fired = 0;

        yield return new WaitForSeconds(0.5f);

        while (timer < attack1RotateTime)
        {
            timer += Time.deltaTime;

            float t = timer / attack1RotateTime;

            float angle =
                Mathf.Lerp(0f, totalRotation, t);

            transform.rotation =
                Quaternion.Euler(0, 0, angle);

            if (fired < attack1FireCount &&
                timer >= interval * fired)
            {
                if (attack1Prefab != null)
                {
                    Instantiate(
                        attack1Prefab,
                        transform.position,
                        transform.rotation
                    );
                }

                fired++;
            }

            yield return null;
        }

        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(attackWaitTime);
    }

    IEnumerator AttackType2()
    {
        if (boss == null || player == null)
            yield break;

        Vector2 target =
            (Vector2)boss.transform.position +
            Random.insideUnitCircle * bossRadius;

        yield return MoveTo(target);

        Vector2 dir =
            (player.position - transform.position)
            .normalized;

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        yield return new WaitForSeconds(0.5f);

        if (attack2Prefab != null)
        {
            Instantiate(
                attack2Prefab,
                transform.position,
                Quaternion.Euler(0, 0, angle)
            );
        }

        yield return new WaitForSeconds(attackWaitTime);
    }

    IEnumerator AttackType3()
    {
        if (cam == null || player == null)
            yield break;

        Vector3 target = GetEdgePoint();

        yield return MoveTo(target);

        float timer = 0f;

        Vector2 dir =
            (player.position - transform.position)
            .normalized;

        while (IsInScreen())
        {
            timer += Time.deltaTime;

            if (timer >= attack3Interval)
            {
                timer = 0f;

                if (attack3Prefab != null)
                {
                    Instantiate(
                        attack3Prefab,
                        transform.position,
                        Quaternion.Euler(
                            0,
                            0,
                            Mathf.Atan2(dir.y, dir.x)
                            * Mathf.Rad2Deg
                        )
                    );
                }
            }

            transform.position +=
                (Vector3)dir *
                moveSpeed *
                2f *
                Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        if (boss != null)
            transform.position = boss.transform.position;
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (
            Vector2.Distance(
                transform.position,
                target
            ) > 0.1f)
        {
            transform.position =
                Vector2.MoveTowards(
                    transform.position,
                    target,
                    moveSpeed * Time.deltaTime
                );

            yield return null;
        }
    }

    Vector3 GetEdgePoint()
    {
        float h = cam.orthographicSize;
        float w = h * cam.aspect;

        Vector3 c = cam.transform.position;

        bool top = Random.value > 0.5f;

        float x = c.x + w - cameraInset;

        float y = top
            ? c.y + h - cameraInset
            : c.y - h + cameraInset;

        return new Vector3(
            x,
            y,
            transform.position.z
        );
    }

    bool IsInScreen()
    {
        Vector3 vp =
            cam.WorldToViewportPoint(
                transform.position
            );

        return vp.x >= 0 &&
               vp.x <= 1 &&
               vp.y >= 0 &&
               vp.y <= 1;
    }
}