using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_spawn : MonoBehaviour
{
    [Header("可生成物件清單")]
    public List<GameObject> spawnList = new List<GameObject>();

    [Header("生成位置")]
    public Transform spawnPoint;

    [Header("生成Tag")]
    public string spawnTag = "Enemy";

    [Header("重生等待時間")]
    public float respawnDelay = 5f;

    private GameObject currentObject;
    private bool spawning;

    void Start()
    {
        SpawnObject();
    }

    void Update()
    {
        // ⭐ 如果物件不存在 → 開始重生流程
        if (currentObject == null && !spawning)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    void SpawnObject()
    {
        if (spawnList.Count == 0) return;

        GameObject prefab = spawnList[Random.Range(0, spawnList.Count)];

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;

        currentObject = Instantiate(prefab, pos, Quaternion.identity);

        currentObject.tag = spawnTag;
    }

    IEnumerator RespawnRoutine()
    {
        spawning = true;

        yield return new WaitForSeconds(respawnDelay);

        SpawnObject();

        spawning = false;
    }
}
