using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Move : MonoBehaviour
{
	[Header("移動開關")]
    public bool enableMove = true;

    [Header("移動速度")]
    public float moveSpeed = 3f;

    [Header("移動曲線")]
    public AnimationCurve moveCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("移動範圍縮小比例（0~1）")]
    public float moveAreaScale = 0.7f;

    [Header("移動間隔時間")]
    public float moveInterval = 1f;

    [Header("卡死重置時間")]
    public float moveStuckLimit = 5f;

    private Camera cam;

    private Vector3 startPos;
    private Vector3 targetPos;

    private bool hasTarget;
    private bool waiting;

    private float moveTimer;
    private float moveDuration;

    private float moveStuckTimer;

    void Start()
    {
        cam = Camera.main;

        ResetMoveState();
    }

    // ⭐新增：重新啟用物件時強制可動
    void OnEnable()
    {
        ResetMoveState();

        // ⭐避免「啟用但不開始動」的狀態
        waiting = false;
        hasTarget = false;
    }

    void Update()
    {
        if (enableMove)
            HandleMovement();

        if (moveStuckTimer > moveStuckLimit)
            ResetMoveState();
    }

    void HandleMovement()
    {
        if (cam == null || waiting)
            return;

        if (!hasTarget)
        {
            float h = cam.orthographicSize;
            float w = h * cam.aspect;

            Vector3 c = cam.transform.position;

            float halfW = w * moveAreaScale;
            float halfH = h * moveAreaScale;

            targetPos = new Vector3(
                Random.Range(c.x, c.x + halfW),
                Random.Range(c.y - halfH, c.y + halfH),
                transform.position.z
            );

            startPos = transform.position;

            moveTimer = 0f;

            moveDuration = Mathf.Max(
                0.1f,
                Vector3.Distance(startPos, targetPos) / moveSpeed
            );

            hasTarget = true;
            moveStuckTimer = 0f;
        }

        moveTimer += Time.deltaTime;
        moveStuckTimer += Time.deltaTime;

        float t = moveTimer / moveDuration;

        float curveT =
            moveCurve.Evaluate(Mathf.Clamp01(t));

        transform.position =
            Vector3.Lerp(
                startPos,
                targetPos,
                curveT
            );

        if (t >= 1f)
        {
            hasTarget = false;
            StartCoroutine(MoveInterval());
        }
    }

    IEnumerator MoveInterval()
    {
        waiting = true;

        yield return new WaitForSeconds(moveInterval);

        waiting = false;
    }

    void ResetMoveState()
    {
        hasTarget = false;
        waiting = false;

        moveTimer = 0f;
        moveStuckTimer = 0f;
    }
}