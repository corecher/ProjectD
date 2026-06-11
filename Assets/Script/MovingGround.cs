using UnityEngine;

public class MovingGround : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float move = Mathf.PingPong(Time.time * speed, distance);

        transform.position = startPos + new Vector3(move, 0, 0);
    }
}
