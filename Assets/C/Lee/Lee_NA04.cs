using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_NA04 : MonoBehaviour
{
    [Header("前進速度")]
    public float speed = 10f;

    [Header("轉彎速度（角速度）")]
    public float turnSpeed = 90f;

    [Header("轉彎加速度（螺旋增長）")]
    public float turnAcceleration = 10f;

    [Header("最大轉彎速度（限制螺旋強度）")]
    public float maxTurnSpeed = 360f;

    private float currentTurnSpeed;

    void Start()
    {
        currentTurnSpeed = turnSpeed;
    }

    void Update()
    {
        MoveForward();
        ApplyTurn();
    }

    void MoveForward()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void ApplyTurn()
    {
        currentTurnSpeed += turnAcceleration * Time.deltaTime;
        currentTurnSpeed = Mathf.Min(currentTurnSpeed, maxTurnSpeed);

        transform.Rotate(0f, 0f, currentTurnSpeed * Time.deltaTime);
    }
}