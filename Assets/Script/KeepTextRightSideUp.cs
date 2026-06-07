using UnityEngine;

public class KeepTextRightSideUp : MonoBehaviour
{
    private Vector3 initialScale;

    void Start()
    {
        // 텍스트의 처음 스케일 값을 저장해 둡니다.
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            Vector3 newScale = initialScale;

            // 부모의 좌우 스케일이 음수(-1)라면, 자식의 스케일도 음수로 만들어 상쇄시킵니다.
            if (transform.parent.localScale.x < 0)
            {
                newScale.x = -Mathf.Abs(initialScale.x);
            }
            else
            {
                newScale.x = Mathf.Abs(initialScale.x);
            }

            transform.localScale = newScale;
        }
    }
}
