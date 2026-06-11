using UnityEngine;

public class StartSound : MonoBehaviour
{
    public string musicName;
    void Start()
    {
        SoundManager.Instance.PlayBGM(musicName);    
    }
}
