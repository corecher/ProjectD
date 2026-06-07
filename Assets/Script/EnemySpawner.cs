using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IState
{
    [Header("소환 설정")]
    public GameObject objectToSpawn; // 소환할 프리팹
    public int spawnCount = 5;       // 한 번에 소환할 개수

    [Header("범위 설정")]
    public Vector2 spawnAreaSize = new Vector2(5f, 5f); // 소환 구역의 가로/세로 크기
    
    [Header("컴포넌트 및 오브젝트")]
    public Animator animator;
    public GameObject emergencyMark;
    public int hp;
    private CameraShake cameraShake;

    [Header("은신 설정")]
    public SpriteRenderer spawnerRenderer; // 스포너의 이미지를 담당하는 렌더러
    [Range(0f, 1f)]
    public float stealthAlpha = 0f;        // 은신 상태일 때의 목표 투명도
    public float fadeDuration = 1.5f;      // 사라지거나 나타나는 데 걸리는 시간 (초)
    public bool isStealthed = false;      // 현재 은신 여부 확인용 변수
    private Coroutine currentFadeCoroutine; // 현재 실행 중인 페이드 코루틴 저장용
    private Collider2D collider2D;
    [SerializeField]private GameObject explosionEffect;
    [SerializeField]private CoreManager coreManager;
    void Start()
    {
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj != null)
        {
            cameraShake = cameraObj.GetComponent<CameraShake>();
        }

        if (spawnerRenderer == null)
        {
            spawnerRenderer = GetComponent<SpriteRenderer>();
        }
        if (collider2D == null)
        {
            collider2D = GetComponent<Collider2D>();
        }
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
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);

            Vector2 randomPosition = new Vector2(
                transform.position.x + randomX, 
                transform.position.y + randomY
            );

            Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        }

        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.15f));
        }
    }

    public void GetDamage(int damage)
    {
        if (isStealthed) return;

        hp -= damage;
        Instantiate(explosionEffect,transform.position,Quaternion.identity);
        if (hp <= 0)
        {
            coreManager.GameOver(true,1);
            Destroy(gameObject);
        }
        if(hp%400==0)
        {
            StartCoroutine(FadeTime());
        }
    }
    IEnumerator FadeTime()
    {
        StartStealthFade(true);
        yield return new WaitForSeconds(20f);
        StartStealthFade(false);
    }
    IEnumerator EnemySpawnTime()
    {

        yield return new WaitForSeconds(7f);

        if (animator != null) animator.SetTrigger("Spawn");
        if (emergencyMark != null) emergencyMark.SetActive(true);
        
        // 3초 경고 대기
        yield return new WaitForSeconds(3f);

        // 3. 적 소환 및 표식 비활성화
        SpawnObjectsInArea();
        if (emergencyMark != null) emergencyMark.SetActive(false);
        
        // 루프 재시작
        StartCoroutine(EnemySpawnTime());
    }

    /// <summary>
    /// 안전하게 이전 페이드 효과를 끊고 새로운 페이드 효과를 시작하는 메서드
    /// </summary>
    private void StartStealthFade(bool stealth)
    {
        isStealthed = stealth;

        // 이미 페이드 효과가 재생 중이라면 중복 방지를 위해 멈춤
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        // 새로운 페이드 코루틴 실행
        currentFadeCoroutine = StartCoroutine(FadeRoutine(stealth));
    }

    /// <summary>
    /// 시간에 따라 알파값을 부드럽게 변화시키는 코루틴
    /// </summary>
    IEnumerator FadeRoutine(bool stealth)
    {
        if (spawnerRenderer == null) yield break;
        
        Color color = spawnerRenderer.color;
        float startAlpha = color.a;                  // 시작할 때의 현재 투명도
        float targetAlpha = stealth ? stealthAlpha : 1f; // 목표 투명도
        float elapsedTime = 0f;
        collider2D.isTrigger = stealth;
        // fadeDuration 시간 동안 매 프레임 알파값 보간
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Mathf.Lerp를 이용해 startAlpha에서 targetAlpha까지 부드럽게 변동
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            spawnerRenderer.color = color;
            yield return null; // 다음 프레임까지 대기
        }

        // 혹시 모를 오차를 위해 최종 목표치로 고정
        color.a = targetAlpha;
        spawnerRenderer.color = color;
        currentFadeCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
