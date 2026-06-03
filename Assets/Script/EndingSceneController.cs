using UnityEngine;

public class EndingSceneController : MonoBehaviour
{
    public GameObject successObject;
    public GameObject failObject;

    void Start()
    {
        if (EndingManager.Instance.successEnding)
        {
            successObject.SetActive(true);
            failObject.SetActive(false);
        }
        else
        {
            successObject.SetActive(false);
            failObject.SetActive(true);
        }
    }
}
