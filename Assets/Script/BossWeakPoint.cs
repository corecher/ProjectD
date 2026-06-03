using UnityEngine;

public class BossWeakPoint : MonoBehaviour
{
    [Header("약점 설정")]
    public EnemyState boss;       // 데미지를 전달할 본체(보스) 스크립트
    private BossController bossController;
    public int damageToBoss = 30;     // 약점이 부서질 때 보스 본체에 들어갈 강력한 데미지
    public GameObject explosionEffect;
    void Start()
    {
        boss = GameObject.Find("BossEnemy").GetComponent<EnemyState>();
        bossController = boss.GetComponent<BossController>();
    }
    public void TakeDamage()
    {
        BreakWeakPoint();
    }

    void BreakWeakPoint()
    {
        bossController.OnWeakPointDestroyed();
        boss.GetDamage(damageToBoss);
        Instantiate(explosionEffect,transform.position,Quaternion.identity);
        Destroy(gameObject); 
    }
}
