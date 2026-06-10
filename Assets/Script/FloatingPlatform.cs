using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveDistance = 2f; // 이동 거리
    public float moveSpeed = 2f;    // 이동 속도

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}
