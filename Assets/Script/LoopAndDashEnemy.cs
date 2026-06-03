using UnityEngine;

public class LoopAndDashEnemy : MonoBehaviour
{
    [Header("타깃 설정")]
    public Transform core;   

    [Header("이동 및 회전 설정")]
    public float hoverHeight = 3f;   
    public float moveUpSpeed = 8f;   
    public float loopRadius = 0.8f;
    public float loopSpeed = 15f;

    [Header("돌진 설정")]
    public float dashSpeed = 20f;
    public float dashDuration = 3f;
    public EnemyState myState;
    private enum AttackPhase 
    { 
        MoveToTop,
        Looping,
        Dashing
    }
    private AttackPhase currentPhase = AttackPhase.MoveToTop;
    private float stateTimer = 0f;
    private Vector2 loopCenter;
    private Vector2 dashDirection;
    void Start()
    {
        core = GameObject.FindGameObjectWithTag("Core").GetComponent<Transform>();
        myState = GetComponent<EnemyState>();
    }

    void Update()
    {
        if (core == null) return;

        switch (currentPhase)
        {
            case AttackPhase.MoveToTop:
                MoveToTargetTop();
                break;

            case AttackPhase.Looping:
                DoLoop();
                break;

            case AttackPhase.Dashing:
                DashTowards();
                break;
        }
    }
    private void MoveToTargetTop()
    {
        Vector2 targetPos = (Vector2)core.position + new Vector2(0, hoverHeight);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveUpSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 20f)
        {
            currentPhase = AttackPhase.Looping;
            stateTimer = 0f;
            loopCenter = (Vector2)transform.position - new Vector2(0, loopRadius);
        }
    }
    private void DoLoop()
    {
        stateTimer += Time.deltaTime;
        float angle = (stateTimer * loopSpeed) + (Mathf.PI / 2);
        float x = Mathf.Cos(angle) * loopRadius;
        float y = Mathf.Sin(angle) * loopRadius;

        transform.position = loopCenter + new Vector2(x, y);

        if (stateTimer * loopSpeed >= Mathf.PI * 2)
        {
            currentPhase = AttackPhase.Dashing;
            stateTimer = 0f;
            dashDirection = (core.position - transform.position).normalized;
        }
    }

    private void DashTowards()
    {
        transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (stateTimer >= dashDuration)
        {
            currentPhase = AttackPhase.MoveToTop;
            stateTimer = 0f;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Core"))
        {
            CoreState enemy = collision.gameObject.GetComponent<CoreState>();
            enemy.GetDamage(myState.damage);
            Destroy(gameObject);
        }
    }
}
