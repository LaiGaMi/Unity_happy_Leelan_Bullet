using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyTransition;

public class Player_HP : MonoBehaviour
{
	[Header("血量")]
    public int hp = 100;

    [Header("血量UI（Image Fill）")]
    public Image hpBar;

    [Header("受傷冷卻時間")]
    public float hurtCooldown = 0.5f;

    [Header("受傷顏色透明度")]
    public float hurtAlpha = 192f;

    [Header("死亡特效")]
    public GameObject deathEffectPrefab; // ⭐保留擴充用

    private SpriteRenderer sr;
    private bool canBeHit = true;
    private Color originalColor;
	
	// 場景名稱
    public string sceneName;
    // 改成拖這個
    public TransitionSettings transitionSettings;
	public float loadDelay = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            originalColor = sr.color;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBeHit) return;

        if (other.CompareTag("Ent_Attack") || other.CompareTag("Ent_Attack_NO"))
        {
            TakeDamage(5);

            // Ent_Attack 才刪除
            if (other.CompareTag("Ent_Attack"))
            {
                Destroy(other.gameObject);
            }

            StartCoroutine(HurtCooldown());

            if (hp <= 0)
            {
                Die();
            }
        }
    }

    void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp < 0) hp = 0;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = hp / 100f;
        }
    }

    IEnumerator HurtCooldown()
    {
        canBeHit = false;

        // ⭐變透明（192 / 255）
        if (sr != null)
        {
            Color c = sr.color;
            c.a = hurtAlpha / 255f;
            sr.color = c;
        }

        yield return new WaitForSeconds(hurtCooldown);

        // 恢復
        if (sr != null)
        {
            sr.color = originalColor;
        }

        canBeHit = true;
    }

    void Die()
    {
        // ⭐死亡特效（保留擴充）
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
		
		EndGame();

        Destroy(gameObject);
    }
	
	void EndGame()
    {
        TransitionManager.Instance().Transition(sceneName, transitionSettings, loadDelay);
    }
}
