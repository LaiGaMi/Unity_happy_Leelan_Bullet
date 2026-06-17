using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
	[Header("子彈預置物")]
    public GameObject bulletPrefab;

    [Header("發射位置")]
    public Transform firePoint;

    [Header("長按多久開始連射")]
    public float holdTime = 1f;

    [Header("連射間隔")]
    public float fireInterval = 0.1f;

    private float holdTimer;
    private float fireTimer;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isPressing =
            Input.GetKey(KeyCode.Space) ||
            Input.GetMouseButton(0);

        bool isDown =
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0);

        bool isUp =
            Input.GetKeyUp(KeyCode.Space) ||
            Input.GetMouseButtonUp(0);

        // 按下瞬間
        if (isDown)
        {
            Shoot();

            holdTimer = 0f;
            fireTimer = 0f;
        }

        // 持續按住
        if (isPressing)
        {
            holdTimer += Time.deltaTime;

            // 超過指定秒數開始連射
            if (holdTimer >= holdTime)
            {
                fireTimer += Time.deltaTime;

                if (fireTimer >= fireInterval)
                {
                    Shoot();
                    fireTimer = 0f;
                }
            }
        }

        // 放開
        if (isUp)
        {
            holdTimer = 0f;
            fireTimer = 0f;
        }
    }
	
	void Shoot()
    {
        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }
}
