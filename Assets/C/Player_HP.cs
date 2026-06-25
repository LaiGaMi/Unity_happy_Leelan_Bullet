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

    [Header("旋轉一圈")]
    public float hurtRotateAmount = 360f;

    [Header("死亡特效")]
    public GameObject deathEffectPrefab; // ⭐保留擴充用

    private SpriteRenderer sr;
    private bool canBeHit = true;
    private Color originalColor;

    private Coroutine hurtRoutine;

	// 場景名稱
    public string sceneName;
    public TransitionSettings transitionSettings;
	public float loadDelay = 0f;

	public GameObject HPEffectPrefab;
	private AudioSource AttSource;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
		AttSource = GetComponent<AudioSource>();

        if (sr != null)
            originalColor = sr.color;

        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag("HPUP"))
        {
			AAA_font.HPUPUP += 1;
            Destroy(other.gameObject);
            hp = 100;
            UpdateUI();
        }
		
        if (!canBeHit) return;

        if (other.CompareTag("Ent_Attack") || other.CompareTag("Ent_Attack_NO"))
        {
			AttSource.Play();
			
            TakeDamage(5);

            if (other.CompareTag("Ent_Attack"))
                Destroy(other.gameObject);

            if (hurtRoutine != null)
                StopCoroutine(hurtRoutine);

            hurtRoutine = StartCoroutine(HurtCooldown());

            if (hp <= 0)
                Die();
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
            hpBar.fillAmount = hp / 100f;
    }

    IEnumerator HurtCooldown()
    {
        canBeHit = false;

        float timer = 0f;
        float startZ = transform.eulerAngles.z;

        Color hitColor = sr != null ? sr.color : Color.white;
        if (sr != null)
        {
            hitColor.a = hurtAlpha / 255f;
            sr.color = hitColor;
        }

        while (timer < hurtCooldown)
        {
            timer += Time.deltaTime;

            float t = timer / hurtCooldown;

            // 曲線 easing（先快後慢）
            float curve = 1f - Mathf.Pow(1f - t, 3f);

            float angle = Mathf.Lerp(
                startZ,
                startZ + hurtRotateAmount,
                curve
            );

            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        // 恢復
        if (sr != null)
            sr.color = originalColor;

        transform.rotation = Quaternion.Euler(0f, 0f, startZ);

        canBeHit = true;
    }

    void Die()
    {
        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        EndGame();
        Destroy(gameObject);
    }

    void EndGame()
    {
        TransitionManager.Instance().Transition(sceneName, transitionSettings, loadDelay);
    }
}