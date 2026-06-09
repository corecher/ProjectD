using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void ChangeNextScene(int i)
    {
        StartCoroutine(FadeManager.Instance.FadeOutAndLoadScene(i));
    } 
}
