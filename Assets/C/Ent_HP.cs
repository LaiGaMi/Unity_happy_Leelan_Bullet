using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_HP : MonoBehaviour
{
	[Header("最大血量")]
    public int hp = 5;

    [Header("受傷顏色")]
    public Color hurtColor = Color.red;

    [Header("受傷持續時間")]
    public float hurtDuration = 0.1f;
	
	[Header("死亡特效預置物")] 
    public GameObject deathEffectPrefab; 

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
	
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("player_attack"))
            return;

        // 扣血
        hp--;

        // 刪除子彈
        Destroy(other.gameObject);

        // 受傷閃色
        StartCoroutine(HurtFlash());

        if (hp <= 0)
        {
            Die();
        }
    }
	
	void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity); // ⭐ 新增
        }

        Destroy(gameObject);
    }
	
	IEnumerator HurtFlash()
    {
        spriteRenderer.color = hurtColor;

        yield return new WaitForSeconds(hurtDuration);

        spriteRenderer.color = originalColor;
    }
}
