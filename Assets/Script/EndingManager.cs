using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    public bool successEnding;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetEnding(bool isSuccess)
    {
        successEnding = isSuccess;
    }
}
