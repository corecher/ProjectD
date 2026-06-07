using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    public Text hp;
    public TextMesh playerHp;
    public PlayerMovement playerMovement;
    public EnemySpawner enemySpawner;
    void Update()
    {
        if(playerMovement != null)
        playerHp.text = playerMovement.hp+"/100";
        if(enemySpawner != null)
        hp.text = enemySpawner.hp+"/2000";
    }
}
