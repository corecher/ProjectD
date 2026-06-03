using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("소환 설정")]
    public GameObject objectToSpawn; // 소환할 프리팹
    public int spawnCount = 5;       // 한 번에 소환할 개수

    [Header("범위 설정")]
    public Vector2 spawnAreaSize = new Vector2(5f, 5f); // 소환 구역의 가로/세로 크기

    void Start()
    {
        StartCoroutine(EnemySpawnTime());
    }

    private void SpawnObjectsInArea()
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("소환할 오브젝트가 설정되지 않았습니다!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // 1. 구역의 중심(현재 Spawner의 위치)을 기준으로 X, Y 랜덤 값을 뽑습니다.
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);

            // 2. 현재 위치에 랜덤 값을 더해 최종 소환 위치를 계산합니다.
            Vector2 randomPosition = new Vector2(
                transform.position.x + randomX, 
                transform.position.y + randomY
            );

            // 3. 해당 위치에 오브젝트 소환
            Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        }
    }
    IEnumerator EnemySpawnTime()
    {
        SpawnObjectsInArea();
        yield return new WaitForSeconds(10f);
        StartCoroutine(EnemySpawnTime());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // 선 색상
        Gizmos.DrawWireCube(transform.position, spawnAreaSize); // 중심점과 크기를 바탕으로 네모 그리기
    }
}
