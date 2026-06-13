using System.Collections;
using UnityEngine;

public class FailEnding : MonoBehaviour
{
    public ButtonManager buttonManager;
    
    void Start()
    {
        StartCoroutine(RestartGame());    
    }
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(4f);
        buttonManager.ChangeNextScene(100);
    }
}
