using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // 위로 올라가는 속도
    private float backgroundHeight; // 배경의 세로 길이

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundHeight = spriteRenderer.bounds.size.y;
    }

    void Update()
    {
        // 1. 배경을 위(Vector3.up)로 이동시킵니다.
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 2. 배경이 화면 '위'로 완전히 벗어나면 (y 좌표가 배경 높이 이상이 되면)
        if (transform.position.y >= backgroundHeight)
        {
            Reposition();
        }
    }

    void Reposition()
    {
        // 3. 틈새를 막기 위해 정확히 '아래'로 순간이동 시킵니다. (-backgroundHeight * 2f)
        Vector3 offset = new Vector3(0, -backgroundHeight * 2f+5, 0);
        transform.position = transform.position + offset;
    }
}
