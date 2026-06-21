using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_Move1 : MonoBehaviour
{
    [Header("移動半徑")]
    public float moveRadius = 1f;

    [Header("移動時間 A")]
    public float moveTime = 2f;

    [Header("等待時間 B")]
    public float waitTime = 1f;

    [Header("縮放循環時間 C")]
    public float scaleCycleTime = 2f;

    [Header("最大縮放倍率")]
    public float maxScale = 1.5f;

    private Vector3 startLocalPos;
    private Vector3 baseLocalScale;

    private Transform parentTf;

    private void Start()
    {
        parentTf = transform.parent;

        startLocalPos = transform.localPosition;
        baseLocalScale = transform.localScale;

        StartCoroutine(MoveLoop());
        StartCoroutine(ScaleLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector2 circleCenter = GetWorldCenter();

            Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
            Vector2 targetWorld = circleCenter + randomOffset;

            Vector3 startWorld = transform.position;
            Vector3 targetWorld3 = new Vector3(targetWorld.x, targetWorld.y, transform.position.z);

            float timer = 0f;

            while (timer < moveTime)
            {
                timer += Time.deltaTime;

                float t = Mathf.Clamp01(timer / moveTime);
                float curveT = Mathf.SmoothStep(0f, 1f, t);

                transform.position = Vector3.Lerp(startWorld, targetWorld3, curveT);

                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator ScaleLoop()
    {
        while (true)
        {
            float timer = 0f;

            while (timer < scaleCycleTime)
            {
                timer += Time.deltaTime;

                float half = scaleCycleTime * 0.5f;

                float t = Mathf.PingPong(timer / half, 1f);
                float curveT = Mathf.SmoothStep(0f, 1f, t);

                float scaleValue = Mathf.Lerp(1f, maxScale, curveT);

                transform.localScale = baseLocalScale * scaleValue;

                yield return null;
            }
        }
    }

    Vector2 GetWorldCenter()
    {
        if (parentTf == null)
            return transform.position;

        return parentTf.position;
    }
}