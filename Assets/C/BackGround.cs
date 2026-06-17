using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
	public float moveSpeed = 5f;

    private float width;

    private void Start()
    {
        // 自動取得圖片寬度（世界座標）
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // 移動
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // 超出左邊 → 移到右邊
        if (transform.position.x <= -width)
        {
            Vector3 pos = transform.position;
            pos.x += width * 2f;
            transform.position = pos;
        }
    }
}
