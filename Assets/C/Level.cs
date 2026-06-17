using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class Level : MonoBehaviour
{
	[Header("波次列表（可自由增加）")]
    public List<GameObject> waves = new List<GameObject>();

    [Header("第一波生成位置")]
    public Transform spawnPoint; 

    private int currentWaveIndex = 0;
    private GameObject currentWave;
	
	// 場景名稱
    public string sceneName;

    // 改成拖這個
    public TransitionSettings transitionSettings;

    public float loadDelay = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        SpawnNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave == null) return;

        // 檢查是否還有子物件（敵人）
        if (currentWave.transform.childCount == 0)
        {
            Destroy(currentWave);
            SpawnNextWave();
        }
    }
	
	void SpawnNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            EndGame();
            return;
        }

        currentWave = Instantiate(
            waves[currentWaveIndex],
            spawnPoint.position,
            Quaternion.identity
        );

        currentWaveIndex++;
    }

    void EndGame()
    {
        TransitionManager.Instance().Transition(sceneName, transitionSettings, loadDelay);
    }
}
