using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void ChangeNextScene(string nextScene)
    {
        StartCoroutine(FadeManager.Instance.FadeOutAndLoadScene(nextScene));
    } 
}
