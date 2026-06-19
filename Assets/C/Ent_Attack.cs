using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Attack : MonoBehaviour
{
	[Header("攻擊預置物")]
    public GameObject attackPrefab;

    [Header("攻擊生成位置")]
    public Transform firePoint;

    [Header("開始延遲")]
    public float startDelay = 2f;

    [Header("攻擊間隔")]
    public float attackInterval = 1f;

    private Transform player;
	
	private AudioSource AttSource;
	
	public bool AttSo = true ;
	
    // Start is called before the first frame update
    void Start()
    {
		AttSource = GetComponent<AudioSource>();
		
        // 自動尋找場景中的 Player
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        StartCoroutine(AttackLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
			if (AttSo)
			{
				SpawnAttack();
			}
            else
			{
				SpawnAttack2();
			}
            yield return new WaitForSeconds(attackInterval);
        }
    }

    void SpawnAttack()
    {
		StartCoroutine(PlayRoutine());
		
        if (attackPrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        // 預設方向
        Quaternion rotation = Quaternion.identity;

        // 有玩家才計算朝向
        if (player != null)
        {
            Vector2 dir = (player.position - spawnPos).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            rotation = Quaternion.Euler(0, 0, angle);
        }

        Instantiate(attackPrefab, spawnPos, rotation);
    }
	
	void SpawnAttack2()
    {
		StartCoroutine(PlayRoutine());
		
        if (attackPrefab == null) return;
		
		Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        Instantiate(attackPrefab, spawnPos, transform.rotation);
    }
	
	IEnumerator PlayRoutine()
    {
        float delay = Random.Range(0.0f, 0.1f);
        yield return new WaitForSeconds(delay);

        if (AttSource != null)
            AttSource.Play();
    }
}
