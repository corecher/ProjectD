using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreManager : MonoBehaviour
{
    public void GameOver(bool success)
    {
        EndingManager.Instance.successEnding = success;
        if(success) FadeManager.Instance.fadeImage.color = Color.white;
        else FadeManager.Instance.fadeImage.color = Color.black;
        StartCoroutine(FadeManager.Instance.FadeOutAndLoadScene("GameEndScene"));
    }
}
