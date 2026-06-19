using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Del : MonoBehaviour
{
	[Header("幾秒後刪除")]
    public float destroyTime = 5f;
	
	[Header("刪除前生成物件")]
    public GameObject spawnBeforeDestroy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(destroyTime);

        if (spawnBeforeDestroy != null)
        {
            Instantiate(
                spawnBeforeDestroy,
                transform.position,
                transform.rotation
            );
        }

        Destroy(gameObject);
    }
}
