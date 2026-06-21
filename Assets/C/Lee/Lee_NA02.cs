using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_NA02 : MonoBehaviour
{
    [Header("爆炸物件")]
    public GameObject explosionPrefab;

    private Camera cam;
    private bool hasTriggered;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (hasTriggered) return;
        if (cam == null) return;

        Vector3 vp = cam.WorldToViewportPoint(transform.position);

        bool outOfScreen =
            vp.x < 0f || vp.x > 1f ||
            vp.y < 0f || vp.y > 1f;

        if (outOfScreen)
        {
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (cam == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 vp = cam.WorldToViewportPoint(transform.position);

        Vector3 normal = Vector3.zero;

        if (vp.x < 0f) normal = Vector3.right;
        else if (vp.x > 1f) normal = Vector3.left;
        else if (vp.y < 0f) normal = Vector3.up;
        else if (vp.y > 1f) normal = Vector3.down;

        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;

        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.Euler(0, 0, angle)
            );
        }

        Destroy(gameObject);
    }
}