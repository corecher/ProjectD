using UnityEngine;
using System.Collections;
public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public Transform attackPoint;    // 공격의 중심점이 될 오브젝트
    public float attackRange = 0.5f; // 공격 반지름 범위
    public LayerMask enemyLayers;   // 공격할 적들의 레이어
    public LayerMask enemyWeakPoint;
    public int attackDamage = 20;    // 공격력

    [Header("공격 입력")]
    public KeyCode attackKey = KeyCode.Z;
    private Animator animator;
    public CameraShake cameraShake;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Collider2D[] bossWeakPoint = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyWeakPoint);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyState enemys  = enemy.gameObject.GetComponent<EnemyState>();
            StartCoroutine(cameraShake.Shake(0.1f, 0.15f));
            StartCoroutine(HitStop(0.05f));
            enemys.GetDamage(attackDamage);
            playerMovement.jumpCount--;
            SoundManager.Instance.PlaySFX("Attack",1f);
        }
        foreach (Collider2D weak in bossWeakPoint)
        {
            BossWeakPoint weaks = weak.gameObject.GetComponent<BossWeakPoint>();
            StartCoroutine(cameraShake.Shake(0.1f, 0.15f));
            StartCoroutine(HitStop(0.05f));
            weaks.TakeDamage();
            playerMovement.jumpCount--;
            SoundManager.Instance.PlaySFX("Attack",1f);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0f;
        
        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }
}
