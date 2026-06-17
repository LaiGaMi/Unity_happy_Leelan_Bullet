using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    public enum HandState { Attack, Defense }
    public HandState state;

    // =========================
    // ⭐ 單一狀態開關（重點）
    // true = 攻擊 / false = 防禦
    // =========================
    private bool attackEnabled;

    // =========================
    // ⭐ 防禦設定
    // =========================
    [Header("血量")]
    public int hp = 30;

    [Header("防禦特效")]
    public GameObject defenseEffect;

    [Header("消滅特效")]
    public GameObject deathEffect;

    [Header("冷卻重啟時間")]
    public float reviveTime = 10f;

    private GameObject boss;
    private BoxCollider2D box;

    // =========================
    // ⭐ 攻擊設定（保留全部）
    // =========================
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

    [Header("隨機範圍")]
    public float playerRadius = 2f;
    public float bossRadius = 2f;

    [Header("攝影機內縮")]
    public float cameraInset = 1f;

    private Camera cam;
    private Transform player;

    private int lastAttack = -1;

    private Coroutine attackRoutine;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        boss = GameObject.Find("Boss_B");
        cam = Camera.main;

        GameObject p = GameObject.Find("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        // ⭐ 攻擊=不貼身 / 防禦=貼身
        if (!attackEnabled && boss != null)
        {
            transform.position = boss.transform.position;
        }
    }

    // =========================================================
    // ⭐ 攻擊模式
    // =========================================================
    public void SetAttack()
    {
        state = HandState.Attack;

        attackEnabled = true;

        if (defenseEffect != null)
            defenseEffect.SetActive(false);

        if (box != null)
            box.enabled = false;

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (attackEnabled)
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
        if (player == null) yield break;

        Vector2 target = (Vector2)player.position +
                         Random.insideUnitCircle * playerRadius;

        yield return MoveTo(target);

        if (attack1Prefab != null)
            Instantiate(attack1Prefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(attackWaitTime);
    }

    IEnumerator AttackType2()
    {
        if (boss == null || player == null) yield break;

        Vector2 target = (Vector2)boss.transform.position +
                         Random.insideUnitCircle * bossRadius;

        yield return MoveTo(target);

        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (attack2Prefab != null)
            Instantiate(attack2Prefab, transform.position,
                Quaternion.Euler(0, 0, angle));

        yield return new WaitForSeconds(attackWaitTime);
    }

    IEnumerator AttackType3()
    {
        if (cam == null || player == null) yield break;

        Vector3 target = GetEdgePoint();

        yield return MoveTo(target);

        float timer = 0f;
        Vector2 dir = (player.position - transform.position).normalized;

        while (IsInScreen())
        {
            timer += Time.deltaTime;

            if (timer >= attack3Interval)
            {
                timer = 0f;

                if (attack3Prefab != null)
                    Instantiate(attack3Prefab,
                        transform.position,
                        Quaternion.Euler(0, 0,
                            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
            }

            transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        if (boss != null)
            transform.position = boss.transform.position;
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
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
        float y = top ? (c.y + h - cameraInset) : (c.y - h + cameraInset);

        return new Vector3(x, y, transform.position.z);
    }

    bool IsInScreen()
    {
        Vector3 vp = cam.WorldToViewportPoint(transform.position);
        return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
    }

    // =========================================================
    // ⭐ 防禦（單開關版本）
    // =========================================================
    public void SetDefense()
    {
        state = HandState.Defense;

        attackEnabled = false;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        hp = 30;

        if (defenseEffect != null)
            defenseEffect.SetActive(true);

        if (box != null)
            box.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attackEnabled) return; // ⭐ 攻擊時不吃傷

        if (!other.CompareTag("player_attack"))
            return;

        hp--;

        Destroy(other.gameObject);

        if (hp <= 0)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    IEnumerator DieAndRespawn()
    {
        attackEnabled = false;

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        if (defenseEffect != null)
            defenseEffect.SetActive(false);

        if (box != null)
            box.enabled = false;

        Vector3 pos = transform.position;

        if (deathEffect != null)
            Instantiate(deathEffect, pos, Quaternion.identity);

        transform.position = new Vector3(9999, 9999, 0);

        yield return new WaitForSeconds(reviveTime);

        SetDefense();
    }

    public void SetStateAttack() => SetAttack();
    public void SetStateDefense() => SetDefense();
}