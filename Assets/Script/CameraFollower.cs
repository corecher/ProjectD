using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("따라갈 대상")]
    public Transform target;

    [Header("데드존 크기")]
    public float deadZoneX = 3f;
    public float deadZoneY = 2f;

    [Header("이동 속도")]
    public float smoothSpeed = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = camPos;

        // 플레이어가 데드존 밖으로 나갔을 때만 이동
        if (target.position.x > camPos.x + deadZoneX)
            targetPos.x = target.position.x - deadZoneX;
        else if (target.position.x < camPos.x - deadZoneX)
            targetPos.x = target.position.x + deadZoneX;

        if (target.position.y > camPos.y + deadZoneY)
            targetPos.y = target.position.y - deadZoneY;
        else if (target.position.y < camPos.y - deadZoneY)
            targetPos.y = target.position.y + deadZoneY;

        targetPos.z = camPos.z;

        transform.position = Vector3.SmoothDamp(
            camPos,
            targetPos,
            ref velocity,
            1f / smoothSpeed
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(deadZoneX * 2, deadZoneY * 2, 0)
        );
    }
}
