using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("상태 설정 (State)")]
    private bool isSpawned = false;

    [Header("체력 설정 (Health)")]         

    [Header("등장 설정 (Emerge)")]
    public float emergeOffset = 10f; 
    public float emergeSpeed = 3f;   
    private bool isEmerging = false; 

    [Header("이동 설정 (Movement)")]
    public float moveSpeed = 2f;      
    public float moveDistance = 3f;   

    [Header("공격 설정 (Attack)")]
    public float attackCooldown = 5f; 
    [Header("약점 설정 (Weak Point)")]
    public GameObject weakPointPrefab;              // 소환할 약점 프리팹
    public Vector2 spawnArea = new Vector2(3f, 3f); // 약점이 나타날 수 있는 가로/세로 범위

    private float targetStartY;       
    private float moveTime;           
    private float attackTimer;        
    public float weakPointRespawnTime = 3f;         // 🌟 약점이 파괴되고 몇 초 뒤에 다시 만들지 설정

    private bool hasWeakPoint = false;              // 🌟 현재 화면에 약점이 존재하는지 체크
    private float respawnTimer;
    public CoreState coreState;
    public CameraShake cameraShake;
    void Start()
    {
        targetStartY = transform.position.y;
        transform.position = new Vector3(transform.position.x, targetStartY - emergeOffset, transform.position.z);
        attackTimer = 0f; 
        moveTime = 0f;
        respawnTimer = 0f;
    }

    void Update()
    {
        if (!isSpawned) return;

        if (isEmerging) EmergeFromBelow();
        else
        {
            MoveUpAndDown();
            HandleAttack();
            HandleWeakPointRespawn();
        }
    }

    public void SpawnBoss()
    {
        if (isSpawned) return; 
        isSpawned = true;      
        isEmerging = true;     
        
        // 보스가 소환될 때 랜덤한 위치에 약점도 함께 생성합니다.
        
        SpawnRandomWeakPoint();
    }

    // 🌟 랜덤한 위치에 약점을 생성하는 함수
    void SpawnRandomWeakPoint()
    {
        if (weakPointPrefab == null)
        {
            Debug.LogWarning("약점 프리팹이 할당되지 않았습니다!");
            return;
        }

        // 1. 설정한 범위(spawnArea) 내에서 랜덤한 X, Y 값을 뽑습니다.
        float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float randomY = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        
        // 2. 보스의 중심을 기준으로 한 상대적인 위치(Local Position)를 설정합니다.
        Vector3 randomLocalPosition = new Vector3(randomX, randomY, 0f);

        // 3. 약점을 생성하고, 보스를 부모(Transform)로 설정하여 보스와 함께 움직이게 합니다.
        GameObject wpObject = Instantiate(weakPointPrefab, transform);
        wpObject.transform.localPosition = randomLocalPosition;
    }
    void HandleWeakPointRespawn()
    {
        // 화면에 약점이 있다면 타이머를 돌릴 필요가 없습니다.
        if (hasWeakPoint) return;

        // 약점이 없다면 시간을 계속 더해줍니다.
        respawnTimer += Time.deltaTime;

        // 설정한 리스폰 시간(예: 3초)이 지나면 새로운 약점을 소환합니다.
        if (respawnTimer >= weakPointRespawnTime)
        {
            SpawnRandomWeakPoint();
            respawnTimer = 0f; // 타이머 초기화
        }
    }
    public void OnWeakPointDestroyed()
    {
        hasWeakPoint = false; // 보스에게 약점이 없어졌음을 알림
        respawnTimer = 0f;    // 타이머를 0부터 다시 시작하게 설정
    }
    void EmergeFromBelow()
    {
        transform.Translate(Vector3.up * emergeSpeed * Time.deltaTime);
        if (transform.position.y >= targetStartY)
        {
            transform.position = new Vector3(transform.position.x, targetStartY, transform.position.z);
            isEmerging = false; 
        }
    }

    void MoveUpAndDown()
    {
        moveTime += Time.deltaTime;
        float newY = targetStartY + Mathf.Sin(moveTime * moveSpeed) * moveDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void HandleAttack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f; 
        }
    }
    void Attack()
    {
        StartCoroutine(cameraShake.Shake(0.1f, 1f));
        coreState.GetDamage(3);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, spawnArea.y, 0f));
    }
}