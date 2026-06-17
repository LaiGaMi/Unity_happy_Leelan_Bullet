using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Move : MonoBehaviour
{
	public float disableX = -10f;

    private ConstantForce2D force2D;
	private Rigidbody2D rb;
	
    // Start is called before the first frame update
    void Start()
    {
        force2D = GetComponent<ConstantForce2D>();
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < disableX)
        {
            // 關掉持續力
            force2D.force = Vector2.zero;

            // 清掉殘留速度（重點）
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
