using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAA_font : MonoBehaviour
{
    public static int time;
    public static int HPUPUP;
	
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator AddLoop()
    {
        while (true)
        {
            time += 1; // static 變數
            yield return new WaitForSeconds(1f);
        }
    }
}
