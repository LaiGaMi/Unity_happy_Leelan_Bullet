using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
	public float moveSpeed = 5f;
    public float maxSpeed = 5f;

    private Rigidbody2D rb;

    private Vector2 lastMousePos;
    private bool isDragging;

    private Camera cam;

	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseDrag();
        HandleWASD();
        LimitSpeed();
		ClampToCamera();
    }
	
	void HandleWASD()
    {
        Vector2 move = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector2.up;
        if (Input.GetKey(KeyCode.S)) move += Vector2.down;
        if (Input.GetKey(KeyCode.A)) move += Vector2.left;
        if (Input.GetKey(KeyCode.D)) move += Vector2.right;

        rb.AddForce(move * moveSpeed);
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (!isDragging) return;

        Vector2 currentMousePos = Input.mousePosition;
        Vector2 delta = currentMousePos - lastMousePos;
        lastMousePos = currentMousePos;

        transform.position += new Vector3(delta.x, delta.y, 0f) * 0.01f;
    }

    void LimitSpeed()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    // =========================
    // ⭐ Camera 範圍限制（重點）
    // =========================
    void ClampToCamera()
    {
        if (cam == null) return;

        Vector3 pos = transform.position;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector3 min = cam.transform.position - new Vector3(width, height, 0);
        Vector3 max = cam.transform.position + new Vector3(width, height, 0);

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }
}
