using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Attack2 : MonoBehaviour
{
	[Header("開始延遲")]
    public float startDelay = 2f;

    [Header("旋轉時間")]
    public float spinDuration = 0.5f;

    [Header("衝刺最大速度")]
    public float dashSpeed = 10f;

    [Header("衝刺加速度")]
    public float dashAcceleration = 20f;

    [Header("攻擊後等待")]
    public float waitTime = 2f;

    [Header("到達判定距離")]
    public float arriveDistance = 0.1f;

    private Transform player;
	
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");

        if (playerObj != null)
            player = playerObj.transform;

        StartCoroutine(AttackLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            yield return StartCoroutine(SpinAttack());

            yield return StartCoroutine(DashAttack());

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpinAttack()
    {
        float timer = 0f;

        float totalRotation = 1080f; // 3圈

        float startZ = transform.eulerAngles.z;

        while (timer < spinDuration)
        {
            timer += Time.deltaTime;

            float t = timer / spinDuration;

            // 曲線速度（先快後慢）
            float curve = 1f - Mathf.Pow(1f - t, 3f);

            float angle = Mathf.Lerp(
                startZ,
                startZ + totalRotation,
                curve
            );

            transform.rotation =
                Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }
    }

    IEnumerator DashAttack()
    {
        if (player == null)
            yield break;

        // 面向玩家
        Vector2 dir =
            (player.position - transform.position).normalized;

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        // 鎖定當前位置
        Vector2 targetPos = player.position;

        float currentSpeed = 0f;

        while (
            Vector2.Distance(
                transform.position,
                targetPos
            ) > arriveDistance)
        {
            currentSpeed +=
                dashAcceleration * Time.deltaTime;

            currentSpeed =
                Mathf.Min(currentSpeed, dashSpeed);

            transform.position =
                Vector2.MoveTowards(
                    transform.position,
                    targetPos,
                    currentSpeed * Time.deltaTime
                );

            yield return null;
        }
    }
}
