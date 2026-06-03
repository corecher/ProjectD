using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float gameTime=300;
    public Text timer;
    public CoreManager coreManager;
    public BossController bossController;
    void Update()
    {
        gameTime -= Time.deltaTime;    
        timer.text = "남은 시간 : "+(int)gameTime+"초";
        if(gameTime < 0f)
        {   
            if(bossController!=null) bossController.SpawnBoss();
            Destroy(this);
        }
    }
}
