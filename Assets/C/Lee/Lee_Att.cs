using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttackController : MonoBehaviour
{
    [Header("攻擊組1")]
    public List<GameObject> attackGroup1 = new List<GameObject>();

    [Header("攻擊組2")]
    public List<GameObject> attackGroup2 = new List<GameObject>();

    [Header("攻擊組3")]
    public List<GameObject> attackGroup3 = new List<GameObject>();

    [Header("攻擊生成位置")]
    public Transform spawnPoint;

    [Header("首次等待時間")]
    public float firstDelay = 1f;

    [Header("下一輪等待時間")]
    public float loopDelay = 2f;

    private Transform player;

    // ⭐ 記錄上一輪選擇
    private int lastFirstIndex = -1;
    private int lastSecondIndex = -1;

    private void Start()
    {
        GameObject p = GameObject.Find("Player");

        if (p != null)
            player = p.transform;

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(firstDelay);

        while (true)
        {
            SpawnAttack();

            yield return new WaitUntil(() => spawnPoint.childCount == 0);

            yield return new WaitForSeconds(loopDelay);
        }
    }

    void SpawnAttack()
    {
        List<List<GameObject>> groups = new List<List<GameObject>>()
        {
            attackGroup1,
            attackGroup2,
            attackGroup3
        };

        if (groups.Count < 2) return;
        if (spawnPoint == null) return;

        // =========================
        // ⭐ 第一個攻擊（第0項）
        // 排除上一輪用過的組
        // =========================
        List<int> availableA = new List<int>();

        for (int i = 0; i < groups.Count; i++)
        {
            if (i != lastFirstIndex)
                availableA.Add(i);
        }

        int firstIndex = availableA[Random.Range(0, availableA.Count)];

        // =========================
        // ⭐ 第二個攻擊（第1項）
        // 排除同組 + 上一輪用過
        // =========================
        List<int> availableB = new List<int>();

        for (int i = 0; i < groups.Count; i++)
        {
            if (i != firstIndex && i != lastSecondIndex)
                availableB.Add(i);
        }

        int secondIndex = availableB[Random.Range(0, availableB.Count)];

        List<GameObject> groupA = groups[firstIndex];
        List<GameObject> groupB = groups[secondIndex];

        if (groupA.Count < 2) return;
        if (groupB.Count < 2) return;

        // 第一個攻擊使用第0項
        GameObject attackA = Instantiate(
            groupA[0],
            spawnPoint.position,
            Quaternion.identity,
            spawnPoint
        );

        LookAtPlayer(attackA);

        // 第二個攻擊使用第1項
        GameObject attackB = Instantiate(
            groupB[1],
            spawnPoint.position,
            Quaternion.identity,
            spawnPoint
        );

        LookAtPlayer(attackB);

        // =========================
        // ⭐ 更新記錄
        // =========================
        lastFirstIndex = firstIndex;
        lastSecondIndex = secondIndex;
    }

    void LookAtPlayer(GameObject obj)
    {
        if (obj == null) return;
        if (player == null) return;

        Vector2 dir =
            (player.position - obj.transform.position).normalized;

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        obj.transform.rotation =
            Quaternion.Euler(0, 0, angle);
    }
}