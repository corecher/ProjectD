using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreManager : MonoBehaviour
{
    public void GameOver(bool success,int index)
    {
        EndingManager.Instance.successEnding = success;
        if(success) FadeManager.Instance.fadeImage.color = Color.white;
        else FadeManager.Instance.fadeImage.color = Color.black;
        if(index==0)
        StartCoroutine(FadeManager.Instance.FadeOutAndLoadScene("GameEndScene"));
        else
        StartCoroutine(FadeManager.Instance.FadeOutAndLoadScene("StoryScene"));
    }
}
