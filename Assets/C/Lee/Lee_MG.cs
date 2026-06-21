using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_MG : MonoBehaviour
{
    [Header("要控制的敵人物件")]
    public Transform enemy;

    [Header("敵人移動時間")]
    public float moveTime = 1f;

    [Header("移動曲線")]
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("每次移動後等待時間")]
    public float waitTime = 1f;

    [Header("敵人死亡後生成的攻擊物件")]
    public GameObject attackPrefab;

    [Header("生成後刪除自身等待時間")]
    public float destroyDelay = 5f;

    private Transform player;
    private bool spawnedAttack;

    private void Start()
    {
        GameObject p = GameObject.Find("Player");

        if (p != null)
            player = p.transform;

        if (enemy != null)
            StartCoroutine(MoveLoop());
    }

    private void Update()
    {
        if (spawnedAttack) return;

        if (enemy == null)
        {
            spawnedAttack = true;

            if (attackPrefab != null)
            {
                Instantiate(
                    attackPrefab,
                    transform.position,
                    transform.rotation,
                    transform
                );
            }

            StartCoroutine(DestroySelf());
        }
    }

    IEnumerator MoveLoop()
    {
        while (enemy != null)
        {
            if (player != null)
            {
                Vector3 startPos = enemy.position;
                Vector3 targetPos = player.position;

                float timer = 0f;

                while (timer < moveTime && enemy != null)
                {
                    timer += Time.deltaTime;

                    float t = Mathf.Clamp01(timer / moveTime);
                    float curveT = moveCurve.Evaluate(t);

                    enemy.position =
                        Vector3.Lerp(startPos, targetPos, curveT);

                    yield return null;
                }
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}