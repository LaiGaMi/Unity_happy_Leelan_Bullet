using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_NA01 : MonoBehaviour
{
    [Header("抵達時間（秒）")]
    public float arriveTime = 1f;

    [Header("命中特效（可選）")]
    public GameObject hitEffectPrefab;

    private Transform player;
    private Vector3 targetPos;

    private Vector3 startPos;

    void Start()
    {
        GameObject p = GameObject.Find("Player");

        if (p != null)
            player = p.transform;

        startPos = transform.position;

        if (player != null)
            targetPos = player.position;
        else
            targetPos = transform.position;

        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        float timer = 0f;

        while (timer < arriveTime)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / arriveTime);

            // 曲線減速（越接近越慢）
            float curveT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, curveT);

            yield return null;
        }

        transform.position = targetPos;

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, targetPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}