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

    // =========================
    // ⭐ 新增：攝影機範圍
    // =========================
    [Header("邊界內縮")]
    public float cameraInset = 1f;

    private Transform player;
    private Camera cam;
	
	public AudioSource AttSource;
	public AudioSource rotSource;

    void Start()
    {
		
        GameObject playerObj = GameObject.Find("Player");

        if (playerObj != null)
            player = playerObj.transform;

        cam = Camera.main;

        StartCoroutine(AttackLoop());
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
		AttSource.Play();
		
        if (player == null || cam == null)
            yield break;

        // 面向玩家
        Vector2 dir =
            (player.position - transform.position).normalized;

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        // =========================
        // ⭐ 計算攝影機邊界
        // =========================
        float h = cam.orthographicSize;
        float w = h * cam.aspect;

        Vector3 c = cam.transform.position;

        float left = c.x - w + cameraInset;
        float right = c.x + w - cameraInset;
        float top = c.y + h - cameraInset;
        float bottom = c.y - h + cameraInset;

        // =========================
        // ⭐ 根據方向決定撞哪個邊界
        // =========================
        Vector2 targetPos = player.position;

        float dx = dir.x;
        float dy = dir.y;

        float tX = float.PositiveInfinity;
        float tY = float.PositiveInfinity;

        if (Mathf.Abs(dx) > 0.0001f)
        {
            tX = (dx > 0 ? (right - transform.position.x)
                         : (left - transform.position.x)) / dx;
        }

        if (Mathf.Abs(dy) > 0.0001f)
        {
            tY = (dy > 0 ? (top - transform.position.y)
                         : (bottom - transform.position.y)) / dy;
        }

        float tFinal = Mathf.Min(tX, tY);

        Vector2 edgeTarget =
            (Vector2)transform.position + dir * tFinal;

        float currentSpeed = 0f;

        while (Vector2.Distance(transform.position, edgeTarget) > arriveDistance)
        {
            currentSpeed += dashAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, dashSpeed);

            transform.position = Vector2.MoveTowards(
                transform.position,
                edgeTarget,
                currentSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}
