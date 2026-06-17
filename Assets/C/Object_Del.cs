using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Del : MonoBehaviour
{
	[Header("幾秒後刪除")]
    public float destroyTime = 5f;

	
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
