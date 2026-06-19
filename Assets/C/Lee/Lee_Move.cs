using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_Move : MonoBehaviour
{
    [Header("最小X座標")]
    public float minX = -5f;

    [Header("最大X座標")]
    public float maxX = 5f;

    [Header("移動時間(單趟秒數)")]
    public float moveTime = 2f;

    [Header("移動曲線")]
    public AnimationCurve moveCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool movingRight = true;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / moveTime);
        float curveT = moveCurve.Evaluate(t);

        float x;

        if (movingRight)
        {
            x = Mathf.Lerp(minX, maxX, curveT);
        }
        else
        {
            x = Mathf.Lerp(maxX, minX, curveT);
        }

        transform.position = new Vector3(
            x,
            transform.position.y,
            transform.position.z
        );

        if (t >= 1f)
        {
            timer = 0f;
            movingRight = !movingRight;
        }
    }
}
